using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;

namespace RedisExchangeAPI.Web.Controllers;

public class HashTypeController :  BaseController
{
	public HashTypeController(RedisService redisService) : base(redisService) { }

	public IActionResult Index()
	{
		
		return View();
	}



}
