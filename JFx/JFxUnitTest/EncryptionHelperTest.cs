using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JFx;

namespace JFxUnitTest
{
    [TestClass]
    public class EncryptionHelperTest
    {
        [TestMethod]
        public void EncryptionHelperAll()
        {
            string encryptString = "RANEN.TONG";
            string encryptKey = "FX.JINRI.ORG.CN";

            string encrypt = EncryptionHelper.EncryptByAES(encryptString, encryptKey);
            string souce = EncryptionHelper.DecryptByAES(encrypt, encryptKey);

            Console.WriteLine("***********AES************");
            Console.WriteLine(string.Format("字符串:{0}  Key:{1}  加密结果:{2}", encryptString, encryptKey, encrypt));
            Console.WriteLine(string.Format("解密结果：{0}", souce));
            Assert.IsTrue(souce == encryptString);

            Console.WriteLine("***********DES************");
            encrypt = EncryptionHelper.EncryptByDES(encryptString, encryptKey);
            souce= EncryptionHelper.DecryptByDES(encrypt, encryptKey);
            Console.WriteLine(string.Format("字符串:{0}  Key:{1}  加密结果:{2}", encryptString, encryptKey, encrypt));
            Console.WriteLine(string.Format("解密结果：{0}", souce));
            Assert.IsTrue(souce == encryptString);

            Console.WriteLine("***********MD5************");
            Console.WriteLine(string.Format("源字符串：{0} MD5:{1}", encryptString, EncryptionHelper.Md5(encryptString)));
        }
    }
}
