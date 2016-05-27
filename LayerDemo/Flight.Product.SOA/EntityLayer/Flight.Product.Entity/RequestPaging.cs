using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Product.Entity
{
    /// <summary>
    /// 分页数据请求类
    /// </summary>
    [DataContract]
    [Serializable]
    public class RequestPaging
    {
        /// <summary>
        /// 每页记录数
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
        /// <summary>
        /// 页索引
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }

    }
}
