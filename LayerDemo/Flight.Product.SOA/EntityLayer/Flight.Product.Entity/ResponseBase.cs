using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Product.Entity
{
    /// <summary>
    /// 响应基类
    /// </summary>
    [DataContract]
    [Serializable]
    public class ResponseBase
    {
        public ResponseBase() { ErrMsg = string.Empty; }
        /// <summary>
        /// 是否成功
        /// </summary>
        [DataMember]
        public bool Success { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        [DataMember]
        public string ErrMsg { get; set; }
    }
}
