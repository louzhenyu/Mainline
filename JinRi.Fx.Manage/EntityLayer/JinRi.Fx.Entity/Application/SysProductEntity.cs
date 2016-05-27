using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 产品线实体类
    /// </summary>
    public class SysProductEntity
    {
        /// <summary>
        /// 产品线Id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// 产品线名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string ProductName { get; set; }
        [Required(ErrorMessage = "英文名称不能为空")]
        public string ProductEName { get; set; }
        public string Remark { get; set; }
    }
}
