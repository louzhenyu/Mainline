using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace JFxUnitTest.Extensions
{
    [TestClass]
    public class CollectionExtensionsTest
    {
        [TestMethod]
        public void CollectionExtensionsAll()
        {
            List<int> list = new List<int>();

            Assert.IsTrue(list.AddUnique(1));
            Assert.IsFalse(list.AddUnique(1));
            Assert.IsTrue(list.AddUnique(2));

            List<int> list2 = new List<int>() { 1, 3, 4};

            list.AddRangeUnique(list2);

            Assert.IsTrue(list.Count == 4);

            list.RemoveAll(n => n % 2 == 0);

            foreach (int item in list)
            {
                Assert.IsTrue(item % 2 != 0);
            }
        }
    }
}
