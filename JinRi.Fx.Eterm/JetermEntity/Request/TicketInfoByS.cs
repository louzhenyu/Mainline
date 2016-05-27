using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{
    /// <summary>
    /// 获取票号 DETR:TN,S 请求对象
    /// </summary>
    [Serializable]
    public class TicketInfoByS
    {
        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNo { get; set; }
    }
}
