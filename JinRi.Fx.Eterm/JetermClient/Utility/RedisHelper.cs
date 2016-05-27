using System;
using ServiceStack.Redis;
using System.Configuration;
using System.Collections.Generic;
using ServiceStack.Model;
using ServiceStack.Redis.Support;

namespace JetermClient.Utility
{
    public class RedisHelper
    {
        #region 定义
        private static BasicRedisClientManager basicRedisClientManager = new BasicRedisClientManager(ConfigurationManager.AppSettings["FxRedisServerIP"].ToString());
        #endregion

        #region Redis字符串
        /// <summary>
        /// 字符串设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ts"></param>
        /// <returns>设置是否成功</returns>
        public static bool stringSet(string key, string value,TimeSpan ts)
        {
            using (var redisClient = basicRedisClientManager.GetClient())
            {
                return redisClient.Set<string>(key, value , ts);                
            }
        }

        /// <summary>
        /// 字符串获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string stringGet(string key)
        {
            using (var redisClient = basicRedisClientManager.GetClient())
            {
                return redisClient.Get<string>(key); 
            }
        }

        #endregion

        #region Redis事件处理
        public static bool TransactionSet(string key,string value)
        {
            var redisClient = basicRedisClientManager.GetClient();            
            using (IRedisTransaction IRT = redisClient.CreateTransaction())
            {
                IRT.QueueCommand(r => r.Set<string>(key, value));
                IRT.QueueCommand(r => r.Increment(key, 1));

                return IRT.Commit(); // 提交事务
            }
        }
        #endregion

        #region RedisHash处理
        /// <summary>
        /// Redis Set操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="values"></param>
        public static void HashSet(string key,List<string> values)
        {           
            using (var redisClient = basicRedisClientManager.GetClient())
            {
                foreach(string val in values)
                {
                    redisClient.AddItemToSet(key, val);
                }
            }
        }
        /// <summary>
        /// Redis Set操作
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<string> HashGet(string key)
        {
            List<string> values = new List<string>();

            using (var redisClient = basicRedisClientManager.GetClient())
            {
                IHasNamed<IRedisSet> rr = redisClient.Sets;
                HashSet<string> HashSetString = rr[key].GetAll();
                foreach (string str in HashSetString)
                {
                    values.Add(str);
                }
            }

            return values;
        }
        #endregion

        #region Redis 泛型T处理
        /// <summary>
        /// 泛型T赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool tSet<T>(string key, T t, TimeSpan ts)
        {
            using (var redisClient = basicRedisClientManager.GetClient())
            {
                return redisClient.Set<T>(key, t, ts);
            }
        }
        /// <summary>
        /// 泛型T取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T tGet<T>(string key)
        {
           using (var redisClient = basicRedisClientManager.GetClient())
            {
                return redisClient.Get<T>(key);
            }
        }
             

        /// <summary>
        /// 泛型T赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool listSet<T>(string key,List<T> list)
        {
            using (var redisClient = basicRedisClientManager.GetClient())
            {
                //object序列化方式存储
                var ser = new ObjectSerializer();
                return redisClient.Set<byte[]>(key, ser.Serialize(list));
            }
        }

        /// <summary>
        /// 泛型T赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<T> listGet<T>(string key)
        {
            using (var redisClient = basicRedisClientManager.GetClient())
            {
                //object序列化方式存储
                var ser = new ObjectSerializer();
                return ser.Deserialize(redisClient.Get<byte[]>(key)) as List<T>;
            }
        }
        #endregion
    }
}
