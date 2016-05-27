using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Metrics.Utils
{
    public class HttpHelper
    {
        public string HttpGet(string strURL, int timeOut )
        {
            return HttpGet(strURL, timeOut, Encoding.Default);
        }
        /// <summary>
        /// GET方式
        /// </summary>
        /// <param name="strURL">URI资源</param>
        /// <param name="timeOut">超时值：以毫秒为单位</param>
        /// <returns></returns>
        public string HttpGet(string strURL, int timeOut, Encoding encode)
        {
            StringBuilder rStr = new StringBuilder();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader streamReader = null;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(strURL);
                request.Timeout = timeOut;
                response = (HttpWebResponse)request.GetResponse();
                streamReader = new StreamReader(response.GetResponseStream(), encode);
                rStr.Append(streamReader.ReadToEnd());
                streamReader.Close();
            }
            catch (Exception ex) { throw ex; }
            finally
            {
                if (streamReader != null) { streamReader.Close(); }
                if (response != null) { response.Close(); }
                if (request != null) { request.Abort(); }
            }
            return rStr.ToString();
        }

        public string HttpPost(string strURL, string strParm, int timeOut)
        {
            return HttpPost(strURL, strParm, timeOut, Encoding.Default);
        }
        /// <summary>
        /// POST方式
        /// </summary>
        /// <param name="strURL">URI资源</param>
        /// <param name="strParm">POST参数</param>
        /// <param name="timeOut">超时值：以毫秒为单位</param>
        /// <returns></returns>
        public string HttpPost(string strURL, string strParm, int timeOut, Encoding coding)
        {
            StringBuilder rStr = new StringBuilder();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            StreamReader streamReader = null;
            Stream requestStream = null;
            try
            {
                byte[] postBytes = Encoding.UTF8.GetBytes(strParm);
                request = (HttpWebRequest)HttpWebRequest.Create(strURL);
                request.Timeout = timeOut;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBytes.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                response = (HttpWebResponse)request.GetResponse();
                streamReader = new StreamReader(response.GetResponseStream(), coding);
                rStr.Append(streamReader.ReadToEnd());
                streamReader.Close();
                requestStream.Close();
            }
            //catch
            //{

            //}
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine("HttpHelper.cs - HttpPost(): 现在时间为 - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "；异常信息为：" + ex.ToString());
#endif
            }
            finally
            {
                if (streamReader != null) { streamReader.Close(); }
                if (requestStream != null) { requestStream.Close(); }
                if (response != null) { response.Close(); }
                if (request != null) { request.Abort(); }
            }
            return rStr.ToString();
        }
    }
}
