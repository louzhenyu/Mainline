using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FX.CTI.Entity.DTO
{
    /// <summary>
    /// SMS短信请求
    /// </summary>
    [DataContract]
    public class SMSRequest
    {
        /// <summary>
        /// 调用者编号
        /// 可否为null:否
        /// </summary>
        [DataMember]
        public string AppId { get; set; }

        /// <summary>
        /// 手机号
        /// 可否为null:否
        /// </summary>
        [DataMember]
        public string Mobile { get; set; }

        /// <summary>
        /// 短信内容
        /// 可否为null:否
        /// </summary>
        [DataMember]
        public string Content { get; set; }
    }
}
