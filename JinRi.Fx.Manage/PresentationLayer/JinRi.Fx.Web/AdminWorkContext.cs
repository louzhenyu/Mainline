using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinRi.Fx.Web
{
    /// <summary>
    /// 后台工作上下文
    /// </summary>
    public class AdminWorkContext
    {
        /// <summary>
        /// 当前请求是否为ajax请求
        /// </summary>
        public bool IsHttpAjax { get; set; }

        /// <summary>
        /// 用户ip
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 当前url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 上一次访问的url
        /// </summary>
        public string UrlReferrer { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 动作方法
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 页面标示符(Controller_Action)
        /// </summary>
        public string PageKey { get; set; }

        /// <summary>
        /// 当前登录的用户信息
        /// </summary>
        public LoginUserInfo CurrentUser { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsAdmin
        {
            get { return ConfigManage.AdministratorRoles.Contains(CurrentUser.RoleId.ToString()); }
        }
    }
}