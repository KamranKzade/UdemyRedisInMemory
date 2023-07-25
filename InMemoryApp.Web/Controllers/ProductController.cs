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
				// AbsoluteExpiration elave etmek 
				// cacheOptions.AbsoluteExpiration = DateTime.Now.AddSeconds(10);

				// SlidingExpiration elave etmek 
				// cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(10);
			}

			// SlidingExpiratio && AbsoluteExpiration --> 1 yerde istifade meqsedi,
			//											  sliding erzinde data cacheden silinmese,
			//											  absolute uygun olaraq o vaxt silinecek.

			cacheOptions.SlidingExpiration = TimeSpan.FromSeconds(10);
			cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

			{
				// Expiration vermeden Set etmek
				// _memoryCache.Set<string>("Zaman", DateTime.Now.ToString());
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
		ViewBag.zaman = zamanCache;

		return View();
	}
}
