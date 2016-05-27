using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JinRi.Fx.Web
{
    /// <summary>
    /// 需要登录的Controller基类
    /// </summary>
    public class ControllerBaseAdmin : ControllerBase
    {
        public LoginUserInfo GetLoginUser()
        {
            if (Request != null && Request.IsAuthenticated)
            {
                //获取授权cookie
                HttpCookie authCookie = Request.Cookies[System.Web.Security.FormsAuthentication.FormsCookieName];
                //根据Cookie得到登录用户票据
                FormsAuthenticationTicket ticke = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticke != null || string.IsNullOrEmpty(ticke.UserData))
                {
                    LoginUserInfo UserInfo = JsonHelper.DataContractJsonDeserialize<LoginUserInfo>(ticke.UserData);
                    return UserInfo;
                }
            }
            return new LoginUserInfo();
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            WorkContext.CurrentUser = GetLoginUser();
        }
        protected override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.UserInfo = WorkContext.CurrentUser;
        }
        /// <summary>
        /// 当弹出DIV弹窗时，需要刷新浏览器整个页面
        /// </summary>
        /// <returns></returns>
        public ContentResult RefreshParent(string alert = null)
        {
            var script = string.Format("<script>{0}; parent.location.reload(1)</script>", string.IsNullOrEmpty(alert) ? string.Empty : "alert('" + alert + "')");
            return this.Content(script);
        }

        public new ContentResult RefreshParentTab(string alert = null)
        {
            var script = string.Format("<script>{0}; if (window.opener != null) {{ window.opener.location.reload(); window.opener = null;window.open('', '_self', '');  window.close()}} else {{parent.location.reload(1)}}</script>", string.IsNullOrEmpty(alert) ? string.Empty : "alert('" + alert + "')");
            return this.Content(script);
        }

        /// <summary>
        /// 用JS关闭弹窗
        /// </summary>
        /// <returns></returns>
        public ContentResult CloseThickbox()
        {
            return this.Content("<script>top.tb_remove()</script>");
        }

        /// <summary>
        ///  警告并且历史返回
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ContentResult Back(string notice)
        {
            var content = new StringBuilder("<script>");
            if (!string.IsNullOrEmpty(notice))
                content.AppendFormat("alert('{0}');", notice);
            content.Append("history.go(-1)</script>");
            return this.Content(content.ToString());
        }


        public ContentResult PageReturn(string msg, string url = null)
        {
            var content = new StringBuilder("<script type='text/javascript'>");
            if (!string.IsNullOrEmpty(msg))
                content.AppendFormat("alert('{0}');", msg);
            if (string.IsNullOrWhiteSpace(url))
                url = Request.Url.ToString();
            content.Append("window.location.href='" + url + "'</script>");
            return this.Content(content.ToString());
        }

        /// <summary>
        /// 转向到一个提示页面，然后自动返回指定的页面
        /// </summary>
        /// <param name="notice"></param>
        /// <param name="redirect"></param>
        /// <returns></returns>
        public ContentResult Stop(string notice, string redirect, bool isAlert = false)
        {
            var content = "<meta http-equiv='refresh' content='1;url=" + redirect + "' /><body style='margin-top:0px;color:red;font-size:24px;'>" + notice + "</body>";

            if (isAlert)
                content = string.Format("<script>alert('{0}'); window.location.href='{1}'</script>", notice, redirect);

            return this.Content(content);
        }
    }
}
