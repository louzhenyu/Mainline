using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 子系统实体类
    /// </summary>
    public class SysSubEntity
    {
        /// <summary>
        /// 子系统Id
        /// </summary>
        public int SubSystemId { get; set; }
        /// <summary>
        /// 所属产品线Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 子系统中文名称
        /// </summary>
        [Required(ErrorMessage = "中文名称不能为空")]
        public string SystemName { get; set; }

        /// <summary>
        /// 子系统英文名称
        /// </summary>
        [Required(ErrorMessage = "英文名称不能为空")]
        public string SystemEName { get; set; }

        public string Remark { get; set; }
    }
}
