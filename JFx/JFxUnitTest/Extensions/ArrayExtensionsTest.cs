using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JFxUnitTest.Extensions
{
    [TestClass]
    public class ArrayExtensionsTest
    {
        [TestMethod]
        public void ArrayExtensionsAll()
        {
            string[] array = null;

            Assert.AreEqual(array.IsNullOrEmpty(), true);
            
            array = new string[] { };
            Assert.AreEqual(array.IsNullOrEmpty(), true);

            array = new string[] { "item" };
            Assert.AreEqual(array.IsNullOrEmpty(), false);

            Assert.AreEqual(array.WithinIndex(0), true);

            Assert.AreEqual(array.WithinIndex(1), false);
        }
    }
}
