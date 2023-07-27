using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedisExampleApp.Cache
{
	//// Bir nece program ayri-ayri istifade ede bilsin deye ddl e cixartdiq
	public class RedisService
	{
		// Redis-e baglanmaq ucun olan class
		private readonly ConnectionMultiplexer _connectionMultiplexer;

		public RedisService(string url)
		{
			_connectionMultiplexer = ConnectionMultiplexer.Connect(url);
		}

		public IDatabase GetDb(int db)
		{
			return _connectionMultiplexer.GetDatabase(db);
		}
	}
}
