using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ServiceStack.Redis;
using System.Configuration;

namespace RedisDemo
{
    /// <summary>
    /// Redis数据类型是List型的Demo
    /// </summary>
    class RedisListDemo
    {
        static void Main1(string[] args)
        {
            PooledRedisClientManager pooleManager = new PooledRedisClientManager(10, 5, ConfigurationManager.AppSettings["RedisServerIP"].ToString());
            using (var redisClient = pooleManager.GetClient())
            {

                var list = redisClient.Lists["additemtolist"];
                list.Clear();

                var list1 = redisClient.Lists["additemtolist"];

                List<string> storeMembers = new List<string>() { "one", "two", "three" };
                //将单个项往Redis内部中添加
                storeMembers.ForEach(x => redisClient.AddItemToList("additemtolist", x));
                //一次性将参数中的List<T>中的多个值添加入内部的List<T>
                redisClient.AddRangeToList("additemtolist1", storeMembers);
                

                //得到指定的key所对应的value集合
                var members = redisClient.GetAllItemsFromList("OrderIDList");
                // 获取指定索引位置数据
                var item = redisClient.GetItemFromList("additemtolist", 2);
                redisClient.RemoveItemFromList("additemtolist", item);//移除指定键值,在服务器
                var members1 = redisClient.GetAllItemsFromList("additemtolist1");
                var ss = redisClient.PopItemFromList("additemtolist");//移除末尾元素并返回
               

                members = redisClient.GetAllItemsFromList("additemtolist");

                members.ForEach(s => Console.WriteLine("<br/>additemtolist :" + s));




            }
        }
    }
}
