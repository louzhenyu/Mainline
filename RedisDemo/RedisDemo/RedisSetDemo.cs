using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using ServiceStack.Redis;
using ServiceStack.Model;

namespace RedisDemo
{
    /// <summary>
    /// Redis数据类型是Set型的Demo
    /// </summary>
    class RedisSetDemo
    {
        /// Redis主服务器地址
        /// </summary>
        /// <returns></returns>
        public static  string[] RedisServerIP1()
        {
            return ConfigurationManager.AppSettings["RedisServerIP"].ToString().Split(',');
        }


        static void Main1(string[] args)
        {
            string rs = ConfigurationManager.AppSettings["RedisServerIP"].ToString();
            BasicRedisClientManager basicRedisClientManager = new BasicRedisClientManager(rs);

            using (var redisClient = basicRedisClientManager.GetClient())
            {

                redisClient.AddItemToSet("蜀国", "刘备");
                redisClient.AddItemToSet("蜀国", "关羽");
                redisClient.AddItemToSet("蜀国", "张飞");
                redisClient.GetAllKeys();
                IHasNamed<IRedisSet> rr = redisClient.Sets;
                HashSet<string> HashSetString = rr["蜀国"].GetAll();
                foreach (string str in HashSetString)
                {
                    Console.WriteLine(str);
                }
            }
        }
    }
}
