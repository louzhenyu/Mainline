using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JFx;

namespace JFxUnitTest
{
    [TestClass]
    public class AppEnvironmentTest
    {
        [TestMethod]
        public void AppEnvironmentAll()
        {
            Console.WriteLine(string.Format("AppEnvironment.CommandLine:{0}", AppEnvironment.CommandLine));
            Console.WriteLine(string.Format("AppEnvironment.CPUCount:{0}", AppEnvironment.CPUCount));
            Console.WriteLine(string.Format("AppEnvironment.HostName:{0}", AppEnvironment.HostName));
            Console.WriteLine(string.Format("AppEnvironment.LocalIPAddress:{0}", AppEnvironment.LocalIPAddress));
            Console.WriteLine(string.Format("AppEnvironment.MachineName:{0}", AppEnvironment.MachineName));
            Console.WriteLine(string.Format("AppEnvironment.OSVersion:{0}", AppEnvironment.OSVersion));
            Console.WriteLine(string.Format("AppEnvironment.UserDomainName:{0}", AppEnvironment.UserDomainName));
            Console.WriteLine(string.Format("AppEnvironment.UserName:{0}", AppEnvironment.UserName));
            Console.WriteLine(string.Format("AppEnvironment.AppId:{0}", AppEnvironment.AppId));
        }
    }
}
