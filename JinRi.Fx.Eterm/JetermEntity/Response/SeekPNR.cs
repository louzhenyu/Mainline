using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{
    /// <summary>
    /// 提取PNR返回对象
    /// </summary>
    [Serializable]
    public class SeekPNR
    {
        /// <summary>
        /// 乘机人
        /// </summary>
        public List<Passenger> PassengerList { get; set; }
        /// <summary>
        /// PNR记录编码
        /// </summary>
        public string PNR { get; set; }    
        /// <summary>
        /// 航班信息
        /// </summary>
        public List<Flight> FlightList { get; set; }
        /// <summary>
        /// 共享航班（true表示共享航班，false表示非共享航班；默认值为false）
        /// </summary>
        public bool ShareFlight { get; set; }
        /// <summary>
        /// Flight Type（O表示单程，F表示往返，T表示联程；默认值为O）
        /// </summary>
        public EtermCommand.FlightType FlightType { get; set; }
        /// <summary>
        /// 授权的OFFICE号
        /// </summary>
        public List<string> RMKOfficeNoList { get; set; }
        /// <summary>
        /// 大编码
        /// </summary>
        public string BigPNR { get; set; }
        /// <summary>
        /// 联系电话，CTCT项
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 座机号
        /// </summary>
        public string PhoneNo { get; set; }
        /// <summary>
        /// OfficeNo
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 儿童监管人的PNR
        /// </summary>
        public string AdultPnr { get; set; }
        /// <summary>
        /// 基本运价
        /// </summary>
        public List<Price> PriceList { get; set; } 
        /// <summary>
        /// RT、PAT命令返回结果
        /// </summary>
        public string ResultBag { get; set; }

        public SeekPNR()
        {
            PassengerList = new List<Passenger>();
            FlightList = new List<Flight>();            
            RMKOfficeNoList = new List<string>();
            PriceList = new List<Price>();
            ShareFlight = false;
            FlightType = EtermCommand.FlightType.O;
        }
    }
}
