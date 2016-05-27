using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JFx.Utils;

namespace JFxUnitTest
{
    [TestClass]
    public class ZipHelperTest
    {
        [TestMethod]
        public void ZipHelperAll()
        {
            Console.WriteLine("当前目录：" + Environment.CurrentDirectory);
            ZipHelper zip = new ZipHelper();

            string zipPath = Environment.CurrentDirectory + "\\ZipFile\\test.zip";
            zip.AddFile(Environment.CurrentDirectory + "\\ZipFile\\6035021_111610206000_2.jpg");
            zip.AddFile(Environment.CurrentDirectory + "\\ZipFile\\test.txt");
            bool zipResult = zip.CompressionZip(zipPath);

            Assert.IsTrue(zipResult);

            Console.WriteLine("压缩成功");

            string[] files = new string[] { };
            bool dezipResult = zip.DeCompressionZip(zipPath, Environment.CurrentDirectory + "\\ZipFile\\DeCompression",out files);

            Assert.IsTrue(dezipResult);
            Console.WriteLine("解压成功，文件列表：");
            for (int index = 0; index < files.Length; index++)
            {
                Console.WriteLine(files[index]);
            }

            Assert.IsTrue(files.Length == 2);
        }
    }
}
