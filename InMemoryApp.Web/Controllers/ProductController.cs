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
		// Way 1: Bu (zaman) adda key varsa alib, yenileyirik --> String vasitesi ile

		if (String.IsNullOrEmpty(_memoryCache.Get<string>("Zaman")))
		{
			_memoryCache.Set<string>("Zaman", DateTime.Now.ToString());
		}

		// Way 2: Bu (zaman) adda key varsa alib, yenileyirik --> TryGetValue vasitesi ile 

		if (!_memoryCache.TryGetValue<string>("Zaman", out string zamanCache))
		{
			_memoryCache.Set<string>("Zaman", DateTime.Now.ToString());
		}

		{
			// Cache-den melumati silmek

			// _memoryCache.Remove("Zaman");
		}

		return View();
	}


	public IActionResult Show()
	{
		// Cache - da olan data varsa aliriq, yoxdursa yaradib deyer veririk
		// {
		// 	_memoryCache.GetOrCreate<string>("Zaman", entry =>
		// 	{
		// 		return DateTime.Now.ToString();
		// 	});
		// }

		ViewBag.zaman = _memoryCache.Get<string>("Zaman");
		return View();
	}
}
