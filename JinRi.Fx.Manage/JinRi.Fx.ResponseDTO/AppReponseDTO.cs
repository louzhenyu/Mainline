using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.ResponseDTO
{
    public class AppReponseDTO
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
        public string AppName { get; set; }
        /// <summary>
        /// 应用英文名称
        /// </summary>
        public string AppEName { get; set; }
        /// <summary>
        /// 应用类型
        /// </summary>
        public int AppTypeId { get; set; }
        /// <summary>
        /// 状态：0:启用,1:禁用
        /// </summary>
        public int Status { get; set; }
    }
}
