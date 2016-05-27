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
        private static string m_DomesticDD;

         /// <summary>
         /// 国内测试master库
         /// </summary>
         public static string DomesticDD
         {
             get
             {
                 if (string.IsNullOrEmpty(m_DomesticDD))
                 {
                     m_DomesticDD = EncryptTool.Decrypt(ConfigurationManager.ConnectionStrings["DomesticDDConnectionStr"].ConnectionString, "BeiJing#2008"); ;
                 }
                 return m_DomesticDD;
             }
         }

         private static string m_InternationalDD;

        /// <summary>
        /// 国际测试master库
        /// </summary>
        public static string InternationalDD
        {
            get
            {
                if (string.IsNullOrEmpty(m_InternationalDD))
                {
                    m_InternationalDD =
                        EncryptTool.Decrypt(
                            ConfigurationManager.ConnectionStrings["InternationalDDConnectionStr"].ConnectionString,
                            "BeiJing#2008");
                    ;
                }
                return m_InternationalDD;
            }
        }
    }
}
