using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;


namespace IDistributedCacheRedisApp.Web.Controllers;

public class ProductsController : Controller
{
	// Distributed Cache elave edirik, Redis --> Redis-e sade datalari yazmaq ve oxumaq 
	private IDistributedCache _distributedCache;

	public ProductsController(IDistributedCache distributedCache)
	{
		_distributedCache = distributedCache;
	}


	public async Task<IActionResult> Index()
	{
		// Cache optionlar vermek ucun istifade edirik, yasam omru, prioriteti ve s.

		DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
		cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

		// Redis-e data elave edirik.
		// _distributedCache.SetString("name", "kamran", cacheEntryOptions);

		// Redis-e data elave edirik (asinxron).
		await _distributedCache.SetStringAsync("surname", "karimzada", cacheEntryOptions);

		return View();
	}

	public async Task<IActionResult> Show()
	{
		// Redis-de olan data elde edirik.
		// string name = _distributedCache.GetString("name");

		// Redis-de olan data elde edirik(asinxron).
		string name = await _distributedCache.GetStringAsync("surname");


		ViewBag.Name = name;
		return View();
	}

	public async Task<IActionResult> Remove()
	{
		// Redis - de olan data silirik.
		// _distributedCache.Remove("name");
		
		// Redis - de olan data silirik(asinxron).
		await _distributedCache.RemoveAsync("surname");
		return View();
	}
}
