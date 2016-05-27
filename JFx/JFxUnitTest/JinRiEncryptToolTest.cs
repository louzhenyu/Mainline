using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JFx;

namespace JFxUnitTest
{
    [TestClass]
    public class JinRiEncryptToolTest
    {
        [TestMethod]
        public void JinRiEncryptToolAll()
        {
            string encryptString = "GyEItJtJeAgBrfz5z4LSm0jDqWYl4RhmF6ydL7RAZ0i2BZEYiA5luSxcVbJkR/I5a4IxQiWoq0bjOEoreKqhiYQms3Fx6IoboFNaOb9bu4PixUi2F5AKOA==";
            Console.WriteLine(JinRiEncryptTool.Decrypt(encryptString, "BeiJing#2008"));
        }
    }
}
