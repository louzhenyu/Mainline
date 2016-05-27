using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using RemotableObjects;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using JinRi.Jason.Alipay;
using log4net;

namespace JetermClient.Utility
{
    public class HttpService
    {
        private static ILog log = LogManager.GetLogger(typeof(HttpService));

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
        /// <param name="strURL">URL资源</param>
        /// <param name="timeOut">超时值：以毫秒为单位</param>
        /// <param name="dtStr">请求时间</param>
        /// <returns></returns>
        public static string HttpGet(string strURL, int timeOut, string dtStr)
        {
            string innerErrorMessage = string.Empty;

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
            catch (Exception ex)
            {
                innerErrorMessage = string.Format("JetermClient的HttpGet请求抛异常：{0}请求时间：{1}{0}请求url:{0}{2}{0}返回：{0}{3}{0}异常信息为：{0}{4}", Environment.NewLine, dtStr, strURL, (rStr.Length == 0 ? "返回为空" : rStr.ToString()), ex.ToString());
                log.Error(innerErrorMessage);
            }
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

            if (!string.IsNullOrWhiteSpace(innerErrorMessage))
            {
                throw new Exception(innerErrorMessage);
            }            
            return rStr.ToString();
        }

        /// <summary>
        /// POST方式
        /// </summary>
        /// <param name="strURL">URI资源</param>
        /// <param name="strParm">POST参数</param>
        /// <param name="timeOut">超时值：以毫秒为单位</param>
        /// <param name="dtStr">请求时间</param>
        /// <param name="method">方法名</param>
        /// <param name="i">第几次请求</param>       
        /// <returns></returns>
        public static string HttpPost(string strURL, string strParm, int timeOut, string dtStr, string method = "Common", int i = 0)
        {
            string innerErrorMessage = string.Empty;

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
                //request.ServicePoint.Expect100Continue = false;
                request.ContentLength = postBytes.Length;
                requestStream = request.GetRequestStream();
                requestStream.Write(postBytes, 0, postBytes.Length);
                response = (HttpWebResponse)request.GetResponse();
                streamReader = new StreamReader(response.GetResponseStream(), Encoding.Default);
                rStr.Append(streamReader.ReadToEnd());
                streamReader.Close();
                requestStream.Close();
            }
            catch (Exception ex)
            {
                innerErrorMessage = string.Format("JetermClient.{0}的HttpPost{1}请求抛异常：{2}请求时间：{3}{2}请求url:{2}{4}{2}请求数据:{2}{5}{2}返回：{2}{6}{2}异常信息为：{2}{7}", method, (i > 0 ? string.Format("第{0}次", i) : string.Empty), Environment.NewLine, dtStr, strURL, strParm, (rStr.Length == 0 ? "返回为空" : rStr.ToString()), ex.ToString());
                log.Error(innerErrorMessage);
            }
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

            if (!string.IsNullOrWhiteSpace(innerErrorMessage))
            {
                throw new Exception(innerErrorMessage);
            }
            return rStr.ToString();
        }

        /// <summary>
        /// HttpPost方法
        /// </summary>
        /// <param name="Address">请求的地址(IP:端口)</param>
        /// <param name="method">方法名称</param>
        /// <param name="strPost">请求数据</param>
        /// <param name="dtStr">请求时间</param>
        /// <param name="Conifg">配置名</param>
        /// <param name="i">第几次请求</param>
        /// <param name="RecvTimeOut">返回超时</param>
        /// <param name="ReqTimeout">请求超时</param>        
        /// <returns>返回数据</returns>
        public static string HttpPost(string Address, string method, string strPost, string dtStr, string Conifg = "", int i = 0, int RecvTimeOut = 3000, int ReqTimeout = 1000)
        {
            string innerErrorMessage = string.Empty;

            string data = string.Empty;
            Socket socket = null;

            try
            {
                string[] strAddress = Address.Split(':');

                TimeoutObject.Reset();

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                IPAddress ip = IPAddress.Parse(strAddress[0]);

                byte[] postBytes = Encoding.ASCII.GetBytes(UrlEncodeGB2312(strPost));

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("POST /format=json&language=CSharp&method={0}{1} HTTP/1.1\r\n", method, string.IsNullOrEmpty(Conifg) ? string.Empty : string.Format("&USING={0}", Conifg));
                sb.Append("Content-Type: application/x-www-form-urlencoded\r\n");
                sb.AppendFormat("Host: {0}\r\n", Address);
                sb.AppendFormat("Content-Length: {0}\r\n", postBytes.Length);
                sb.Append("Connection: Keep-Alive\r\n\r\n");

                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, RecvTimeOut);

                socket.BeginConnect(new IPEndPoint(ip, Convert.ToInt32(strAddress[1])), CallBackMethod, socket);

                //阻塞当前线程
                if (!TimeoutObject.WaitOne(ReqTimeout, false))
                {
                    //连接超时
                    return string.Empty;
                }

                byte[] sendData = Encoding.ASCII.GetBytes(UrlEncodeGB2312(sb.ToString()));

                byte[] nCon = new byte[sendData.Length + postBytes.Length];
                sendData.CopyTo(nCon, 0);
                postBytes.CopyTo(nCon, sendData.Length);

                socket.Send(nCon);

                byte[] recive = new byte[40960];

                int nlen = 0;
                int ntotal = 0;
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"(?<=Content-Length:)\d{1,8}");

