using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{
    /// <summary>
    /// 授权返回结果
    /// </summary>
    [Serializable]
    public class Rmk
    {
        /// <summary>
        /// 是否授权成功
        /// </summary>
        public bool IsSuccess { get; set; }       
    }
}
