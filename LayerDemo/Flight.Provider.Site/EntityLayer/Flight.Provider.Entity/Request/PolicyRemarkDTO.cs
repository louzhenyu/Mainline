using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Flight.Provider.Entity.Request
{
    [Serializable]
    public class PolicyRemarkDTO
    {
        /// <summary>
        /// 政策备注编号
        /// </summary>
        public int PolicyRemarkId { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public int ProviderId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 0:普通政策（正常）2:普通政策（特殊）5:联程政策 6:包机切位政策 7:特惠政策 12:直投政策 13:特价政策 14:团队往返
        /// </summary>
        public int PolicyType { get; set; }
    }
}
