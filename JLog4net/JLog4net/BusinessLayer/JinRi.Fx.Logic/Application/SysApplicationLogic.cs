using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JinRi.Fx.Logic
{
    /// <summary>
    /// 应用管理
    /// </summary>
    public class SysApplicationLogic
    {
        SysApplicationDal sysApplicationDal = new SysApplicationDal();
        ILog log = LogManager.GetLogger(typeof(SysApplicationLogic));
        /// <summary>
        /// 获取单个应用信息
        /// </summary>
        /// <param name="appId">应用编号</param>
        /// <returns></returns>
        public SysApplicationEntity GetSysApplicationInfo(int appId)
        {
            return sysApplicationDal.GetSysApplicationInfo(appId);
        }
        /// <summary>
        /// 获取APPID OpenAPI
        /// </summary>
        /// <param name="searchRQ">请求实体</param>
        /// <returns></returns>
        public List<SysApplicationEntity> GetSysApplicationList(string strUrl)
        {
            #region 变量声明

            string strError = string.Empty;
            bool isError = false;
            string strResponse = null;
            string strLog = string.Empty;


            List<SysApplicationEntity> response = null;

            #endregion
            #region 航班查询
            strResponse = JFx.Utils.HttpHelper.SendGet(strUrl);
            #endregion

            #region 处理返回数据

            try
            {
                if (string.IsNullOrEmpty(strResponse))
                {
                    strError = "没有Appid应用数据";
                }


                //反序列化查询结果

                response = JsonConvert.DeserializeObject<List<SysApplicationEntity>>(strResponse);

            }
            catch (Exception ex)
            {
                isError = true;
                strLog += "异常错误:" + ex.ToString() + System.Environment.NewLine;
               
            }
            finally
            {
                strLog += "解析返回:" + JsonConvert.SerializeObject(response) + System.Environment.NewLine;
                if (isError)
                    log.Error(strLog);
                else
                    log.Info(strLog);
            }

            #endregion

            return response;
        }
        /// <summary>
        /// Appid智能感应
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public string[] GetAppidList(string appId,string requestUrl)
        {
            List<string> returnData = new List<string>();
            //IEnumerable<SysApplicationEntity> GetSysApplicationList = sysApplicationDal.GetSysApplicationList(appId, null);
            List<SysApplicationEntity> GetSysAppidList = GetSysApplicationList(requestUrl);
            foreach (SysApplicationEntity ent in GetSysAppidList)
            {
                if (ent.AppName.Contains(appId)||ent.AppId.ToString().Contains(appId)||ent.AppEName.ToUpper().Contains(appId.ToUpper()))
                {  
                    returnData.Add(ent.AppId.ToString()+"  "+ent.AppEName);
                }
            }
            return returnData.ToArray();
        }
    }
}
