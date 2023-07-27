using System;
using System.Linq;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RedisExchangeAPI.Web.Services;


namespace RedisExchangeAPI.Web.Controllers;


public class SortedSetTypeController : Controller
{
	private readonly IDatabase db;
	private string listKey = "SortedSetname";
	private readonly RedisService _redisService;

	public SortedSetTypeController(RedisService redisService)
	{
		_redisService = redisService;

		// Daim eyni Db ile isleyeceyikse eger, contructorda yaziriq
		db = redisService.GetDb(3);
	}

	public IActionResult Index()
	{
		HashSet<string> list = new HashSet<string>();

		if (db.KeyExists(listKey))
		{
			//	Sdb.SortedSetScan(listKey).ToList().ForEach(x =>
			//	S{
			//	S	list.Add(x.ToString());
			//	S});

			db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList()
				.ForEach(x =>
				{
					list.Add(x.ToString());
				});
		}

		return View(list);
	}

	[HttpPost]
	public IActionResult Add(string name, int score)
	{
		db.SortedSetAdd(listKey, name, score);
		db.KeyExpire(listKey, DateTime.Now.AddMinutes(2));

		return RedirectToAction("Index");
	}

	public IActionResult DeleteItem(string name)
	{
		db.SortedSetRemove(listKey,name);

		return RedirectToAction("Index");
	}
}
