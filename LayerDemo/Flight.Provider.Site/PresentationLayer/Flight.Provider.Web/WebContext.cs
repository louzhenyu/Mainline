using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flight.Provider.Web
{
    public class WebContext
    {
        private static string staticFileDominName;
        /// <summary>
        /// 静态资源服务器域名，例如：http://res.jinri.cn/
        /// </summary>
        private static string StaticFileDominName
        {
            get
            {
                if (string.IsNullOrEmpty(staticFileDominName))
                {
                    //从Web.Config获取
                    staticFileDominName = System.Configuration.ConfigurationManager.AppSettings["StaticFileDominName"];
                    if (string.IsNullOrEmpty(staticFileDominName))
                    {
                        //获取当前服务器域名
                        staticFileDominName = "http://" + HttpContext.Current.Request.Url.Authority + "/";
                    }
                }
                return staticFileDominName;
            }
        }

        /// <summary>
        /// 静态资源服务地址，不支持相对路径
        /// </summary>
        /// <param name="path">资源路径，不支持相对路径</param>
        /// <returns></returns>
        public static string StaticFileUrl(string path)
        {
            if (string.IsNullOrEmpty(path)) { return string.Empty; }
            return string.Concat(StaticFileDominName, path.TrimStart('~', '/'));
        }
    }
}