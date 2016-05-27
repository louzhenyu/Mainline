using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{    
    /// <summary>
    /// 获取价格请求对象
    /// </summary>
    [Serializable]
    public class GetPrice
    {
        public GetPrice()
        {
        }

        /// <summary>
        /// 编码乘客类型
        /// </summary>
        public EtermCommand.PassengerType PassengerType { get; set; }
        /// <summary>
        /// Flight Type（O表示单程，F表示往返，T表示联程；默认值为O）
        /// </summary>
        public EtermCommand.FlightType FlightType { get; set; }
        /// <summary>
        /// 航线信息
        /// </summary>
        public List<Flight> FlightList { get; set; }

    }
}
