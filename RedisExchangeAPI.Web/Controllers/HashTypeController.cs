using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;

namespace RedisExchangeAPI.Web.Controllers;

public class HashTypeController :  BaseController
{
	public string key { get; set; } = "sozluk";

	public HashTypeController(RedisService redisService) : base(redisService) { }

	public IActionResult Index()
	{
		
		return View();
	}

	[HttpPost]
	public IActionResult Add(string name, string value)
	{

	}

}
