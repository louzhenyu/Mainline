using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace JetermClient.Utility
{
    public class MD5Helper
    {
        public static string MakeMD5(string original, Encoding encoding)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(encoding.GetBytes(original));
            StringBuilder stringBuilder = new StringBuilder(32);
            for (int i = 0; i < md5Hash.Length; i++)
            {
                stringBuilder.Append(md5Hash[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取MD5加密
        /// </summary>
        /// <param name="s">要加密的字符串</param>
        /// <returns>加密过后的字符串</returns>
        public static string GetMD5(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();

        }

        /// <summary>
        /// 获取MD5加密
        /// </summary>
        /// <param name="s">要加密的字符串</param>
        /// <returns>加密过后的字符串</returns>
        public static string GetMD5(byte[] s)
        {

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(s);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();

        }


        public static string GetMD5_Utf8(string s)
        {

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            byte[] t = md5.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString().ToUpper();

        }


        public static string GetMD5(string s, string _input_charset)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();

        }
    }
}
