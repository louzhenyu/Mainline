using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JinRi.Job.HttpScheduler.Utils
{
    public class Common
    {
        /// <summary>
        /// 使用指定密钥解密
        /// </summary>
        /// <param name="original">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string original, string key)
        {
            return Decrypt(original, key, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 使用指定密钥解密
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码方案</param>
        /// <returns>明文</returns>
        private static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }

        /// <summary>
        /// 使用指定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        private static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
        }

        //----------------------------------------------------------------------------------------

        /// <summary>
        /// 使用指定密钥加密
        /// </summary>
        /// <param name="original">需要加密的字符串</param>
        /// <param name="key">自定义的密钥</param>
        /// <returns>返回加密字符串</returns>
        public static string Encrypt(string original, string key)
        {
            byte[] buff = System.Text.Encoding.Default.GetBytes(original);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return Convert.ToBase64String(Encrypt(buff, kb));
        }

        /// <summary>
        /// 使用指定密钥加密
        /// </summary>
        /// <param name="original">需要加密的字符串</param>
        /// <param name="key">自定义的密钥</param>
        /// <returns>返回加密字符串</returns>
        private static byte[] Encrypt(byte[] original, byte[] key)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;

            return des.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
        }


        //----------------------------------------------------------------------------------------

        /// <summary>
        /// 生成MD5摘要
        /// </summary>
        /// <param name="original">数据源</param>
        /// <returns>摘要</returns>
        private static byte[] MakeMD5(byte[] original)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }


        public static string Encrypt1(string encryptData, string sKey)
        {
            PasswordDeriveBytes pd = new PasswordDeriveBytes(sKey, null);
            byte[] deskey = pd.GetBytes(24);//3DES Key为24位
            byte[] desiv = pd.GetBytes(8);//3DES Key为8位

            byte[] data = Encoding.GetEncoding("GB2312").GetBytes(encryptData);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.IV = desiv;
            des.Key = deskey;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            ICryptoTransform desdecrypt = des.CreateEncryptor();
            byte[] chipcolk = desdecrypt.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(chipcolk);// Encoding.Default.GetString(chipcolk);
        }
        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt1(string descryptData, string sKey)
        {
            PasswordDeriveBytes pd = new PasswordDeriveBytes(sKey, null);
            byte[] deskey = pd.GetBytes(24);//3DES Key为24位
            byte[] desiv = pd.GetBytes(8);//3DES Key为8位

            byte[] data = Convert.FromBase64String(descryptData);// Encoding.Default.GetBytes(chipertext);            
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.IV = desiv;
            des.Key = deskey;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;
            ICryptoTransform desdecrypt = des.CreateDecryptor();
            byte[] chipcolk = desdecrypt.TransformFinalBlock(data, 0, data.Length);
            string strSource = Encoding.GetEncoding("GB2312").GetString(chipcolk);
            return strSource.Replace("\0", "");
        }
    }
}
