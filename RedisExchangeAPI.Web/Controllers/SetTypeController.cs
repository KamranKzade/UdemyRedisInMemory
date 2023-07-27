using System;
using System.Linq;
using StackExchange.Redis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RedisExchangeAPI.Web.Services;


namespace RedisExchangeAPI.Web.Controllers;


public class SetTypeController : Controller
{
	private readonly IDatabase _db;
	private string listKey = "hashName";
	private readonly RedisService _redisService;

	public SetTypeController(RedisService redisService)
	{
		_redisService = redisService;
		_db = redisService.GetDb(2);
	}

	public IActionResult Index()
	{
		HashSet<string> namesList = new HashSet<string>();
		if (_db.KeyExists(listKey))
			_db.SetMembers(listKey).ToList().ForEach(x => namesList.Add(x));

		return View(namesList);
	}

	[HttpPost]
	public IActionResult Add(string name)
	{
		_db.KeyExpire(listKey, DateTime.Now.AddMinutes(3));
		_db.SetAdd(listKey, name);

		return RedirectToAction("index");
	}

	public async Task<IActionResult> DeleteItem(string name)
	{
		await _db.SetRemoveAsync(listKey, name);
		return RedirectToAction("index");
	}

}