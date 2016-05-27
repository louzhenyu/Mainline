using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity
{
    /// <summary>
    /// 基础运价
    /// </summary>
    [Serializable]
    public class Price
    {
        /// <summary>
        /// 票面价
        /// </summary>
        public decimal FacePrice { get; set; }       
        /// <summary>
        /// 机建费
        /// </summary>
        public decimal Tax { get; set; }
        /// <summary>
        /// 燃油税
        /// </summary>
        public decimal Fuel { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 价格标识
        /// </summary>
        public string Tag { get; set; }
    }
}
