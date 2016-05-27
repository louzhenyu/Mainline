using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{
    /// <summary>
    /// 插编码返回对象
    /// </summary>
    [Serializable]
    public class CancelPnr
    {
        /// <summary>
        /// 是否擦成功
        /// </summary>
        public bool IsSuccess { get; set; }     
    }
}
