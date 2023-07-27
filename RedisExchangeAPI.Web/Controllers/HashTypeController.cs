using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RedisExchangeAPI.Web.Services;


namespace RedisExchangeAPI.Web.Controllers;


public class HashTypeController : BaseController
{
	public string hashKey { get; set; } = "sozluk";

	public HashTypeController(RedisService redisService) : base(redisService) { }

	public IActionResult Index()
	{
		Dictionary<string, string> list = new();

		if (db.KeyExists(hashKey))
		{
			db.HashGetAll(hashKey).ToList().ForEach(x =>
			{
				list.Add(x.Name.ToString(), x.Value.ToString());
			});
		}

		{
			// Yalniz 1 data almaq
			// db.HashGet(hashKey, "dictionary-> Key");
		}

		return View(list);
	}

	[HttpPost]
	public IActionResult Add(string key, string value)
	{
		db.HashSet(hashKey, key, value);
		return RedirectToAction("Index");
	}

	public IActionResult DeleteItem(string name)
	{
		db.HashDelete(hashKey, name);
		return RedirectToAction("Index");
	}

}
