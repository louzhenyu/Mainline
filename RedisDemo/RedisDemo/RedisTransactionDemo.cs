using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Configuration;

namespace RedisDemo
{
    class RedisTransactionDemo
    {
        static void Main1(string[] args)
        {
            PooledRedisClientManager pooleManager = new PooledRedisClientManager(10, 5, ConfigurationManager.AppSettings["RedisServerIP"].ToString());
           
            var redisClient = pooleManager.GetClient();
            redisClient.Add("key", 1);
            using (IRedisTransaction IRT = redisClient.CreateTransaction())
            {
                IRT.QueueCommand(r => r.Set("key", 20));
                IRT.QueueCommand(r => r.Increment("key", 1));

                IRT.Commit(); // 提交事务
            }
            Console.WriteLine(redisClient.Get<string>("key"));
            Console.ReadKey();
        }

    }
}

