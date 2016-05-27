using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JinRi.Fx.Utility
{
    /// <summary>
    ///  Web请求数据通用工具类
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 执行HTTP POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="encoding">编码</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="reqType">请求类型,xml,url,json</param>
        /// <returns></returns>
        public static string HttpPost(string url, string parameter, Encoding encoding, int timeout, string reqType)
        {
            HttpWebRequest _request = null;
            HttpWebResponse res = null;
            string result = null;
            try
            {
                if (_request == null) _request = CreateHttpWebRequest(url, timeout, reqType);

                if (parameter == null) parameter = "";
                byte[] submitData = encoding.GetBytes(parameter);

                using (Stream stream = _request.GetRequestStream())
                {
                    stream.Write(submitData, 0, submitData.Length);
                }
                res = (HttpWebResponse)_request.GetResponse();

                using (Stream stm = res.GetResponseStream())
                {
                    //Encoding.GetEncoding("utf-8")
                    using (StreamReader sr = new StreamReader(stm, encoding))
                    {
                        try
                        {
                            result = sr.ReadToEnd();
                        }
                        catch
                        {
                        }
                        return result;
                    }
                }
            }
            catch { return null; }
            finally
            {
                if (_request != null)
                {
                    _request.Abort();
                    _request = null;
                }
            }
        }

        /// <summary>
        /// 执行HTTP POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameter">请求参数</param>
        /// <param name="encoding">编码</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="reqType">请求类型,xml,url,json</param>
        /// <param name="strErrorMsg">返回错误说明</param>
        /// <returns></returns>
        public static string HttpPost(string url, string parameter, Encoding encoding, int timeout, string reqType, out string strErrorMsg)
        {
            HttpWebRequest _request = null;
            HttpWebResponse res = null;
            string result = null;
            strErrorMsg = null;
            try
            {
                if (_request == null) _request = CreateHttpWebRequest(url, timeout, reqType);
                if (parameter == null) parameter = "";
                byte[] submitData = encoding.GetBytes(parameter);

                using (Stream stream = _request.GetRequestStream())
                {
                    stream.Write(submitData, 0, submitData.Length);
                }
                res = (HttpWebResponse)_request.GetResponse();

                using (Stream stm = res.GetResponseStream())
                {
                    //Encoding.GetEncoding("utf-8")
                    using (StreamReader sr = new StreamReader(stm, encoding))
                    {
                        try
                        {
                            result = sr.ReadToEnd();
                        }
                        catch (Exception ex1)
                        {
                            strErrorMsg = ex1.Message;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex2)
            {
                strErrorMsg = ex2.Message;
                return result;
            }
            finally
            {
                if (_request != null)
                {
                    _request.Abort();
                    _request = null;
                }

            }
        }

        /// <summary>
        /// 生成新请求实例
        /// </summary>
        /// <param name="url"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private static HttpWebRequest CreateHttpWebRequest(string url, int time, string reqType)
        {
            HttpWebRequest _Request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            _Request.Timeout = time * 1000;
            _Request.ServicePoint.Expect100Continue = false;
            switch (reqType)
            {
                case "xml":
                    _Request.ContentType = "application/xml";
                    break;
                case "json":
                    _Request.ContentType = "application/json";
                    break;
                case "url":
                default:
                    _Request.ContentType = "application/x-www-form-urlencoded";
                    break;
            }
            _Request.Method = "POST";

            return _Request;
        }

    }
}
