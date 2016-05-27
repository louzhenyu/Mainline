using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 模块实体        RANEN.TONG 2014-10-24
    /// </summary>
    public class SysModule
    {
        /// <summary>
        /// 模块代码
        /// </summary>
        public int ModuleId { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        [Required(ErrorMessage = "模块名称不能为空")]
        public string ModuleName { get; set; }
        /// <summary>
        /// 所属系统编号
        /// </summary>
        public int SystemId { get; set; }
        /// <summary>
        /// 模块地址
        /// </summary>
        public string ModuleUrl { get; set; }
        /// <summary>
        /// 图标地址
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 状态(0：启用，1：停用)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
