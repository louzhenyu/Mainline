using Flight.Provider.Utility;
using JFx;
using log4net;
using Metrics;
using System;
using System.Text;
using System.Web;

namespace Flight.Provider.Web
{
    public class Global : System.Web.HttpApplication
    {
        ILog log = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //if (SqlInjectHandler.SqlInjectFilter(HttpContext.Current))
            //{
            //    StringBuilder errorMsg = new StringBuilder();
            //    errorMsg.AppendLine(string.Format("检测到危险字符：URL:{0}", HttpContext.Current.Request.Url.ToString()));
            //    errorMsg.AppendLine(string.Format("GET:{0}", HttpContext.Current.Request.QueryString.ToString()));
            //    errorMsg.AppendLine(string.Format("POST:{0}", HttpContext.Current.Request.Form.ToString()));
            //    errorMsg.AppendLine(string.Format("Server IP:{0},Client IP:{1}", AppEnvironment.LocalIPAddress, JFx.Utils.Utility.GetClientIp()));
            //    //加入用户相关信息
            //    log.Info(errorMsg.ToString());

            //    //记录信息
            //    HttpContext.Current.Response.Write("<script>alert('检测到包含非法字符！');history.go(-1);</script>");
            //    HttpContext.Current.Response.End();
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //记录异常日志并抛出异常
            Exception ex = Server.GetLastError().GetBaseException();
            if (ex != null)
            {
                StringBuilder errorMsg = new StringBuilder();
                errorMsg.AppendLine(string.Format("URL:{0}", Request.Url.ToString()));
                errorMsg.AppendLine(string.Format("TargetSite:{0}", ex.TargetSite));
                errorMsg.AppendLine(string.Format("Message:{0}", ex.Message));
                errorMsg.AppendLine(string.Format("StackTrace:{0}", ex.StackTrace));
                errorMsg.AppendLine(string.Format("Server IP:{0},Client IP:{1}", AppEnvironment.LocalIPAddress, JFx.Utils.Utility.GetClientIp()));

                //加入用户相关信息
                log.Error(errorMsg.ToString());
            }
            //加入Metrics
            MetricsManager.MeterMark("Flight.Provider.ExceptionCount", Unit.Custom("个"));
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}