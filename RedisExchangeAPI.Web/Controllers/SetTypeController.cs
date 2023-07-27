using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RedisExchangeAPI.Web.Services;


namespace RedisExchangeAPI.Web.Controllers;


public class SetTypeController : BaseController
{
	public SetTypeController(RedisService redisService) : base(redisService) { }

	public IActionResult Index()
	{
		HashSet<string> namesList = new ();
		if (db.KeyExists(listKey))
			db.SetMembers(listKey).ToList().ForEach(x => namesList.Add(x.ToString()));

		return View(namesList);
	}

	[HttpPost]
	public IActionResult Add(string name)
	{
		db.KeyExpire(listKey, DateTime.Now.AddMinutes(3));
		db.SetAdd(listKey, name);

		return RedirectToAction("index");
	}

	public async Task<IActionResult> DeleteItem(string name)
	{
		await db.SetRemoveAsync(listKey, name);
		return RedirectToAction("index");
	}

}
