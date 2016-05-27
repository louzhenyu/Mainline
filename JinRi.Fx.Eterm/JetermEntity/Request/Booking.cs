
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Request
{
    /// <summary>
    /// 订位请求对象
    /// </summary>
    [Serializable]
    public class Booking
    {        
        /// <summary>
        /// 航线信息列表（第1项表示起飞信息，若有第2项，则表示返回信息）
        /// </summary>
        public List<Flight> FlightList { get; set; }
        /// <summary>
        /// 乘机人信息列表
        /// </summary>
        public List<Passenger> PassengerList { get; set; }
        /// <summary>
        /// 保留时限项的Office号
        /// </summary>
        public string OfficeNo { get; set; }        
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 座机号
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// 授权Office号列表
        /// </summary>
        public List<string> RMKOfficeNoList { get; set; }
        /// <summary>
        /// 授权备注
        /// </summary>
        public string RMKRemark { get; set; }

        /// <summary>
        /// 成人PNR，订儿童票时使用
        /// </summary>
        public string Pnr { get; set; }

        public Booking()
        {
            FlightList = new List<Flight>();
            PassengerList = new List<Passenger>();
            RMKOfficeNoList = new List<string>();
        }
    }
}
