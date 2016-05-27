using MetricsServer.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;

namespace MetricsServer
{
    /// <summary>
    /// Metrics 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Metrics : System.Web.Services.WebService
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Metrics));
        [WebMethod]
        public void Save(string metricsValue)
        {
            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "text/plain";
                if (!string.IsNullOrEmpty(metricsValue))
                {
                    JObject json = JObject.Parse(metricsValue);
                    if (json != null)
                    {
                        int appId = 0;
                        JToken environmentToken = json["Environment"];
                        if (environmentToken == null) { return; }
                        string context = json["Context"] == null ? "" : json["Context"].ToString();
                        //string ipAddress = Context.Request.UserHostAddress;
                        string ipAddress = environmentToken["IPAddress"] == null ? "" : environmentToken["IPAddress"].ToString();
                        int.TryParse(environmentToken["AppId"] == null ? "" : environmentToken["AppId"].ToString(), out appId);

                        SaveMeters(ipAddress, appId, json);

                        SaveHistograms(ipAddress, appId, json);

                        Context.Response.Write("OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Context.Response.Write("Exception");
                logger.Info(string.Format("异常信息：{0}{1}MetricsValues：{2}", ex.ToString(), System.Environment.NewLine, Regex.Replace(metricsValue, @"\s+", " ")));
#if DEBUG
                Console.WriteLine("Metrics.asmx.cs - Save(): 现在时间为 - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "；异常信息为：" + string.Format("异常信息：{0}{1}MetricsValues：{2}", ex.ToString(), System.Environment.NewLine, Regex.Replace(metricsValue, @"\s+", " ")));
#endif
            }
            Context.Response.End();
        }

        #region 保存Histogram度量数据
        /// <summary>
        /// 保存Histogram度量数据
        /// </summary>
        /// <param name="ipAddress">客户端IP地址</param>
        /// <param name="appId">应用程序编号</param>
        /// <param name="json">客户端POST过来的json数据</param>
        /// <returns></returns>
        int SaveHistograms(string ipAddress, int appId, JToken json)
        {
            if (json == null) { return 0; }
            string context = json["Context"] == null ? "" : json["Context"].ToString();
            string commandHead = "INSERT INTO MetsHistogram(AppID,HostIP,Name,ContextName,ValueCount,ValueSum,ValueAvg,ValueMin,ValueMax,ValueMedian,HistogramUnit,AddTime) VALUES";
            StringBuilder commandText = new StringBuilder();
            JToken token = null;
            if (json["Histograms"] != null)
            {
                long count, sum;
                List<JToken> timersToken = json["Histograms"].ToList<JToken>();
                for (int index = 0; index < timersToken.Count; index++)
                {
                    token = timersToken[index];
                    count = Convert.ToInt32(token["Count"].ToString());
                    sum = Convert.ToInt64(token["Sum"].ToString());
                    commandText.AppendFormat("('{0}','{1}','{2}','{3}',{4},{5},{6},{7},{8},{9},'{10}',GETDATE()),",
                            appId,
                            ipAddress,
                            token["Name"].ToString().Trim(),
                            context,
                            count,
                            sum,
                            count == 0 ? 0 : sum / count,
                            Convert.ToInt64(token["Min"].ToString()),
                            Convert.ToInt64(token["Max"].ToString()),
                            Convert.ToInt64(token["Median"].ToString()),
                            token["Unit"].ToString()
                        );
                }
            }
            int result = 0;
            string commandValues = commandText.ToString().TrimEnd(',');
            if (!string.IsNullOrEmpty(commandValues))
            {
                result += DBUtil.ExecuteNonQuery(ConnectionStr.FxDb, commandHead + commandValues, null);
            }
            if (json["ChildContexts"] != null)
            {
                List<JToken> childContext = json["ChildContexts"].ToList<JToken>();
                for (int index = 0; index < childContext.Count; index++)
                {
                    result += SaveHistograms(ipAddress, appId, childContext[index]);
                }
            }
            return result;
        }
        #endregion

        #region 保存Meter度量数据
        /// <summary>
        /// 保存Meter度量数据
        /// </summary>
        /// <param name="ipAddress">客户端IP地址</param>
        /// <param name="appId">应用程序编号</param>
        /// <param name="json">客户端POST过来的json数据</param>
        /// <returns></returns>
        int SaveMeters(string ipAddress, int appId, JToken json)
        {
            if (json == null) { return 0; }
            string context = json["Context"] == null ? "" : json["Context"].ToString();
            string commandHead = "INSERT INTO MetsMeter(AppID,HostIP,Name,ContextName,SumRequestCount,RequestCount,MeterUnit,AddTime) VALUES";
            StringBuilder commandText = new StringBuilder();
            JToken token = null;
            if (json["Meters"] != null)
            {
                List<JToken> metersToken = json["Meters"].ToList<JToken>();
                for (int index = 0; index < metersToken.Count; index++)
                {
                    token = metersToken[index];
                    commandText.AppendFormat("('{0}','{1}','{2}','{3}',{4},{5},'{6}',GETDATE()),",
                            appId,
                            ipAddress,
                            token["Name"].ToString().Trim(),
                            context,
                            Convert.ToInt32(token["Count"].ToString().Trim()),
                            Convert.ToInt32(token["UnitCount"].ToString().Trim()),
                            token["Unit"].ToString()
                        );
                }
            }
            int result = 0;
            string commandValues = commandText.ToString().TrimEnd(',');
            if (!string.IsNullOrEmpty(commandValues))
            {
                result += DBUtil.ExecuteNonQuery(ConnectionStr.FxDb, commandHead + commandValues, null);
            }
            if (json["ChildContexts"] != null)
            {
                List<JToken> childContext = json["ChildContexts"].ToList<JToken>();
                for (int index = 0; index < childContext.Count; index++)
                {
                    result += SaveMeters(ipAddress, appId, childContext[index]);
                }
            }
            return result;
        }
        #endregion
    }
}
