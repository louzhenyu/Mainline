using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Utility
{
    /// <summary>
    /// 缓存管理类
    /// </summary>
    public class CacheManager
    {
        static int DefaultExpiresTime = 10;
        static BasicRedisClientManager redisManager = new BasicRedisClientManager(ConfigurationManager.AppSettings["FxRedisServerIP"].ToString());
        public static int appid = Convert.ToInt32(ConfigurationManager.AppSettings["appid"].ToString());
        /// <summary>
        /// 配置管理缓存Key
        /// </summary>
        public static string EtermConfigCacheKey = string.Format("140106_{0}_EtermUrl", appid.ToString());

       
        
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <param name="t">缓存值</param>
        /// <param name="expiresTimeSpan">过期时间</param>
        public static void SetCache<T>(string key, T t, TimeSpan expiresTimeSpan)
        {
            using (var redisClient = redisManager.GetClient())
            {
                redisClient.Set<T>(key, t, expiresTimeSpan);
            }
        }
        /// <summary>
        /// 设置缓存，过期时间默认10分钟
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <param name="t">缓存值</param>
        public static void SetCache<T>(string key, T t)
        {
            SetCache<T>(key, t, new TimeSpan(0, DefaultExpiresTime, 0));
        }
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <param name="t">缓存值</param>
        /// <param name="minutes">过期分钟数</param>
        public static void SetCache<T>(string key, T t, int minutes)
        {
            SetCache<T>(key, t, new TimeSpan(0, minutes, 0));
        }

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <typeparam name="T">缓存值类型</typeparam>
        /// <param name="key">缓存Key</param>
        /// <param name="defaultValue">默认值</param>
        public static T GetCache<T>(string key, T defaultValue = default(T))
        {
            try
            {
                using (var redisClient = redisManager.GetClient())
                {
                    return redisClient.Get<T>(key);
                }
            }
            catch
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// 是否包含指定Key缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool ContainsKey(string key)
        {
            try
            {
                using (var redisClient = redisManager.GetClient())
                {
                    object obj = redisClient.Get<object>(key);
                    if (obj != null)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void ClearCache(string key)
        {
            try
            {
                using (var redisClient = redisManager.GetClient())
                {
                     redisClient.Remove(key);
                }
            }
            catch
            {
                
            }
        }
    }
}
