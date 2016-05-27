using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FX.CTI.Entity.DTO
{
    /// <summary>
    /// Email请求
    /// </summary>
    [DataContract]
    public class EmailRequest
    {
        /// <summary>
        /// 调用者编号
        /// 可否为null:否
        /// </summary>
        [DataMember]
        public string AppId { get; set; }

        /// <summary>
        /// 收件人Email地址
        /// 可否为null:否
        /// </summary>
        [DataMember]
        public string ToAddr { get; set; }

        /// <summary>
        /// 抄送地址
        /// 可否为null:是
        /// 多个地址用","分隔
        /// </summary>
        [DataMember]
        public string CC { get; set; }

        /// <summary>
        /// 标题
        /// 可否为null:否
        /// </summary>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// 可否为null:否
        /// </summary>
        [DataMember]
        public string Content { get; set; }
    }
}