                while (true)
                {
                    int nrec = socket.Receive(recive);
                    if (nrec == 0) break;
                    ntotal += nrec;
                    string header = Encoding.Default.GetString(recive);
                    if (nlen == 0)
                    {
                        System.Text.RegularExpressions.Match match = reg.Match(header);
                        if (match.Success)
                        {
                            nlen = Convert.ToInt32(match.Value);
                        }
                    }
                    data += header;
                    int npos = data.IndexOf("\r\n\r\n");
                    if (npos != -1)
                    {
                        data = data.Substring(npos + 4);
                    }
                    if (ntotal - 78 - nlen.ToString().Length >= nlen)
                    {
                        break;
                    }
                }

                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception ex)
            {
                innerErrorMessage = string.Format("JetermClient.{0}的HttpPost{1}请求抛异常：{2}请求时间：{3}{2}请求EtermServer服务器地址：{4}{2}请求数据：{2}{5}{2}返回：{2}{6}{2}异常信息为：{2}{7}", method, (i > 0 ? string.Format("第{0}次", i) : string.Empty), Environment.NewLine, dtStr, Address, strPost, (string.IsNullOrWhiteSpace(data) ? "返回为空" : data), ex.ToString());
                log.Error(innerErrorMessage);
            }
            finally
            {
                if (socket != null)
                {
                    socket.Close();
                }
            }

            if (!string.IsNullOrWhiteSpace(innerErrorMessage))
            {
                throw new Exception(innerErrorMessage);
            }
            return data;
        }

        private static readonly ManualResetEvent TimeoutObject = new ManualResetEvent(false);

        //--异步回调方法
        private static void CallBackMethod(IAsyncResult asyncresult)
        {
            //使阻塞的线程继续
            TimeoutObject.Set();
        }

        /// <summary>
        /// Remote调用
        /// </summary>
        /// <param name="Cmd"></param>
        /// <param name="timeout"></param>
        /// <param name="dtStr"></param>
        /// <returns></returns>
        public static string RemoteCmd(string url, string Cmd, int timeout, string dtStr)
        {
            string innerErrorMessage = string.Empty;

            string s = null;
            try
            {
                string httpChannelName = "remoting";
                HttpClientChannel chan = (HttpClientChannel)ChannelServices.GetChannel(httpChannelName);
                if (chan == null)
                {
                    chan = new HttpClientChannel(httpChannelName, null);

                    IDictionary properties = new Hashtable();
                    properties["name"] = httpChannelName;
                    properties["timeout"] = timeout;

                    chan = new HttpClientChannel(properties, null);
                    ChannelServices.RegisterChannel(chan, false);
                }

                // Create an instance of the remote object
                MyRemotableObject remoteObject = (MyRemotableObject)Activator.GetObject(typeof(MyRemotableObject), url);
                s = remoteObject.SetMessage(Cmd);
            }
            catch (Exception ex)
            {
                innerErrorMessage = string.Format("JetermClient.Common的RemoteCmd请求抛异常：{0}请求时间：{1}{0}请求url:{0}{2}{0}请求数据:{0}{3}{0}返回：{0}{4}{0}异常信息为：{0}{5}", Environment.NewLine, dtStr, url, Cmd, (string.IsNullOrWhiteSpace(s) ? "返回为空" : s), ex.ToString());
                log.Error(innerErrorMessage);
            }

            if (!string.IsNullOrWhiteSpace(innerErrorMessage))
            {
                throw new Exception(innerErrorMessage);
            }
            return s;
        }
    }


    public class PostUrl
    {
        /// <summary>
        /// 获取远程的字符串
        /// </summary>
        /// <param name="Url">url</param>
        /// <returns>string</returns>
        public string GetRemoteCode(String Url)
        {
            DateTime dt = DateTime.Now;
            string dtStr = dt.ToString("yyyy-MM-dd HH:mm:ss ffff");
            string strResult = HttpService.HttpGet(Url, 20000, dtStr);
            if (String.IsNullOrEmpty(strResult))
            {
                strResult = "err";
            }
            return strResult;
        }

        public string GetRemoteCode(String Url, int timeOut)
        {
            DateTime dt = DateTime.Now;
            string dtStr = dt.ToString("yyyy-MM-dd HH:mm:ss ffff");
            string strResult = HttpService.HttpGet(Url, timeOut, dtStr);
            if (String.IsNullOrEmpty(strResult))
            {
                strResult = "err";
            }
            return strResult;
        }


        #region Model
        private string _airline;
        private string _cabinlist;
        private string _discountlist;
        private DateTime _startdate;
        private DateTime _enddate;
        /// <summary>
        /// 
        /// </summary>
        public string Airline
        {
            set { _airline = value; }
            get { return _airline; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CabinList
        {
            set { _cabinlist = value; }
            get { return _cabinlist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DiscountList
        {
            set { _discountlist = value; }
            get { return _discountlist; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate
        {
            set { _startdate = value; }
            get { return _startdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }

        private string _Scity;
        public string Scity
        {
            get { return _Scity; }
            set { _Scity = value; }
        }

        private string _Ecity;
        public string Ecity
        {
            get { return _Ecity; }
            set { _Ecity = value; }
        }

        private string _ParPrice;
        public string ParPrice
        {
            get { return _ParPrice; }
            set { _ParPrice = value; }
        }

        private decimal _FullPrice;
        public decimal FullPrice
        {
            get { return _FullPrice; }
            set { _FullPrice = value; }
        }
        #endregion Model
    }
}
