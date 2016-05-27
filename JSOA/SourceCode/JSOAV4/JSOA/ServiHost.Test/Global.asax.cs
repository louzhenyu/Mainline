using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using ServiceStack;

namespace ServiHost.Test
{
    public class Global : System.Web.HttpApplication
    {
        public class ProdsssServiceHost : AppHostBase
        {
            // Register your Web service with ServiceStack.
            public ProdsssServiceHost()
                : base("订单管理test3", typeof(ProdsssServiceHost).Assembly)
            {

            }

            public override void Configure(Funq.Container container)
            {
                // Register any dependencies your services use here.
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // Initialize your Web service on startup.
            new ProdsssServiceHost().Init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}