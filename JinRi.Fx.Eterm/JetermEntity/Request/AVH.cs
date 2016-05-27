using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{
    /// <summary>
    /// 某航司的各直达航线的各舱位剩余可订数请求对象
    /// </summary>
    [Serializable]
    public class AVH
    {
        public AVH()
        {
            DepDate = DateTime.MinValue.Date;
        }

        /// <summary>
        /// 出发城市三字码
        /// </summary>
        public string SCity { get; set; }

        /// <summary>
        /// 到达城市三字码
        /// </summary>
        public string ECity { get; set; }

        /// <summary>
        /// 起飞日期
        /// </summary>
        public DateTime DepDate { get; set; }

        /// <summary>
        /// 航司
        /// </summary>
        public string Airline { get; set; }

        /// <summary>
        /// 航班号
        /// </summary>
        public string FlightNo { get; set; }

        /// <summary>
        /// 舱位
        /// </summary>
        public string Carbin { get; set; }
    }
}
