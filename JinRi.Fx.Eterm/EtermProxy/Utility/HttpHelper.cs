using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EtermProxy.Utility
{
    public class HttpHelper
    {
        /// <summary>
        /// 解决URL中文编码
        /// </summary>
        /// <param name="strParm">参数</param>
        /// <returns></returns>
        public static string UrlEncodeGB2312(string strParm)
        {
            byte[] bs = Encoding.GetEncoding("gb2312").GetBytes(strParm);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bs.Length; i++)
            {
                if (bs[i] < 128)
                    sb.Append((char)bs[i]);
                else
                {
                    sb.Append("%" + bs[i++].ToString("x").PadLeft(2, '0'));
                    sb.Append("%" + bs[i].ToString("x").PadLeft(2, '0'));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// GET方式
        /// </summary>
        /// <param name="strURL">URI资源</param>
        /// <param name="timeOut">超时值：以毫秒为单位</param>
        /// <returns></returns>
        public static string HttpGet(string strURL, int timeOut)
        {
            StringBuilder rStr = new StringBuilder();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader streamReader = null;
            try
            {
                strURL = UrlEncodeGB2312(strURL);
                request = (HttpWebRequest)HttpWebRequest.Create(strURL);
                request.Timeout = timeOut;
                response = (HttpWebResponse)request.GetResponse();
                streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                rStr.Append(streamReader.ReadToEnd());
                streamReader.Close();
            }
            catch { }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return rStr.ToString();
        }

        /// <summary>
        /// POST方式
        /// </summary>
        /// <param name="strURL">URI资源</param>
        /// <param name="strParm">POST参数</param>
        /// <param name="timeOut">超时值：以毫秒为单位</param>
        /// <returns></returns>
        public static string HttpPost(string strURL, string strParm, int timeOut)
        {
            StringBuilder rStr = new StringBuilder();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader streamReader = null;
            Stream requestStream = null;
            try
            {
                byte[] postBytes = Encoding.ASCII.GetBytes(UrlEncodeGB2312(strParm));
                request = (HttpWebRequest)HttpWebRequest.Create(strURL);
                request.Timeout = timeOut;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBytes.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                response = (HttpWebResponse)request.GetResponse();
                streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                rStr.Append(streamReader.ReadToEnd());
                streamReader.Close();
                requestStream.Close();
            }
            catch { }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (requestStream != null)
                {
                    requestStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            return rStr.ToString();
        }

        
    }
}
