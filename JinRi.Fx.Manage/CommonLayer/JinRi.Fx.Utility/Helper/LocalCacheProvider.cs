using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace JinRi.Fx.Utility
{
    /// <summary>
    /// 本地缓存，基于：HttpRuntime.Cache
    /// </summary>
    public class LocalCacheProvider
    {
        /// <summary>
        /// 默认缓存过期时间，单位分钟，默认10分钟
        /// </summary>
        public const int DEFAULT_EXPIRE_TIME = 10;
        private System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        /// <summary>
        /// 本地缓存
        /// </summary>
        public LocalCacheProvider() { }
        /// <summary>
        /// 获取缓存项
        /// </summary>
        /// <param name="key">缓存KEY</param>
        /// <returns></returns>
        public T GetCache<T>(string key)
        {
            return (T)objCache.Get(key);
        }

        /// <summary>
        /// 新增缓存项，默认10分钟过期
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public void SetCache<T>(string key, T value)
        {
            objCache.Insert(key, value, null, DateTime.Now.AddMinutes(DEFAULT_EXPIRE_TIME), TimeSpan.Zero);
        }

        /// <summary>
        /// 新增缓存项
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expireTime">过期时间</param>
        public void SetCache<T>(string key, T value, DateTime expireTime)
        {
            objCache.Insert(key, value, null, expireTime, TimeSpan.Zero);
        }

        /// <summary>
        /// 移除指定缓存项
        /// </summary>
        /// <param name="key">Key</param>
        public void Remove(string key)
        {
            objCache.Remove(key);
        }

        /// <summary>
        /// 指定缓存Key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            return GetCache<object>(key) != null;
        }
    }
}
