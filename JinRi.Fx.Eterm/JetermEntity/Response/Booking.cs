using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetermEntity;

namespace JetermEntity.Response
{
    /// <summary>
    /// 订位返回对象
    /// </summary>
    [Serializable]
    public class Booking
    {   
        ///<summary>    
        ///订位编码 
        /// </summary>
        public string Pnr { get; set; }
        /// <summary>
        ///订位Office号
        /// </summary>
        public string OfficeNo { get; set; }
        /// <summary>
        /// 根据编码状态返回占座是否成功
        /// </summary>
        public EtermCommand.BookingState BookingState { get; set; }
        /// <summary>
        /// 大编码号
        /// </summary>
        public string BigPNR { get; set; } 
        /// <summary>
        /// 订单执行的SS指令
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// 订位命令返回结果
        /// </summary>
        public string ResultBag { get; set; }
        /// <summary>
        /// RT命令返回结果
        /// </summary>
        public string RTResultBag { get; set; }
    }
}
