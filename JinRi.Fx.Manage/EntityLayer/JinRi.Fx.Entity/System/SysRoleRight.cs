using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    /// <summary>
    /// 角色权限实体      RANEN.TONG 2014-10-27
    /// </summary>
    public class SysRoleRight
    {
        public int ID { get; set; }
        /// <summary>
        /// 角色代码
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        ///  菜单代码
        /// </summary>
        public int MenuId { get; set; }
        /// <summary>
        /// 应用程序ID
        /// </summary>
        public int AppId { get; set; }  
        /// <summary>
        /// 应用程序ID（string类型）
        /// </summary>
        public string AppIdString
        {
            get
            {
                return AppId.ToString();
            }
        }
    }
}
