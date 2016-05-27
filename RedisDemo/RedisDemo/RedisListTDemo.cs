using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack;
using ServiceStack.Redis;
using ServiceStack.Text;
using ServiceStack.Redis.Support;
using System.Configuration;
using ServiceStack.Model;
namespace RedisDemo
{
    class RedisListTDemo
    {
       //static RedisClient Redis1 = new RedisClient("127.0.0.1", 6379);//redis服务IP和端口
       
        static void Main1(string[] args)
        {
            PooledRedisClientManager pooleManager = new PooledRedisClientManager(10, 5, ConfigurationManager.AppSettings["RedisServerIP"].ToString());
            using (var redisClient = pooleManager.GetClient())
            {

                //存储对象（JSON序列化方法）它比object序列化方法效率高
                redisClient.Set<UserInfo>("userinfo", new UserInfo() { UserName = "李四", Age = 45 });
                UserInfo userinfo = redisClient.Get<UserInfo>("userinfo");
                Console.WriteLine("name=" + userinfo.UserName + "age=" + userinfo.Age);



                //object序列化方式存储
                var ser = new ObjectSerializer();    //位于namespace ServiceStack.Redis.Support;
                bool result = redisClient.Set<byte[]>("userinfo2", ser.Serialize(new UserInfo() { UserName = "张三", Age = 12 }));
                UserInfo userinfo2 = ser.Deserialize(redisClient.Get<byte[]>("userinfo2")) as UserInfo;
                Console.WriteLine("name=" + userinfo2.UserName + "age=" + userinfo2.Age);

                //也支持列表
                List<UserInfo> userinfoList = new List<UserInfo> {
            new UserInfo{UserName="zzl",Age=1,Id=1},
            new UserInfo{UserName="zhz",Age=3,Id=2},
            };
                redisClient.Set<byte[]>("userinfolist_serialize", ser.Serialize(userinfoList));
                List<UserInfo> userList = ser.Deserialize(redisClient.Get<byte[]>("userinfolist_serialize")) as List<UserInfo>;
                userList.ForEach(i =>
                {
                    Console.WriteLine("name=" + i.UserName + "age=" + i.Age);
                });
            }
        }
    }
}
