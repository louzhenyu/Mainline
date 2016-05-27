using Metrics.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metrics.Reporters
{
    public class ServiceReporter : MetricsReporter
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();
        /// <summary>
        /// Metrics Server 端地址
        /// </summary>
        public string ServiceUrl { get; set; }
        public ServiceReporter(string serviceUrl)
        {
            this.ServiceUrl = string.IsNullOrEmpty(serviceUrl) ? System.Configuration.ConfigurationManager.AppSettings["Metrics.ServiceReportUrl"] : serviceUrl;
        }
        public void RunReport(MetricData.MetricsData metricsData, Func<HealthStatus> healthStatus, System.Threading.CancellationToken token)
        {
            try
            {
            if (string.IsNullOrEmpty(this.ServiceUrl))
            {
                return;
            }
            var json = Metrics.Json.JsonBuilderV2.BuildJson(metricsData);
            Metric.Advanced.ResetMetricsValues();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    string url = ServiceUrl + "/Save";
                    string para = string.Format("metricsValue={0}", json);
                    DateTime now = DateTime.Now;
                    string result = new Metrics.Utils.HttpHelper().HttpPost(url, para, 30000, Encoding.UTF8);
                    log.Info(string.Format("RequestTime:{0} ServiceUrl:{1} ResponseTime:{2} Result:{3}", now.ToString("yyyy-MM-dd HH:mm:ss"), ServiceUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), result));
                }
                catch (Exception ex)
                {
                    log.Info(string.Format("[{0}] ServiceReporter.RunReport Exception:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString()));
//                    //log.ErrorException(string.Format("[{0}] ServiceReporter.RunReport Exception:{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.Message), ex);
//#if DEBUG                   
//                    Console.WriteLine("ServiceReporter.cs - RunReport()：现在时间为 - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "；" + "异常信息：" + ex.ToString());
//#endif
                }
            });
            }
            catch (Exception ex)
            {
                log.Info(ex.ToString());
            }
        }
    }
}
