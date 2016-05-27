using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace JetermClient.Utility
{
    public class MakePass
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

        /// <summary>
        /// 安全加密处理
        /// </summary>
        /// <param name="passTxt"></param>
        /// <param name="passType"></param>
        /// <returns></returns>
        public static string SafePass(string passTxt, PassType passType)
        {
            return SafePass(passTxt, "Killer#2010", passType);
        }

        /// <summary>
        /// 安全加密处理
        /// </summary>
        /// <param name="passTxt"></param>
        /// <param name="passKey"></param>
        /// <param name="passType"></param>
        /// <returns></returns>
        public static string SafePass(string passTxt, string passKey, PassType passType) {
            string passResultTxt = "";
            try {
                if (passType == PassType.Decrypt) {
                    passResultTxt = DESEncrypt.Decrypt(passTxt, passKey);
                } else {
                    passResultTxt = DESEncrypt.Encrypt(passTxt, passKey);
                }
            } catch { }
            return passResultTxt;
        }
    }

    public enum PassType
    {
        /// <summary>
        /// 加密
        /// </summary>
        Encrypt,
        /// <summary>
        /// 解密
        /// </summary>
        Decrypt
    }
}
