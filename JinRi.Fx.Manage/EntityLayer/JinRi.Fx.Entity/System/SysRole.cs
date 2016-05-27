using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 系统角色实体      RANEN.TONG 2014-10-27
    /// </summary>
    public class SysRole
    {
        /// <summary>
        /// 角色代码
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空")]
        public string RoleName { get; set; }
        /// <summary>
        /// 系统代码
        /// </summary>
        public int SystemId { get; set; }
        /// <summary>
        /// 状态(0: 启用，1：禁用)
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
