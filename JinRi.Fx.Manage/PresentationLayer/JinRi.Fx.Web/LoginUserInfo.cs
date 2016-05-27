using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Web
{
    /// <summary>
    /// 登录用户信息
    /// </summary>
    public class LoginUserInfo
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }

        public int RoleId { get; set; }
    }
}
