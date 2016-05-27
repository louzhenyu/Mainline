using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JinRi.PolicyJob.WebSite
{
    public partial class PolicySyncPage : System.Web.UI.Page
    {

        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(PolicySyncPage));
        protected void Page_Load(object sender, EventArgs e)
        {
            logger.Info("Page_Load");
            JinRi.Policy.PolicySync policySync = new Policy.PolicySync();
            policySync.PolicySyncProcess();
            Response.Write("执行完毕."); 
            Response.End();
        }
    }
}