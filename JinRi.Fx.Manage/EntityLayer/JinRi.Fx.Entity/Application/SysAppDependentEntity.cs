using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 应用依赖关系实体类
    /// </summary>
    public class SysAppDependentEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 应用编号
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// 依赖的应用编号
        /// </summary>
        public int DependentAppId { get; set; }
    }
}
