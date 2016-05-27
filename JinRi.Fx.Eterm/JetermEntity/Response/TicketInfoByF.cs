using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{
    /// <summary>
    /// 获取票号 DETR:TN,F 返回对象
    /// </summary>
    [Serializable]
    public class TicketInfoByF
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
        /// 身份证号
        /// </summary>
        public string PassengerCardNo { get; set; }

        /// <summary>
        /// 是否打印行程单
        /// </summary>
        public bool IsSchedule { get; set; }
    }
}
