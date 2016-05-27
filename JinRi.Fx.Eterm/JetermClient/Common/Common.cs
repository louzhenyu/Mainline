using System;
using System.Text;
using JetermClient.Utility;
using System.Configuration;

namespace JetermClient.Common
{
    public class Common
    {
        /// <summary>
        /// SQL连接字符串
        /// </summary>
        public static string ConnectString
        {
            get
            {
                if (string.IsNullOrEmpty(_ConnectString))
                    _ConnectString = MakePass.Decrypt(ConfigurationManager.ConnectionStrings["FXDB_SELECT"].ToString(), "XDfg%3f*");
                return _ConnectString;
            }
        }

        private static string _ConnectString = string.Empty;
    }
}
