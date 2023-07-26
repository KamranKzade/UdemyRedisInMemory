﻿using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;


namespace RedisExchangeAPI.Web.Controllers;


public class StringTypeController : Controller
{
	private readonly IDatabase db;
	private readonly RedisService _redisService;

	public StringTypeController(RedisService redisService)
	{
		_redisService = redisService;

		// Daim eyni Db ile isleyeceyikse eger, contructorda yaziriq
		db = redisService.GetDb(0);
	}



	public IActionResult Index()
	{
		// Ferqli action methodlarda ferqli Db ile calisacayiqsa, bunu burda yaziriq
		// var db = _redisService.GetDb(0);

		db.StringSet("name", "Kamran Karimzada");
		db.StringSet("ziyaretci", 100);


		return View();
	}

	public IActionResult Show()
	{
		{
			// Dataya baxmaq 
			// var value = db.StringGet("name");
		}

		// Datanin araligina baxmaq 
		var value = db.StringGetRange("name", 0, 6);

		{
			// Datanin uzunluguna baxmaq
			// var value2 = db.StringLength("name");
		}

		if (value.HasValue)
		{
			ViewBag.Name = value.ToString();
		}

		{
			// Byte ile melumat yazmaq Redise 
			// var resimByte = default(byte[]);
			// db.StringSet("resim", resimByte);
		}

		{
			// Datani artirmaq
			// db.StringIncrement("ziyaretci", 20);
		}

		{
			// Asinxron methodan geriye data donurse
			// var count = db.StringDecrementAsync("ziyaretci", 10).Result;
		}

		{
			// Asinxron methoddan datani almaq istemirikse
			// db.StringDecrementAsync("ziyaretci", 10).Wait();
		}

		{
			// Datanin arxasina soz elave etmek, Concatanation
			// db.StringAppend("name", " Udemy");
		}

		return View();
	}

}