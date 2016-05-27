using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 系统接口实体类
    /// </summary>
    public class SysApiEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int SysApiId { get; set; }
        /// <summary>
        /// 应用编号
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string ApiName { get; set; }
        /// <summary>
        /// 接口类型(0:内部服务，1:OpenApi)
        /// </summary>
        public int ApiType { get; set; }
        /// <summary>
        /// 功能描述
        /// </summary>
        public string ApiDescription { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string ApiOwner { get; set; }
        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiAddress { get; set; }
        /// <summary>
        /// 状态(0：启用，1：禁用)
        /// </summary>
        public int ApiStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
