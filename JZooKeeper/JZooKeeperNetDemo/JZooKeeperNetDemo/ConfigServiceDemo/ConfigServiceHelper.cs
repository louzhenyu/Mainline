using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace ConfigServiceDemo
{
    class ConfigServiceHelper
    {
        internal static string ZKServer
        {
            get
            {
                return ConfigurationManager.AppSettings["ZKServer"];
            }
        }
        internal static double ZKSessionTimeOut
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
        internal static string ZKRootPathWithAppID
        {
            get
            {
                string appID = ConfigurationManager.AppSettings["AppID"];
                return string.Format(@"/ConfigService/{0}", appID);
            }
        }
    }
}
