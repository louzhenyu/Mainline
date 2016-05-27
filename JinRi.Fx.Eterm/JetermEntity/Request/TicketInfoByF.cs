using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{
    /// <summary>
    /// 获取票号 DETR:TN,F 请求对象
    /// </summary>
    [Serializable]
    public class TicketInfoByF
    {
        /// <summary>
        /// 票号
        /// </summary>
        public string TicketNo { get; set; }
    }
}
