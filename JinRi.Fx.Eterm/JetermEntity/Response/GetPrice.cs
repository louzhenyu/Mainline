using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JetermEntity.Response
{    
    /// <summary>
    /// 获取价格返回对象
    /// </summary>
    [Serializable]
    public class GetPrice
    {
        public GetPrice()
        {
        }

        /// <summary>
        /// 基本运价
        /// </summary>
        public List<Price> PriceList { get; set; }

    }
}
