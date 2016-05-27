using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flight.Product.DBEntity
{
    /// <summary>
    /// 政策备注数据实体
    /// </summary>
    public class PolicyRemark
    {
        public PolicyRemark()
        {
            ExtendInfo = string.Empty;
            TempletName = string.Empty;
        }
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public int AgentID { get; set; }
        /// <summary>
        /// 政策备注
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 审核用户
        /// </summary>
        public string OperUser { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime OperTime { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 政策类型
        /// </summary>
        public int RateType { get; set; }
        /// <summary>
        /// 扩展信息
        /// </summary>
        public string ExtendInfo { get; set; }
        /// <summary>
        /// 模版名称
        /// </summary>
        public string TempletName { get; set; }
    }
}
