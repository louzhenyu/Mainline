using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceStack.Redis;
using System.Configuration;

namespace RedisDemo
{
    /// <summary>
    /// Redis数据类型是String型的Demo
    /// </summary>
    class RedisStringDemo
    {
        static void Main(string[] args)
        {
            PooledRedisClientManager pooleManager = new PooledRedisClientManager(10, 5, ConfigurationManager.AppSettings["RedisServerIP"].ToString());
            using (var redisClient = pooleManager.GetClient())
            {
                redisClient.Set<string>("city", "shanghai1", new TimeSpan(0, 5, 0));
                string redisValue = redisClient.Get<string>("city");
                Console.WriteLine(redisValue);
                Console.ReadKey();
            }
         
        }
    }
}
