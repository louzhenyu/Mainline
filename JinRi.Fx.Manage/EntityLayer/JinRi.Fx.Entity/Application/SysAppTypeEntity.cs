using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 应用类型实体类
    /// </summary>
    public class SysAppTypeEntity
    {
        /// <summary>
        /// 应用类型编号
        /// </summary>
        public int AppTypeId { get; set; }
        /// <summary>
        /// 应用类型名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string TypeName { get; set; }
    }
}
