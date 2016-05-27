using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FX.CTI.DBEntity
{
    /// <summary>
    /// 短信数据实体
    /// </summary>
    public class SMS
    {
        /// <summary>
        /// 编号(主键)
        /// </summary>
        public string SMSId { get; set; }
        /// <summary>
        /// 调用者编号
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string SMSMobile { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string SMSContent { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string SMSErrMsg { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        public string SMSStatus { get; set; }
        /// <summary>
        /// 接受时间
        /// </summary>
        public DateTime SMSReceivedTime { get; set; }
        /// <summary>
        /// 发出时间
        /// </summary>
        public DateTime? SMSSentTime { get; set; }
    }

    /// <summary>
    /// 短信处理状态
    /// </summary>
    public enum SMSStatus
    {
        /// <summary>
        /// 已接受
        /// </summary>
        Received,
        /// <summary>
        /// 已发出
        /// </summary>
        Sent,
        /// <summary>
        /// 失败
        /// </summary>
        Fail
    }
}
