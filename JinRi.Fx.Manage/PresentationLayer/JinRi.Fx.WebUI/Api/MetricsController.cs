using JinRi.Fx.Logic;
using JinRi.Fx.ResponseDTO;
using JinRi.Fx.Utility;
using JinRi.Fx.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web;
using System.Web.Script.Serialization;

namespace JinRi.Fx.WebUI.Apis
{
    public class MetricsController : ApiController
    {
        DashboardLogic logic = new DashboardLogic();
        // GET api/<controller>/5
        public HttpResponseMessage Get([FromUri]MetricSearchArgs searchCondition, string callback = "callback")
        {
            MetricSearchResponseDTO result = GetResponseResult(searchCondition);
            if (result != null)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string content = string.Format("{0}({1})", callback, serializer.Serialize(result));
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content, System.Text.Encoding.UTF8, "text/javascript") };
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }

        // POST api/<controller>
        public MetricSearchResponseDTO Post([FromBody]MetricSearchArgs searchCondition)
        {
            return GetResponseResult(searchCondition);
        }

        MetricSearchResponseDTO GetResponseResult(MetricSearchArgs searchCondition)
        {
            MetricSearchResponseDTO result = null;
            switch (searchCondition.MetricType)
            {
                case MetricType.Meters:
                    result = logic.GetMeterList(searchCondition.ToDTO());
                    break;
                case MetricType.Histograms:
                    result = logic.GetHistogramList(searchCondition.ToDTO());
                    break;
            }
            return result;
        }
    }
}