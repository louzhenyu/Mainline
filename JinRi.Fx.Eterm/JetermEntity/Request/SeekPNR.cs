using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{    
    /// <summary>
    /// 提取PNR请求对象
    /// </summary>
    [Serializable]
    public class SeekPNR
    {
        public SeekPNR()
        {
        }
        /// <summary>
        /// 记录编码
        /// </summary>
        public string Pnr { get; set; }

        /// <summary>
        /// 编码乘客类型
        /// </summary>
        public EtermCommand.PassengerType PassengerType { get; set; }

        /// <summary>
        /// 是否获取价格
        /// </summary>
        public bool GetPrice { get; set; }

        /// <summary>
        /// 航司
        /// </summary>
        public string Airline { get; set; }
    }
}
