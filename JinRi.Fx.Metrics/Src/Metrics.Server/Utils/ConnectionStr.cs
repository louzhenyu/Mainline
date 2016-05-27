using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MetricsServer.Utils
{
    public class ConnectionStr
    {
        private static string m_FxDb;
        /// <summary>
        /// FxDb
        /// </summary>
        public static string FxDb
        {
            get
            {
                if (string.IsNullOrEmpty(m_FxDb))
                {
                    m_FxDb = EncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["FxDBConnectionStr"].ConnectionString, "BeiJing#2008"); ;
                }
                return m_FxDb;
            }
        }

    }
}