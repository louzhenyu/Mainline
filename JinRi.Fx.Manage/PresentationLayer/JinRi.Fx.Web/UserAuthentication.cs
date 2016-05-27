using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JinRi.Fx.Web
{
    /// <summary>
    /// 用户登录认证
    /// </summary>
    public class UserAuthentication : AuthorizeAttribute
    {
        public UserToUrlEnum UserToUrlEnum { get; set; }

        public UserAuthentication()
        {
            this.UserToUrlEnum = UserToUrlEnum.Login;
        }
        /// <summary>
        /// 视图响应前执行验证,查看当前用户是否有效
        /// </summary>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session.IsNewSession && string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
            {
                switch (this.UserToUrlEnum)
                {
                    case UserToUrlEnum.Login:
                        //HttpContext.Current.Response.Redirect("~/Account/Login", true);
                        filterContext.Result = new RedirectResult("~/Account/Login");
                        break;
                }
            }
        }
    }
    public enum UserToUrlEnum
    {
        Login,
        About
    }
}