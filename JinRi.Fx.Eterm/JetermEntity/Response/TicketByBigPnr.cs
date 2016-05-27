using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{
    /// <summary>
    /// 根据大编码获取票号
    /// </summary>
    [Serializable]
    public class TicketByBigPnr
    {
        /// <summary>
        /// 乘客集合（信息包括姓名、票号、证件号）
        /// </summary>
        public List<Passenger> PassengerList { get; set; }
        /// <summary>
        /// 航班信息
        /// </summary>
        public List<Flight> FlightList { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public Price Price { get; set; }
        /// <summary>
        /// 客票状态
        /// </summary>
        public EtermCommand.TicketStatus TicketStatus { get; set; }

        /// <summary>
        /// TicketByBigPnr命令返回结果
        /// </summary>
        public string ResultBag { get; set; }
    }
}
