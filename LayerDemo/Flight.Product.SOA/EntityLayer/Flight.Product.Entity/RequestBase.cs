using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Product.Entity
{
    /// <summary>
    /// 请求基类
    /// </summary>
    [DataContract]
    [Serializable]
    public class RequestBase
    {
        /// <summary>
        /// 应用编号
        /// </summary>
        [DataMember]
        public int AppID { get; set; }
    }
}
