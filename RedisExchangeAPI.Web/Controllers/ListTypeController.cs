using System.Linq;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RedisExchangeAPI.Web.Services;


namespace RedisExchangeAPI.Web.Controllers;


public class ListTypeController : Controller
{
	private readonly IDatabase db;
	private string listKey = "names";
	private readonly RedisService _redisService;


	public ListTypeController(RedisService redisService)
	{
		_redisService = redisService;

		// Daim eyni Db ile isleyeceyikse eger, contructorda yaziriq
		db = redisService.GetDb(1);
	}



	public IActionResult Index()
	{
		List<string> namesList = new();

		if (db.KeyExists(listKey))
		{
			db.ListRange(listKey).ToList().ForEach(x =>
			{
				namesList.Add(x);
			});
		}

		return View(namesList);
	}

	[HttpPost]
	public IActionResult Add(string name)
	{
		// List-in axrina data elave etmek
		db.ListRightPush(listKey, name);

		{
			// Listin evveline data elave etmek
			// db.ListRightPush(listKey, name);
		}

		return RedirectToAction("index");
	}

	public IActionResult DeleteItem(string name)
	{
		// Listden data silmek 
		db.ListRemoveAsync(listKey, name).Wait();


		return RedirectToAction("index");
	}

	public IActionResult DeleteFirstItem()
	{
		// Listin basinnan data silmek
		db.ListLeftPop(listKey);

		{
			// Listin sonundan data silmek
			// db.ListRightPop(listKey);
		}

		return RedirectToAction("index");
	}
}
