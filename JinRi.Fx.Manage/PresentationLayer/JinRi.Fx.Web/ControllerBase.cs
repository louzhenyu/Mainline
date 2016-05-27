using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace JinRi.Fx.Web
{
    public class ControllerBase : Controller
    {
        /// <summary>
        /// 上下文对象
        /// </summary>
        private AdminWorkContext m_WorkContext = new AdminWorkContext();

        public AdminWorkContext WorkContext
        {
            get { return m_WorkContext; }
            set { m_WorkContext = value; }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            WorkContext.IsHttpAjax = WebHelper.IsAjax();
            WorkContext.IP = WebHelper.GetIP();
            WorkContext.Url = WebHelper.GetUrl();
            WorkContext.UrlReferrer = WebHelper.GetUrlReferrer();

        }
    }
}
