using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JFxUnitTest.Extensions
{
    [TestClass]
    public class StringExtensionsTest
    {
        [TestMethod]
        public void StringExtensionsAll()
        {
            string str = null;
            string result = str.IfNull("默认值");
            Assert.AreEqual(result, "默认值");
            str = "1";
            result = str.IfNull("默认值");
            Assert.IsTrue(str == result);

            str = "AB".Repeat(10);
            Console.WriteLine(str);
            Assert.IsTrue(str.Length == 20);


            Console.WriteLine("十几年来，方兴东与马云每年一次，老友聚首，开怀畅谈，阿里上市前，作者再次与马云深度对话".TrimToMaxLength(20, "……"));

            Console.WriteLine("{0}-{1}-{2}".FormatWith(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));

            Assert.IsTrue("ASD".Contains("a", StringComparison.OrdinalIgnoreCase));
        }
    }
}
