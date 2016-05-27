using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Provider.Entity
{
    /// <summary>
    /// 请求基类
    /// </summary>
    
    [Serializable]
    public class RequestBase
    {
        /// <summary>
        /// 应用编号
        /// </summary>
        
        public int AppID { get; set; }
    }
}
