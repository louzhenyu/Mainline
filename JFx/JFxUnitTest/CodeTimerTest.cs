using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JFx.Utils;
using System.Text;

namespace JFxUnitTest
{
    [TestClass]
    public class CodeTimerTest
    {
        [TestMethod]
        public void CodeTimerAll()
        {
            CodeTimer.Initialize();
            int times = 1000;
            string s = "";
            CodeTimer.Time("String Concat", times, () => { s += "a"; });

            StringBuilder sb = new StringBuilder();
            CodeTimer.Time("StringBuilder", times, () => { sb.Append("a"); });
        }
    }
}
