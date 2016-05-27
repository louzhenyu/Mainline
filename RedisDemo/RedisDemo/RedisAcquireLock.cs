using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Configuration;
using System.Threading;

namespace RedisDemo
{
    class RedisAcquireLock
    {
        static void Main1(string[] args)
        {
             PooledRedisClientManager pooleManager = new PooledRedisClientManager(10, 5, ConfigurationManager.AppSettings["RedisServerIP"].ToString());
           
            var redisClient = pooleManager.GetClient();
            redisClient.Set<string>("mykey1", "name");
            using (redisClient.AcquireLock("mykey1"))
            {
                Console.WriteLine("申请并发锁");


                var counter = redisClient.Get<string>("mykey1");
                redisClient.Set("mykey1", counter +"m");

                Console.WriteLine(redisClient.Get<int>("mykey1"));
                Console.ReadKey();

            }

        }
    }
}
