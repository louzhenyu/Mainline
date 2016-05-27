using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Data
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

         private static string m_JinRiDb;

        /// <summary>
        /// JinRiDb
        /// </summary>
         public static string JinRiDb
        {
            get
            {
                if (string.IsNullOrEmpty(m_JinRiDb))
                {
                    m_JinRiDb = EncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["JinRiConnectionStr"].ConnectionString, "BeiJing#2008"); ;
                }
                return m_JinRiDb;
            }
        }
        
    }
}
