using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;


namespace InMemoryApp.Web.Controllers;


public class ProductController : Controller
{
	private IMemoryCache _memoryCache;

	public ProductController(IMemoryCache memoryCache)
	{
		_memoryCache = memoryCache;
	}

	public IActionResult Index()
	{
		{
			// Way 1: Bu (zaman) adda key varsa alib, yenileyirik --> String vasitesi ile

			// if (String.IsNullOrEmpty(_memoryCache.Get<string>("Zaman")))
			// {
			// 	_memoryCache.Set<string>("Zaman", DateTime.Now.ToString());
			// }
		}

		// Way 2: Bu (zaman) adda key varsa alib, yenileyirik --> TryGetValue vasitesi ile 

		if (!_memoryCache.TryGetValue<string>("Zaman", out string zamanCache))
		{
			MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions();

			{
				// Expiration vermeden Set etmek
				// _memoryCache.Set<string>("Zaman", DateTime.Now.ToString());
			}

			{
				// AbsoluteExpiration elave etmek 
				// cacheOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

				// SlidingExpiration elave etmek 
				// cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(10);
			}

			// SlidingExpiratio && AbsoluteExpiration --> 1 yerde istifade meqsedi,
			//											  sliding erzinde data cacheden silinmese,
			//											  absolute uygun olaraq o vaxt silinecek.

			cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(3);
			cacheOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

			{
				// Cache Priority --> Silinme ardicilligi asagidaki kimidir.

				// cacheOptions.Priority = CacheItemPriority.Low;
				// cacheOptions.Priority = CacheItemPriority.Normal;
				// cacheOptions.Priority = CacheItemPriority.High;
				// cacheOptions.Priority = CacheItemPriority.NeverRemove;
			}
			cacheOptions.Priority = CacheItemPriority.High;


			// RegisterPostEvictionCallback ==> Cache - dan silinenlerin silinme sebebini gosterirki,
			//									hansi sebebden silinib.

			cacheOptions.RegisterPostEvictionCallback((key, value, reason, state) =>
			{
				_memoryCache.Set("callBack", $"{key}--> {value} ==> Sebeb: {reason}");
			});


			// Complex Type Caching Proses
			{
				Product p = new Product
				{
					Id = 1,
					Name = "Kalem",
					Price = 200
				};

				// Complex Type write Cache
				_memoryCache.Set<Product>("product:1", p);
			}

			_memoryCache.Set<string>("Zaman", DateTime.Now.ToString(), cacheOptions);
		}

		{
			// Cache-den melumati silmek
			// _memoryCache.Remove("Zaman");
		}

		return View();
	}


	public IActionResult Show()
	{
		{
			// Cache - da olan data varsa aliriq, yoxdursa yaradib deyer veririk
			//
			// 	_memoryCache.GetOrCreate<string>("Zaman", entry =>
			// 	{
			// 		return DateTime.Now.ToString();
			// 	});
		}

		_memoryCache.TryGetValue<string>("Zaman", out string zamanCache);
		_memoryCache.TryGetValue<string>("callBack", out string callBack);

		ViewBag.zaman = zamanCache;
		ViewBag.callback = callBack;

		// Complex type -i cache-den oxuyub, view-a oturmek
		{
			ViewBag.product = _memoryCache.Get<Product>("product:1");
		}

		return View();
	}
}
