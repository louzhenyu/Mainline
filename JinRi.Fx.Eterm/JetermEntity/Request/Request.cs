using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{
    public class Command<T> where T:new()
    {
        public Command()
        {
            CacheTime = EtermCommand.CacheTime.none;
            ConfigLevel = EtermCommand.ConfigLevel.A;
            TimeOut = TimeSpan.FromSeconds(20);
        }

        /// <summary>
        /// 应用程序标识
        /// </summary>
        public int AppId {get;set;}
        /// <summary>
        /// 取缓存时间
        /// </summary>
        public EtermCommand.CacheTime CacheTime { get; set; }
        /// <summary>
        /// 配置级别
        /// </summary>
        public EtermCommand.ConfigLevel ConfigLevel { get; set; }

        /// <summary>
        /// 请求超时时间
        /// </summary>
        public TimeSpan TimeOut { get; set; }

        /// <summary>
        /// 系统OFFICE号
        /// </summary>
        public string officeNo { get; set; }
        /// <summary>
        /// 配置名称
        /// </summary>
        public string ConfigName { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public T request { get; set; }
    }
}
