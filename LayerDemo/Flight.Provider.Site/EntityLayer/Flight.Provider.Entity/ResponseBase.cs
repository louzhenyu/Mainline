using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Provider.Entity
{
    /// <summary>
    /// 响应基类
    /// </summary>
    
    [Serializable]
    public class ResponseBase
    {
        public ResponseBase() { ErrMsg = string.Empty; }
        /// <summary>
        /// 是否成功
        /// </summary>
        
        public bool Success { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        
        public string ErrMsg { get; set; }
    }
}
