using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{
    /// <summary>
    /// 获取票号 DETR:TN,S 返回对象
    /// </summary>
    [Serializable]
    public class TicketInfoByS
    {
        /// <summary>
        /// 票号/编码
        /// </summary>
        public string TicketNo { get; set; }

        /// <summary>
        /// 乘客姓名
        /// </summary>
        public string PassengerName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public Price Price { get; set; }

        /// <summary>
        /// 航班信息
        /// </summary>
        public List<Flight> FlightList { get; set; }

        /// <summary>
        /// 客票状态
        /// </summary>
        public EtermCommand.TicketStatus TicketStatus { get; set; }

        /// <summary>
        /// TicketInfoByS命令返回结果
        /// </summary>
        public string ResultBag { get; set; }

        public TicketInfoByS()
        {          
            FlightList = new List<Flight>();        
        }
    }
}
