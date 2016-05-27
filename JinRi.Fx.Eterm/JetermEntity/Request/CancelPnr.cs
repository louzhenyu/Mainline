using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{
    /// <summary>
    /// 擦编码请求对象
    /// </summary>
    [Serializable]
    public class CancelPnr
    {
        /// <summary>
        /// 记录编码
        /// </summary>
        public string Pnr { get; set; }

        /// <summary>
        /// 是否擦出票的编码
        /// </summary>
        public bool CancelOut { get; set; }

    }
}
