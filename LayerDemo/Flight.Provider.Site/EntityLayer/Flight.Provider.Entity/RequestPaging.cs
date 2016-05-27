using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Provider.Entity
{
    /// <summary>
    /// 分页数据请求类
    /// </summary>
    
    [Serializable]
    public class RequestPaging
    {
        /// <summary>
        /// 每页记录数
        /// </summary>
        
        public int PageSize { get; set; }
        /// <summary>
        /// 页索引
        /// </summary>
        
        public int PageIndex { get; set; }

    }
}
