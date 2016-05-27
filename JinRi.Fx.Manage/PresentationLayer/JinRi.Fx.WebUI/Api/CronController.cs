using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace JinRi.Fx.WebUI.Api
{
    public class CronController : ApiController
    {
        public HttpResponseMessage Get([FromUri]CronRequest request, string callback = "callback")
        {
            CronResponse cronResponse = new CronResponse();
            cronResponse.CronExpression = request.CronExpression;
            cronResponse.FireTimes = CommonHelper.GetCronFireTime(request.CronExpression, 10);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string content = string.Format("{0}({1})", callback, serializer.Serialize(cronResponse));
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content, System.Text.Encoding.UTF8, "text/javascript") };
        }
    }
    public class CronRequest
    {
        public string CronExpression { get; set; }
    }
    public class CronResponse
    {
        public CronResponse()
        {
            this.FireTimes = new List<string>();
        }
        public string CronExpression { get; set; }
        public List<string> FireTimes { get; set; }
    }
}
