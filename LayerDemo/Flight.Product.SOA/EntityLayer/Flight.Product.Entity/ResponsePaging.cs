using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Product.Entity
{
    /// <summary>
    /// 分页数据响应类
    /// </summary>
    [DataContract]
    [Serializable]
    public class ResponsePaging
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
        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        [DataMember]
        public int PageCount
        {
            get
            {
                if (PageSize > 0 && TotalCount > 0)
                {
                    return TotalCount % PageSize == 0 ? (TotalCount / PageSize) : (TotalCount / PageSize + 1);
                }
                return 0;
            }
            set { PageCount = value; }
        }
    }
}
