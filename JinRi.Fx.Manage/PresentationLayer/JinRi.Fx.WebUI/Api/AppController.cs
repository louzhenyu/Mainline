using JinRi.Fx.Entity;
using JinRi.Fx.Logic;
using JinRi.Fx.ResponseDTO;
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
    public class AppController : ApiController
    {
        SysApplicationLogic logic = new SysApplicationLogic();
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            LocalCacheProvider cacheProvider = new LocalCacheProvider();
            string cacheKey = "Fx.Manage.AppController.ApplicationList";
            string result = cacheProvider.GetCache<string>(cacheKey);
            if (string.IsNullOrEmpty(result))
            {
                List<SysApplicationEntity> appList = logic.GetSysApplicationList().ToList<SysApplicationEntity>();
                List<AppReponseDTO> list = new List<AppReponseDTO>();
                foreach (SysApplicationEntity item in appList)
                {
                    list.Add(new AppReponseDTO() { AppId = item.AppId, AppEName = item.AppEName, AppName = item.AppName, AppTypeId = item.AppTypeId, Status = item.Status, SubSystemId = item.SubSystemId });
                }
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                result = serializer.Serialize(list);
                if (!string.IsNullOrEmpty(result))
                {
                    cacheProvider.SetCache<string>(cacheKey, result, DateTime.Now.AddHours(1));
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(result, System.Text.Encoding.UTF8, "application/json") };
        }
    }
}