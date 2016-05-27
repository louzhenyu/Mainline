using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FX.CTI.DBEntity
{
    /// <summary>
    /// 邮件数据实体
    /// </summary>
    public class Email
    {
        /// <summary>
        /// 编号(主键)
        /// </summary>
        public string EmailId { get; set; }
        /// <summary>
        /// 调用者编号
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        public string EmailToAddr { get; set; }
        /// <summary>
        /// 抄送人
        /// </summary>
        public string EmailCC { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string EmailSubject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string EmailContent { get; set; }
        /// <summary>
        /// 处理状态
        /// </summary>
        public string EmailStatus { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string EmailErrMsg { get; set; }
        /// <summary>
        /// 接受时间
        /// </summary>
        public DateTime EmailReceivedTime { get; set; }
        /// <summary>
        /// 发出时间
        /// </summary>
        public DateTime? EmailSentTime { get; set; }
    }
    /// <summary>
    /// 邮件处理状态
    /// </summary>
    public enum EmailStatus
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
