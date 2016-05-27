using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{
    /// <summary>
    /// 根据大编码获取票号
    /// </summary>
    [Serializable] 
    public class TicketByBigPnr
    {
        /// <summary>
        /// 大编码
        /// </summary>
        public string BigPnr { get; set; }
        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo { get; set; }
        /// <summary>
        /// 出发城市
        /// </summary>
        public string SCity { get; set; }
        /// <summary>
        /// 到达城市
        /// </summary>
        public string ECity { get; set; }       
    }
}
