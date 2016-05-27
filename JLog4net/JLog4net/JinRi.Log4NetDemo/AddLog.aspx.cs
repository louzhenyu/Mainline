using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Configuration;
namespace JinRi.Log4NetDemo
{
    public partial class AddLog : System.Web.UI.Page
    {

        private static ILog fileLog = LogManager.GetLogger("FileLogger");
        private static ILog log = LogManager.GetLogger(typeof(AddLog));
        //private string traceId = "W2014122404021567278W2014122404021567278W2014122404021567278W2014122404021567278W2014122404021567278W2014122404021567278W2014122404021567278W2014122404021567278";//System.Guid.NewGuid().ToString();

        private string traceId = System.Guid.NewGuid().ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 业务日志，记录到SQLSERVER
        /// </summary>
        private void BusLogAdd()
        {
            ILog busLogger = LogManager.GetLogger("BusOrderLogger");
            ThreadContext.Properties["userid"] = "9527";
            ThreadContext.Properties["username"] = "xupearl";
            ThreadContext.Properties["saleAmount"] = 16;
            ThreadContext.Properties["TraceId"] = System.Guid.NewGuid().ToString();
            busLogger.Info("business erro");
        }


        private void MongoLogAdd1()
        {
            string msg = "测试TraceId日志调用链1";
            ThreadContext.Properties["TraceId"] = traceId;
            log.Info(msg);
        }
        private void MongoLogAdd2()
        {
            string msg = "测试TraceId日志调用链2";
            ThreadContext.Properties["TraceId"] = traceId;
            log.Info(msg);
        }

        /// <summary>
        /// 系统日志，记录到mongodb
        /// </summary>
        private void MongoLogAdd()
        {
            string ss = DateTime.Now.ToString() + "订单号:W2014122404021567278,解冻,请求结果:<html><script language=\"javascript\">window.location.href='http://127.0.0.1/?bargainor_id=1215068201&bus_args=347958%7C729788738%5E347958&bus_type=97&cmdno=98&pay_info=ok&pay_result=0&refund_id=1091215068201201412249395236&sign=73ED2933AE98E88EB7349397906E42AD&sp_billno=W2014122404021567278&transaction_id=1215068201201412241914387331&version=4';</script></html>";
            ThreadContext.Properties["TraceId"] = System.Guid.NewGuid().ToString();
            log.Info(ss);
            log.Error(ss);
            log.Warn(ss);
        }
        /// <summary>
        /// 文本日志
        /// </summary>
        private void FileLogAdd()
        {
            string ss = "订单号:W2014122404021567278,解冻,请求结果:<html><script language=\"javascript\">window.location.href='http://127.0.0.1/?bargainor_id=1215068201&bus_args=347958%7C729788738%5E347958&bus_type=97&cmdno=98&pay_info=ok&pay_result=0&refund_id=1091215068201201412249395236&sign=73ED2933AE98E88EB7349397906E42AD&sp_billno=W2014122404021567278&transaction_id=1215068201201412241914387331&version=4';</script></html>";
            fileLog.Info(ss);
        }

        protected void btnBussLog_Click(object sender, EventArgs e)
        {
            BusLogAdd();
        }

        protected void btnFileLog_Click(object sender, EventArgs e)
        {
            FileLogAdd();
        }

        protected void btnSystemLog_Click(object sender, EventArgs e)
        {
            DateTime d1 = DateTime.Now;
            MongoLogAdd();
            MongoLogAdd1();
            MongoLogAdd2();
            DateTime d2 = DateTime.Now;
            TimeSpan t = d2 - d1;
            lblTime.Text = t.TotalSeconds.ToString();
        }
    }
}