using Flight.Product.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutofacDemo
{
    class UtilityDemo
    {
        private static ILogProvider logger = UtilityService.GetLogger(typeof(UtilityDemo));
        static void Main(string[] args)
        {
            ICacheProvider localCache = UtilityService.GetLocalCache();
            localCache.SetCache<string>("LocalCacheKey", "value", DateTime.Now.AddMinutes(2));
            Console.WriteLine(localCache.GetCache<string>("LocalCacheKey"));

            ICacheProvider redisCache = UtilityService.GetRedisCache();
            redisCache.SetCache<string>("RedisCacheKey", "value", DateTime.Now.AddMinutes(2));
            Console.WriteLine(redisCache.GetCache<string>("RedisCacheKey"));

            logger.Info("logger demo");
            Console.ReadLine();
        }
    }
}
