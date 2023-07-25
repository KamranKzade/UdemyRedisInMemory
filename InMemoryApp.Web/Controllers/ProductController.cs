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
		_memoryCache.Set<string>("Zaman", DateTime.Now.ToString());
		return View();
	}


	public IActionResult Show()
	{
		ViewBag.zaman = _memoryCache.Get<string>("Zaman");
		return View();
	}
}
