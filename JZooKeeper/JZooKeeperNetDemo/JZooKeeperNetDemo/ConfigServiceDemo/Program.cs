using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZooKeeperNet;
using System.Threading;

namespace ConfigServiceDemo
{
    /// <summary>
    /// 这是匹配【配置中心】应用场景的Demo
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using (ZooKeeper zk = new ZooKeeper(ConfigServiceHelper.ZKServer, TimeSpan.FromSeconds(ConfigServiceHelper.ZKSessionTimeOut), null))
            {
                ZooKeeper.WaitUntilConnected(zk);
                string cookieNameKeyPath = string.Format("{0}/{1}", ConfigServiceHelper.ZKRootPathWithAppID, "CookieKeyName");              
                if (zk.Exists(cookieNameKeyPath, false) != null)
                {
                    byte[] _nodeValue = zk.GetData(cookieNameKeyPath, false, null);
                    Console.WriteLine(Encoding.UTF8.GetString(_nodeValue));
                }
            }
            Console.ReadLine();            
        }
    }
}
