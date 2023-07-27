using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RedisExchangeAPI.Web.Services;


namespace RedisExchangeAPI.Web.Controllers;


public class SortedSetTypeController : BaseController
{
	public string listKey = "SortedSetTypeName";
	public SortedSetTypeController(RedisService redisService) : base(redisService) { }

	public IActionResult Index()
	{
		HashSet<string> list = new HashSet<string>();

		if (db.KeyExists(listKey))
		{
			db.SortedSetScan(listKey).ToList().ForEach(x =>
			{
				list.Add(x.ToString());
			});

			{
				// Redisde olan SortedSet Datalarina Sirali sekilde baxmaq ucundu ( ASC, DSC )

				// db.SortedSetRangeByRank(listKey, order: Order.Descending).ToList()
				//  	.ForEach(x =>
				//  	{
				//  		list.Add(x.ToString());
				//  	});
			}
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
		db.SortedSetRemove(listKey, name);
		return RedirectToAction("Index");
	}

}