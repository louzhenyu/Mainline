using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace JinRi.Fx.Utility
{
    public class AppSettingsHelper
    {
        public static string ZKServer
        {
            get
            {
                return ConfigurationManager.AppSettings["ZKServer"];
            }
        }
        public static double ZKSessionTimeOut
        {
            get
            {
                string zkSessionTimeOut = ConfigurationManager.AppSettings["ZKSessionTimeOut"];
                if (string.IsNullOrWhiteSpace(zkSessionTimeOut))
                {
                    return 3600;
                }
                return double.Parse(zkSessionTimeOut);
            }
        }

        public static string JSOAUIAddress
        {
            get
            {
                return ConfigurationManager.AppSettings["JSOAUIAddress"];
            }
        }

        public static string ConfigCenterMenuName
        {
            get
            {
                return ConfigurationManager.AppSettings["ConfigCenterMenuName"];
            }
        }
    }
}
