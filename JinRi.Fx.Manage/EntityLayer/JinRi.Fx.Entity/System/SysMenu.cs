using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 系统菜单实体  RANEN.TONG 2014-10-24
    /// </summary>
    public class SysMenu
    {
        /// <summary>
        /// 菜单编号
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required(ErrorMessage = "菜单名称不能为空")]
        public string MenuName { get; set; }
        /// <summary>
        /// 菜单地址
        /// </summary>
        public string MenuUrl { get; set; }
        /// <summary>
        /// 图标地址
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 所属模块
        /// </summary>
        public int ModuleId { get; set; }
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
