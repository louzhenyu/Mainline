using JinRi.Fx.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinRi.Fx.Entity;
using JinRi.Fx.Logic;
using JinRi.Fx.WebUI.Models;
using JinRi.Fx.Utility;
using JinRi.Fx.ResponseDTO;
using System.Web.Script.Serialization;

namespace JinRi.Fx.WebUI.Controllers.Dashboard
{
    public class DashboardController : ControllerBaseAdmin
    {
        DashboardLogic logic = new DashboardLogic();

        public ActionResult Index(MetricSearchArgs model)
        {
            return View(model);
        }

        public JsonResult Search(MetricSearchArgs searchCondition)
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

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Api(MetricSearchArgs searchCondition)
        {
            return Search(searchCondition);
        }

        public ContentResult MetricsKey()
        {
            LocalCacheProvider cacheProvider = new LocalCacheProvider();
            string cacheKey = "Fx.Manage.Dashboard.MetricsKey";
            string result = cacheProvider.GetCache<string>(cacheKey);
            if (string.IsNullOrEmpty(result))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                List<MetricsKey> list = logic.GetMetricsKeys();
                List<string> keys = new List<string>();
                foreach (MetricsKey key in list)
                {
                    keys.Add(key.Key);
                }
                result = serializer.Serialize(keys);
                if (!string.IsNullOrEmpty(result))
                {
                    cacheProvider.SetCache<string>(cacheKey, result, DateTime.Now.AddDays(1));
                }
            }
            return Content(result);
        }
    }
}