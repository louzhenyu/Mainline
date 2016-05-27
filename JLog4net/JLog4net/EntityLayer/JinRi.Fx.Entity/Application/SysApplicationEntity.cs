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
    public class SysApplicationEntity
    {
        /// <summary>
        /// 应用编号
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// 所属子系统编号
        /// </summary>
        public int SubSystemId { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        public string AppName { get; set; }
        /// <summary>
        /// 应用英文名称
        /// </summary>
        [Required(ErrorMessage = "英文名称不能为空")]
        public string AppEName { get; set; }
        /// <summary>
        /// 应用类型
        /// </summary>
        public int AppTypeId { get; set; }
        ///// <summary>
        ///// 负责人
        ///// </summary>
        //public string Owner { get; set; }
        /// <summary>
        /// 状态：0:启用,1:禁用
        /// </summary>
        public int Status { get; set; }
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string Remark { get; set; }
        ///// <summary>
        ///// 添加时间
        ///// </summary>
        //public string AddTime { get; set; }

        ///// <summary>
        ///// 是否依赖
        ///// </summary>
        //public int IsDependent { get; set; }
    }
}
