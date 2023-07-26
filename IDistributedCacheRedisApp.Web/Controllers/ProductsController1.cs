using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace IDistributedCacheRedisApp.Web.Controllers;

public class ProductsController1 : Controller
{
	// Distributed Cache elave edirik, Redis --> Redis-e sade datalari yazmaq ve oxumaq 
	private IDistributedCache _distributedCache;

	public ProductsController1(IDistributedCache distributedCache)
	{
		_distributedCache = distributedCache;
	}


	public IActionResult Index()
	{
		return View();
	}
}
