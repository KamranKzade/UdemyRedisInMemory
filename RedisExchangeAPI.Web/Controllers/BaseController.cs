using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;

namespace RedisExchangeAPI.Web.Controllers;

public class BaseController : Controller
{
	protected readonly IDatabase db;
	protected readonly RedisService _redisService;

	public BaseController(RedisService redisService)
	{
		_redisService = redisService;
		db = redisService.GetDb(1);
	}

}
