using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{
    /// <summary>
    /// 获取票号 DETR:TN 返回对象
    /// </summary>
    [Serializable]
    public class TicketInfo
    {
        public TicketInfo()
        {

        }

        /// <summary>
        /// 票号/编码
        /// </summary>
        public string TicketNo { get; set; }

        /// <summary>
        /// 航班信息
        /// </summary>
        public List<Flight> FlightList { get; set; }

        /// <summary>
        /// 乘客姓名
        /// </summary>
        public string PassengerName { get; set; }

        /// <summary>
        /// 大编码
        /// </summary>
        public string BigPnr { get; set; }

        /// <summary>
        /// 联程票List
        /// </summary>
        public List<string> LianChengTicketList { get; set; }

        /// <summary>
        /// TicketInfo命令返回结果
        /// </summary>
        public string ResultBag { get; set; }
    }
}
