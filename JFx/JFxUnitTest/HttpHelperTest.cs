using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JFx.Utils;

namespace JFxUnitTest
{
    [TestClass]
    public class HttpHelperTest
    {
        [TestMethod]
        public void HttpHelperAll()
        {
            string str = HttpHelper.SendGet("http://www.jinri.cn/Images/Index_v18/about.html");
            Console.WriteLine(str);
        }
    }
}
