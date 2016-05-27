using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FX.CTI.ConfigHelper;
using RestSharp;

namespace FX.CTI.SMSWS
{
    public class SMSSenderSxun : ISMSSender
    {
        public bool SendSMS(string mobile, string content)
        {
            try
            {
                var client = new RestClient {BaseUrl = new Uri(ConfigMgr.SxunUrl)};
                var request = new RestRequest(Method.POST);
                request.Timeout = 15000;
                request.ReadWriteTimeout = 15000;
                request.AddParameter("userid", ConfigMgr.SxunUserId);
                request.AddParameter("account", ConfigMgr.SxunAccount);
                request.AddParameter("password", ConfigMgr.SxunPassword);
                request.AddParameter("mobile", mobile);
                request.AddParameter("content", content);
                request.AddParameter("sendTime", "");
                request.AddParameter("action", "send");
                request.AddParameter("extno", "");
                var response = client.Execute<SxunResponse>(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }
                if (response.Data.returnstatus != "Success")
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;            
        }
    }
    public class SxunResponse
    {
        public string returnstatus { get; set; }
        public string message { get; set; }
        public string remainpoint { get; set; }
        public string taskID { get; set; }
        public string successCounts { get; set; }
    }
}
