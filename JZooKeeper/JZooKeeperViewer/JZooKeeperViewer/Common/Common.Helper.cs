using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Net;

namespace JZooKeeperViewer.Common
{
    //added by Yang Li
    public partial class Common
    {
        internal static string AppID
        {
            get
            {
                return ConfigurationManager.AppSettings["AppID"];
            }
        }
        internal static string ZKServer
        {
            get
            {
                return ConfigurationManager.AppSettings["ZKServer"];
            }
        }
        internal static string ZKSessionTimeOut
        {
            get
            {
                string zkSessionTimeOut = ConfigurationManager.AppSettings["ZKSessionTimeOut"];
                if (string.IsNullOrWhiteSpace(zkSessionTimeOut))
                {
                    return "3600";
                }
                return zkSessionTimeOut;
            }
        }
        internal static IPAddress GetHostIP()
        {
            string name = Dns.GetHostName();
            IPHostEntry me = Dns.GetHostEntry(name);
            IPAddress[] ips = me.AddressList;
            foreach (IPAddress ip in ips)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    continue;
                }

                return ip;
            }
            return ips != null && ips.Length > 0 ? ips[0] : new IPAddress(0x0);
        }
    }
}
