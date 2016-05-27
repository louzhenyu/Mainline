using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Provider.Entity
{
    /// <summary>
    /// 分页数据响应类
    /// </summary>
    
    [Serializable]
    public class ResponsePaging
    {
        /// <summary>
        /// 每页记录数
        /// </summary>
        
        public int PageSize { get; set; }
        /// <summary>
        /// 页索引
        /// </summary>
        
        public int PageIndex { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        
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
        }
    }
}
