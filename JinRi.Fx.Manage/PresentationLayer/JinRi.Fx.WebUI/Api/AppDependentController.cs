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
using JinRi.Fx.Entity;

namespace JinRi.Fx.WebUI.Apis
{
    public class AppDependentController : ApiController
    {
        SysAppTypeLogic appTypeLogic = new SysAppTypeLogic();
        SysApplicationLogic applicationLogic = new SysApplicationLogic();
        SysAppDependentLogic appDependentLogic = new SysAppDependentLogic();
        // GET api/<controller>/5
        public HttpResponseMessage Get([FromUri]ApplicationRequest request, string callback = "callback")
        {
            try
            {
                int applicationId = request.AppId;
                AppDependentReponseDTO result = new AppDependentReponseDTO();

                result.AppType = appTypeLogic.GetAppTypeList().ToList<SysAppTypeEntity>();
                List<SysAppDependentEntity> dependentList = appDependentLogic.GetAppDependentList(applicationId).ToList<SysAppDependentEntity>();

                //所有应用信息
                List<SysApplicationEntity> applicationList = applicationLogic.GetSysApplicationList().ToList<SysApplicationEntity>();
                Dictionary<int, ApplicationDTO> applicationDictionary = applicationList.ToDictionary(k => k.AppId, v => new ApplicationDTO()
                {
                    AppId = v.AppId,
                    AppType = v.AppTypeId,
                    AppName = v.AppName,
                    AppEName = v.AppEName
                });

                //依赖关系
                foreach (SysAppDependentEntity dependent in dependentList)
                {
                    if (applicationDictionary.ContainsKey(dependent.AppId) && applicationDictionary.ContainsKey(dependent.DependentAppId))
                    {
                        result.AppDependentInfo.Add(new AppDependentDTO() { Source = applicationDictionary[dependent.AppId], Target = applicationDictionary[dependent.DependentAppId] });
                    }
                }
                if (applicationId < 0)
                {
                    //所有应用
                    result.Applications = applicationDictionary.Values.ToList<ApplicationDTO>();
                }
                else
                {
                    //只保留与当前APP有依赖、被依赖关系的应用信息
                    Dictionary<int, ApplicationDTO> appDic = new Dictionary<int, ApplicationDTO>();
                    if (applicationDictionary.ContainsKey(applicationId))
                    {
                        appDic.Add(applicationId, applicationDictionary[applicationId]);
                    }
                    foreach (AppDependentDTO appDependent in result.AppDependentInfo)
                    {
                        if (applicationDictionary.ContainsKey(appDependent.Source.AppId) && !appDic.ContainsKey(appDependent.Source.AppId))
                        {
                            appDic.Add(appDependent.Source.AppId, applicationDictionary[appDependent.Source.AppId]);
                        }
                        if (applicationDictionary.ContainsKey(appDependent.Target.AppId) && !appDic.ContainsKey(appDependent.Target.AppId))
                        {
                            appDic.Add(appDependent.Target.AppId, applicationDictionary[appDependent.Target.AppId]);
                        }
                    }
                    result.Applications = appDic.Values.ToList<ApplicationDTO>();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string content = string.Format("{0}({1})", callback, serializer.Serialize(result));
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(content, System.Text.Encoding.UTF8, "text/javascript") };
            }
            catch (Exception ex)
            {
                //记录异常日志
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
        }
    }

    public class ApplicationRequest
    {
        public int AppId { get; set; }
    }
}