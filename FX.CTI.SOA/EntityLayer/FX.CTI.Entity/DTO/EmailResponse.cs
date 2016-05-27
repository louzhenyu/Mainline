using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FX.CTI.Entity.DTO
{
    /// <summary>
    /// Email响应
    /// </summary>
    [DataContract]
    public class EmailResponse
    {
        /// <summary>
        /// 是否成功
        /// true表示处理成功，已接受发Email请求，但不代表立刻发邮件或发邮件成功
        /// false表示处理失败，已拒绝发Email请求
        /// 可否为null:否
        /// </summary>
        [DataMember]
        public bool Success;
        /// <summary>
        /// 错误信息
        /// 可否为null:处理失败时可以，处理成功时不可以
        /// </summary>
        [DataMember]
        public string ErrMsg;
        /// <summary>
        /// 邮件编号
        /// 可否为null:处理失败时可以,处理成功时不可以
        /// </summary>
        [DataMember]
        public string EmailId;
    }
}
