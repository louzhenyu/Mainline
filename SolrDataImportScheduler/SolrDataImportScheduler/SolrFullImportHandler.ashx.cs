using System.Collections.Generic;
using System.Configuration;
using System.Web;
using SolrNet.Impl;

namespace SolrDataImportScheduler
{
    public class SolrFullImportHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var core = context.Request.QueryString["core"];
            if (string.IsNullOrEmpty(core))
            {
                return;
            }
            FullImport(core);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static void FullImport(string core)
        {
            string command = "full-import";
            string clean = "true";
            string commit = "true";
            var conn = new SolrConnection(ConfigurationManager.AppSettings["SolrConnString"] + core);
            var relativeUrl = "/dataimport";
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("command", command),
                new KeyValuePair<string, string>("clean", clean),
                new KeyValuePair<string, string>("commit", commit)
            };
            conn.Get(relativeUrl, parameters);
        }
    }
}