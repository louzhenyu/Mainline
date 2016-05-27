using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{

    /// <summary>
    /// 获取票号 DETR:TN,H 返回对象
    /// </summary>
    [Serializable]
    public class TicketInfoHis
    {  /// <summary>
        /// 票号/编码
        /// </summary>
        public string TicketNo { get; set; }

        /// <summary>
        /// 乘客姓名
        /// </summary>
        public string PassengerName { get; set; }

        /// <summary>
        /// 取位时间
        /// </summary>
        public string CancelTime { get; set; }

        /// <summary>
        /// 是否打印行程单
        /// </summary>
        public bool IsSchedule { get; set; }
    }
}
