using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity
{
    /// <summary>
    /// 航班信息
    /// </summary>
    [Serializable]
    public class Flight
    {
        public Flight()
        {
            DepDate = DateTime.MinValue;
            ArrDate = DateTime.MinValue;
        }

        private string flightNo = string.Empty;
        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo
        {
            get { return flightNo; }
            set { flightNo = value; }
        }

        private string airLine = string.Empty;
        /// <summary>
        /// 航司
        /// </summary>
        public string Airline
        {
            get { return airLine; }
            set { airLine = value; }
        }

        private string cabin = string.Empty;
        /// <summary>
        /// 舱位
        /// </summary>
        public string Cabin
        {
            get { return cabin; }
            set { cabin = value; }
        }

        private string subCabin = string.Empty;
        /// <summary>
        /// 子舱位
        /// </summary>
        public string SubCabin
        {
            get { return subCabin; }
            set { subCabin = value; }
        }
       
        private string sCity = null;
        /// <summary>
        /// 出发城市三字码
        /// </summary>
        public string SCity
        {
            get { return sCity; }
            set { sCity = value; }
        }

        private string eCity = null;
        /// <summary>
        /// 到达城市三字码
        /// </summary>
        public string ECity
        {
            get { return eCity; }
            set { eCity = value; }
        }

        /// <summary>
        /// 出发航站楼
        /// </summary>
        public string DepTerminal { get; set; }
        /// <summary>
        /// 到达航站楼
        /// </summary>
        public string ArrTerminal { get; set; }

        /// <summary>
        /// 出发时间
        /// </summary>
        public DateTime DepDate { get; set; }
        /// <summary>
        /// 到达时间
        /// </summary>
        public DateTime ArrDate { get; set; }
        /// <summary>
        /// 各航段的PNR当前状态
        /// </summary>
        public string PNRState { get; set; }

        /// <summary>
        /// 起飞日期（string类型）
        /// </summary>
        public string DepDateString { get; set; }

        /// <summary>
        /// 各航段的客票状态（存的是枚举值）
        /// </summary>
        public EtermCommand.TicketStatus TicketStatus { get; set; }   
    }

    
}
