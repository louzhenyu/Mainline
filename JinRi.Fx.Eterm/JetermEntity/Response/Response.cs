using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{
    /// <summary>
    /// Eterm结果集
    /// </summary>
    /// <typeparam name="T">业务参数</typeparam>
    [Serializable]
    public class CommandResult<T>
        where T : new()
    {
        public CommandResult()
        {
            SaveTime = EtermCommand.CacheTime.min30;
            reqtime = DateTime.Now;
        }


        /// <summary>
        /// 状态 成功或失败
        /// </summary>
        public bool state { get; set; }

        /// <summary>
        /// 错误原因
        /// </summary>
        public Error error { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string config { get; set; }

        /// <summary>
        /// OFFICE号
        /// </summary>
        public string OfficeNo { get; set; }

        /// <summary>
        /// 结果集
        /// </summary>
        public T result { get; set; }

        /// <summary>
        /// 请求时间
        /// 此时间为第一次请求黑屏时间，用于缓存时不包括此时间
        /// </summary>
        public DateTime reqtime { get; set; }

        /// <summary>
        /// 缓存时间
        /// 此时间用于缓存结果时的时间
        /// </summary>
        public EtermCommand.CacheTime SaveTime { get; set; }

        /// <summary>
        /// JEterm Server地址
        /// </summary>
        public string ServerUrl { get; set; }
    }
}
