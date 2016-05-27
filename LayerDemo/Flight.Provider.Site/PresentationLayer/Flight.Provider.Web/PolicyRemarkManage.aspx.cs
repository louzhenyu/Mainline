using Flight.Provider.Business;
using Flight.Provider.Entity.Request;
using Flight.Provider.Entity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Flight.Provider.Web
{
    public partial class PolicyRemarkManage : System.Web.UI.Page
    {
        PolicyRemarkLogic policyRemarkLogic = new PolicyRemarkLogic();
        protected void Page_Load(object sender, EventArgs e)
        {
            PolicyRemarkSearchRequest requestEntity = new PolicyRemarkSearchRequest();
            requestEntity.Paging.PageSize = 10;
            requestEntity.ProviderId = 18132;
            PolicyRemarkSearchResponse responseEntity = policyRemarkLogic.PolicyRemarkSearch(requestEntity);
        }
    }
}