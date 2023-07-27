using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;


namespace RedisExchangeAPI.Web.Controllers;


public class StringTypeController : BaseController
{
	public string listKey = "StringTypeName";

	public StringTypeController(RedisService redisService) : base(redisService) { }

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