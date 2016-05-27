using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{
    /// <summary>
    /// 自动出票请求模型
    /// </summary>
    [Serializable]
    public class AutoTicket
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        public string Config { get; set; }
    }
}
