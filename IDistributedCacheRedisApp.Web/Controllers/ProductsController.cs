using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using IDistributedCacheRedisApp.Web.Models;
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
		cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

		// Sade tiplerin redise elave edilmesi
		{
			// Redis-e data elave edirik.
			// _distributedCache.SetString("name", "kamran", cacheEntryOptions);

			// Redis-e data elave edirik (asinxron).
			// await _distributedCache.SetStringAsync("surname", "karimzada", cacheEntryOptions);
		}

		// Complex tiplerin Redis-e yazilmasi
		Product product = new Product { Id = 2, Name = "Karandas", Price = 150 };

		// JsonFormati ile yazmaq
		string jsonProduct = JsonConvert.SerializeObject(product);
		// await _distributedCache.SetStringAsync($"product:{product.Id}", jsonProduct, cacheEntryOptions);

		// Byte arrayina ceviririb, yazmaq
		Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
		await _distributedCache.SetAsync("product:1", byteProduct);

		return View();
	}

	public async Task<IActionResult> Show()
	{
		// Sade tiplerin redisden oxunmasi
		{
			// Redis-de olan data elde edirik.
			// string name = _distributedCache.GetString("name");

			// Redis-de olan data elde edirik(asinxron).
			// string name = await _distributedCache.GetStringAsync("surname");
		}



		// Complex tiplerin Redis-e oxunmasi(Json vasitesi ile)
		// string jsonProduct = await _distributedCache.GetStringAsync("product:1");
		// JsonDeserialize vasitesile class-a menimsetme 
		// Product p = JsonConvert.DeserializeObject<Product>(jsonProduct)!;

		// Complex tiplerin Redis-e oxunmasi(byte arrayi kimi)
		Byte[] bytes = _distributedCache.Get("product:1");
		var byteToString = Encoding.UTF8.GetString(bytes);
		Product p = JsonConvert.DeserializeObject<Product>(byteToString)!;

		ViewBag.Product = p;
		return View();
	}

	public async Task<IActionResult> Remove()
	{
		// Sade tiplerin redisden silinmesi
		{
			// Redis - de olan data silirik.
			// _distributedCache.Remove("name");

			// Redis - de olan data silirik(asinxron).
			//await _distributedCache.RemoveAsync("surname");
		}

		// Complex tiplerin redisden silinmesi
		_distributedCache.Remove("product:1");

		return View();
	}

	public IActionResult ImageCache()
	{
		// Cache-e omur veririk
		var cacheEntryOptions = new DistributedCacheEntryOptions();
		cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

		// Path-i aliriq 
		string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/download.jpg");

		// Path byte arrayine ceviririk
		byte[] imageByte = System.IO.File.ReadAllBytes(path);
		_distributedCache.Set("resim", imageByte, cacheEntryOptions);

		return View();
	}

	public IActionResult ImageUrl()
	{
		// Redisde olan sekili byte[] ceviririk
		var byteImage = _distributedCache.Get("resim");

		return File(byteImage, "image/jpg");
	}
}
