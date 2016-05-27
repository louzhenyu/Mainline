using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetermClient.DAL;
using JetermClient.Utility;
using System.Reflection;
using Newtonsoft.Json;
using JetermEntity;
using JetermEntity.Request;
using JetermEntity.Response;
using System.Diagnostics;
using JetermClient.Common;
using log4net;
using System.Text.RegularExpressions;

namespace JetermClient.BLL
{
    public class EtermClient
    {
        private static ILog log = LogManager.GetLogger(typeof(EtermClient));

        /// <summary>
        /// 通用,支持旧的方式,支持大系统调用 
        /// </summary>
        ///  <param name="appid">应用程序ID</param>
        /// <param name="url">请求url</param>
        /// <param name="cmd">请求POST数据</param>
        /// <param name="server">调用方式</param> 
        /// <param name="TimeOut">请求超时时间</param>
        /// <returns></returns>
        public string Invoke(int appid, string url, string cmd, EtermCommand.ServerSource server, TimeSpan TimeOut)
        {
            string.Format(JMetricsHelper.JetermCount, "Common").MeterMark("次");//计数

            return string.Format(JMetricsHelper.JetermExecTime, "Common").HistogramUpdate(() =>
            {
                string sret = string.Empty;
                int ntimeout = (int)TimeOut.TotalMilliseconds;

                DateTime dt = DateTime.Now;
                string dtStr = dt.ToString("yyyy-MM-dd HH:mm:ss ffff");

                try
                {
                    switch (server)
                    {
                        case EtermCommand.ServerSource.EtermServer:
                            sret = HttpService.HttpPost(url, cmd, ntimeout, dtStr);
                            break;
                        case EtermCommand.ServerSource.EtermRemote:
                            sret = HttpService.RemoteCmd(url, cmd, ntimeout, dtStr);
                            break;
                        case EtermCommand.ServerSource.BigSystem:
                            sret = HttpService.HttpGet(url, ntimeout, dtStr);
                            break;
                    }

                    log.Info(string.Format("JetermClient.Common请求：{0}请求方式：{1}{0}请求时间：{2}{0}请求url:{0}{3}{0}应用程序ID[{4}]{0}请求数据:{0}{5}{0}返回：{0}{6}。", Environment.NewLine, server.ToString(), dtStr, url, appid, cmd, (string.IsNullOrWhiteSpace(sret) ? "返回为空" : sret)));
                }
                catch (Exception ex)
                {
                    string.Format(JMetricsHelper.JetermErrCount, "Common").MeterMark("次");//失败计数

                    string innerErrorMessage = string.Format("JetermClient.Common请求抛异常：{0}请求方式：{1}{0}请求时间：{2}{0}请求url:{0}{3}{0}应用程序ID[{4}]{0}请求数据:{0}{5}{0}返回：{0}{6}{0}异常信息为：{0}【{0}{7}{0}】。", Environment.NewLine, server.ToString(), dtStr, url, appid, cmd, (string.IsNullOrWhiteSpace(sret) ? "返回为空" : sret), ex.ToString());
                    log.Error(innerErrorMessage);
                    throw new Exception(innerErrorMessage);
                }

                return sret;
            });
        }

        private string GetParam(ref string url, string name, bool breplace)
        {
            Match match = new Regex("(^|&|/)" + name + "=.*?(&|$)").Match(url);
            if (match.Success)
            {
                string sret = new Regex(name + "=|&|/").Replace(match.Value, "");
                if (breplace) url = new Regex(name + "=.*?(&|$|/)").Replace(url, "");
                return sret;
            }
            return "";
        }

        /// <summary>
        /// EtermServer调度，自由调度算法
        /// </summary>
        /// <param name="appid">应用程序ID</param>
        /// <param name="url">指令名</param>
        /// <param name="cmd">请求POST数据或脚本</param>
        /// <param name="TimeOut">请求超时时间</param>
        /// <returns></returns>
        public string Invoke(int appid, string url, string cmd, TimeSpan TimeOut)
        {
            string.Format(JMetricsHelper.JetermCount, "Common").MeterMark("次");//计数

            return string.Format(JMetricsHelper.JetermExecTime, "Common").HistogramUpdate(() =>
            {
                string sret = string.Empty;
                string server = string.Empty;
                string dtStr = string.Empty;
                string finalUrl = string.Empty;
                try
                {
                    int ntimeout = (int)TimeOut.TotalMilliseconds;

                    DateTime dt = DateTime.Now;
                    dtStr = dt.ToString("yyyy-MM-dd HH:mm:ss ffff");

                    List<Config> list = RedisHelper.tGet<List<Config>>("140106_140110_EtermUrl");
                    if (list != null)
                    {
                        list.RemoveAll(l => l == null);
                    }
                    if (list == null || list.Count == 0)
                    {
                        list = new JetermClient.DAL.EtermConfig().GetConfigs();
                        if (list != null && list.Count > 0)
                        {
                            RedisHelper.tSet("140106_140110_EtermUrl", list, TimeSpan.FromDays(30));
                        }
                    }

                    if (list == null || list.Count == 0) return string.Empty;

                    string method = GetParam(ref url, "method", true);
                    string officeno = GetParam(ref url, "officeno", true);
                    IEnumerable<Config> configs = list.ToArray();
                    if (!string.IsNullOrEmpty(method))
                        configs = from l in configs where l.cmdType.Contains((EtermCommand.CmdType)Enum.Parse(typeof(EtermCommand.CmdType), method)) select l;
                    if (!string.IsNullOrEmpty(officeno))
                        configs = from l in configs where l.OfficeNo == officeno select l;

                    if (configs == null || configs.Count() == 0) return string.Empty;

                    server = ((Config)configs.ElementAt(new Random(DateTime.Now.GetHashCode()).Next(0, configs.Count()))).ServerUrl;

                    finalUrl = string.Format("http://{0}/{1}", server, url);
                    sret = HttpService.HttpPost(finalUrl, cmd, ntimeout, dtStr);

                    log.Info(string.Format("JetermClient.Common请求：{0}请求方式：InvokeEtermServer{0}请求时间：{1}{0}请求url:{0}{2}{0}应用程序ID[{3}]{0}请求数据:{0}{4}{0}返回：{0}{5}。", Environment.NewLine, dtStr, finalUrl, appid, cmd, (string.IsNullOrWhiteSpace(sret) ? "返回为空" : sret)));
                }
                catch (Exception ex)
                {
                    string.Format(JMetricsHelper.JetermErrCount, "Common").MeterMark("次");//失败计数

                    string innerErrorMessage = string.Format("JetermClient.Common请求抛异常：{0}请求方式：InvokeEtermServer{0}请求时间：{1}{0}请求url:{0}{2}{0}应用程序ID[{3}]{0}请求数据:{0}{4}{0}返回：{0}{5}{0}异常信息为：{0}【{0}{6}{0}】。", Environment.NewLine, dtStr, finalUrl, appid, cmd, (string.IsNullOrWhiteSpace(sret) ? "返回为空" : sret), ex.ToString());
                    log.Error(innerErrorMessage);
                    throw new Exception(innerErrorMessage);
                }

                return sret;
            });
        }

        /// <summary>
        /// Eterm请求接口 
        /// 或 
        /// 大系统调用接口（目前只支持RTPAT指令）
        /// </summary>
        /// <typeparam name="T">请求参数</typeparam>
        /// <typeparam name="R">返回结果</typeparam>
        /// <param name="request">请求参数</param>        
        /// <param name="bigSystem">是否走大系统（目前只支持RTPAT指令）：true-表示走大系统，false-表示走通用接口。默认值为false。</param>
        /// <returns></returns>
        public CommandResult<R> Invoke<T, R>
            (
            Command<T> request
            , bool bigSystem = false
            )
            where T : new()
            where R : new()
        {
            return this.InvokeOne<T, R>(request, 2, bigSystem);
        }

        private CommandResult<R> InvokeOne<T, R>
            (
            Command<T> request,
            int repeat,
            bool bigSystem = false
            )
            where T : new()
            where R : new()
        {
            string method = string.Empty;
            string server = string.Empty;
            CommandResult<R> result = null;
            if (request != null && request.request != null)
            {
                method = request.request.GetType().Name;
                if (method.Equals(EtermCommand.CmdType.SeekPNR.ToString()))
                {
                    // 解析RT指令时，若没传request.officeNo，但需要走大系统，则走大系统，不走通用调用方式
                    if (string.IsNullOrWhiteSpace(request.officeNo))
                    {
                        if (bigSystem)
                        {
                            Command<JetermEntity.Request.SeekPNR> seekPnrCommand = (request as Command<JetermEntity.Request.SeekPNR>);
                            CommandResult<JetermEntity.Response.SeekPNR> seekPNRResult = Invoke(request.AppId, seekPnrCommand, TimeSpan.FromSeconds(5));
                            result = seekPNRResult as CommandResult<R>;
                            return result;
                        }
                    }

                    server = ServerUrl(request);
                    // 若传的office号来自非平台的，则解析RT指令时，也走大系统，不走通用调用方式
                    if (string.IsNullOrWhiteSpace(server))
                    {
                        Command<JetermEntity.Request.SeekPNR> seekPnrCommand = (request as Command<JetermEntity.Request.SeekPNR>);
                        CommandResult<JetermEntity.Response.SeekPNR> seekPNRResult = Invoke(request.AppId, seekPnrCommand, TimeSpan.FromSeconds(5));
                        result = seekPNRResult as CommandResult<R>;
                        return result;
                    }
                }
            }

            int i = 1; // 第几遍执行
            if (repeat == 1)
            {
                i = 2;
            }

            method = request.request.GetType().Name;
            if (i == 1)
            {
                string.Format(JMetricsHelper.JetermCount, method).MeterMark("次");//计数
            }

            string innerErrorMessage = string.Empty;

            return string.Format(JMetricsHelper.JetermExecTime, method).HistogramUpdate(() =>
            {
                string strParams = string.Empty;
                string requestJson = string.Empty;
                string key = string.Empty;
                string url = string.Empty;
                string sret = string.Empty;
                DateTime dt = DateTime.Now;
                string dtStr = dt.ToString("yyyy-MM-dd HH:mm:ss ffff");

                try
                {
                    strParams = JsonConvert.SerializeObject(request.request);
                    requestJson = JsonConvert.SerializeObject(request);
                    key = string.Format("140106{0}", MD5Helper.GetMD5(strParams));

                    if (request.CacheTime > 0)
                    {
                        result = RedisHelper.tGet<CommandResult<R>>(key);
                        if (result != null)
                            if (DateTime.Now - result.reqtime > TimeSpan.FromSeconds((int)request.CacheTime)) result = null;
                    }
                    if (result == null)
                    {
                        server = ServerUrl(request);
                        dt = DateTime.Now;
                        dtStr = dt.ToString("yyyy-MM-dd HH:mm:ss ffff");
                        if (!string.IsNullOrEmpty(server))
                        {
                            url = string.Format("http://{0}/format=json&language=CSharp&method={1}{2}", server, method, string.IsNullOrEmpty(request.ConfigName) ? string.Empty : string.Format("&USING={0}", request.ConfigName));
                            sret = HttpService.HttpPost(url, strParams, (int)request.TimeOut.TotalMilliseconds, dtStr, method, i);
                            result = JsonConvert.DeserializeObject<CommandResult<R>>(sret);
                            if (result != null)
                            {
                                result.ServerUrl = url;
                            }
                            if (result != null && result.state && result.SaveTime > 0)
                            {
                                // 缓存
                                RedisHelper.tSet<CommandResult<R>>(key, result, TimeSpan.FromSeconds((int)result.SaveTime));
                            }
                            if (result != null && !result.state)
                            {
                                string.Format(JMetricsHelper.JetermErrCount, method).MeterMark("次");//失败计数

                                innerErrorMessage = string.Format("JetermClient.{0}第{1}次请求（返回的result不为null）：{2}返回给result.error.ErrorMessage的信息为：{3}{2}请求时间：{4}{2}请求EtermServer服务器地址：{2}{5}{2}返回给result的OfficeNo为：{6}{2}返回给result的ConfigName为：{7}{2}MD5请求[{8}]{2}请求数据:{2}{9}{2}返回给sret的信息为：{2}{10}。", method, i, Environment.NewLine, (result.error == null ? string.Empty : result.error.ErrorMessage), dtStr, server, result.OfficeNo, result.config, key, requestJson, sret);
                                if (result.error == null)
                                {
                                    result.error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
                                    result.error.ErrorMessage = innerErrorMessage;
                                }
                                result.error.InnerDetailedErrorMessage = innerErrorMessage;
                                log.Error(innerErrorMessage);
                            }
                            else if (result == null)
                            {
                                if (i != 1 || (method.Equals(EtermCommand.CmdType.AVH.ToString()) || method.Equals(EtermCommand.CmdType.AV.ToString())))
                                {
                                    string.Format(JMetricsHelper.JetermErrCount, method).MeterMark("次");//失败计数

                                    innerErrorMessage = string.Format("JetermClient.{0}请求（共请求了{1}次）：返回的result为null。{2}请求时间：{3}{2}请求EtermServer服务地址：{2}{4}{2}MD5请求[{5}]{2}请求数据:{2}{6}{2}返回给sret的信息为：{2}{7}。", method, i, Environment.NewLine, dtStr, url, key, requestJson, (string.IsNullOrWhiteSpace(sret) ? "sret返回为空" : sret));
                                    result = GetError<T, R>(request, innerErrorMessage, url);
                                }
                                log.Error(string.Format("JetermClient.{0}第{1}次请求（返回的result为null）：{2}请求时间：{3}{2}请求EtermServer服务器地址：{2}{4}{2}MD5请求[{5}]{2}请求数据:{2}{6}{2}返回给sret的信息为：{2}{7}。", method, i, Environment.NewLine, dtStr, server, key, requestJson, (string.IsNullOrWhiteSpace(sret) ? string.Format("JetermClient.{0}请求的sret返回为空", method) : sret)));
                            }
                        }
                        else
                        {
                            if (result == null && (i != 1 || (method.Equals(EtermCommand.CmdType.AVH.ToString()) || method.Equals(EtermCommand.CmdType.AV.ToString()))))
                            {
                                string.Format(JMetricsHelper.JetermErrCount, method).MeterMark("次");//失败计数

                                innerErrorMessage = string.Format("JetermClient.{0}请求（共请求了{1}次）：没发现可用配置。{2}请求时间：{3}{2}MD5请求[{4}]{2}请求数据:{2}{5}。", method, i, Environment.NewLine, dtStr, key, requestJson);
                                result = GetError<T, R>(request, innerErrorMessage);
                            }
                            log.Error(string.Format("JetermClient.{0}第{1}次请求：{2}请求时间：{3}{2}MD5请求[{4}]{2}请求数据:{2}{5}{2}返回：{2}{6}。", method, i, Environment.NewLine, dtStr, key, requestJson, string.Format("JetermClient.{0}请求-没发现可用配置", method)));
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!string.IsNullOrWhiteSpace(sret))
                    {
                        string.Format(JMetricsHelper.JetermErrCount, method).MeterMark("次");//失败计数

                        if (result == null)
                        {
                            innerErrorMessage = string.Format("JetermClient.{0}请求（共请求了{1}次）抛异常（返回的result为null）：{2}请求时间：{3}{2}请求EtermServer服务地址：{2}{4}{2}MD5请求[{5}]{2}请求数据:{2}{6}{2}返回给sret的信息为：{2}{7}{2}异常信息为：{2}{8}。", method, i, Environment.NewLine, dtStr, url, key, requestJson, sret, ex.ToString());
                            result = GetError<T, R>(request, innerErrorMessage, url);
                        }
                        else
                        {
                            result.state = false;
                            result.ServerUrl = url;
                            innerErrorMessage = string.Format("JetermClient.{0}请求（共请求了{1}次）抛异常（返回的result不为null）：{2}返回给result.error.ErrorMessage的信息为：{3}{2}请求时间：{4}{2}请求EtermServer服务地址：{2}{5}{2}返回给result的OfficeNo为：{6}{2}返回给result的ConfigName为：{7}{2}MD5请求[{8}]{2}请求数据:{2}{9}{2}返回给sret的信息为：{2}{10}{2}异常信息为：{2}{11}。", method, i, Environment.NewLine, (result.error == null ? string.Empty : result.error.ErrorMessage), dtStr, url, result.OfficeNo, result.config, key, requestJson, sret, ex.ToString());
                            if (result.error == null)
                            {
                                result.error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
                                result.error.ErrorMessage = innerErrorMessage;
                            }
                            result.error.InnerDetailedErrorMessage = innerErrorMessage;
                        }
                    }
                    else if (i != 1 || (method.Equals(EtermCommand.CmdType.AVH.ToString()) || method.Equals(EtermCommand.CmdType.AV.ToString())))
                    {
                        string.Format(JMetricsHelper.JetermErrCount, method).MeterMark("次");//失败计数

                        innerErrorMessage = string.Format("JetermClient.{0}请求（共请求了{1}次）抛异常：sret返回为空，而导致返回的result为null。{2}请求时间：{3}{2}请求EtermServer服务地址：{2}{4}{2}MD5请求[{5}]{2}请求数据:{2}{6}{2}异常信息为：{2}{7}。", method, i, Environment.NewLine, dtStr, url, key, requestJson, ex.ToString());
                        result = GetError<T, R>(request, innerErrorMessage, url);
                    }
                    log.Error(string.Format("JetermClient.{0}第{1}次请求抛异常：{2}请求时间：{3}{2}请求EtermServer服务器地址：{2}{4}{2}{5}MD5请求[{6}]{2}请求数据:{2}{7}{2}返回给sret的信息为：{2}{8}{2}异常信息为：{2}【{2}{9}{2}】。", method, i, Environment.NewLine, dtStr, server, (result == null ? string.Empty : string.Format("返回给result的OfficeNo为：{0}{1}返回给result的ConfigName为：{2}{1}", result.OfficeNo, Environment.NewLine, result.config)), key, requestJson, (string.IsNullOrWhiteSpace(sret) ? string.Format("JetermClient.{0}请求返回为空", method) : sret), ex.ToString()));
                }

                if (result == null && string.IsNullOrWhiteSpace(sret) && !method.Equals(EtermCommand.CmdType.AVH.ToString()) && !method.Equals(EtermCommand.CmdType.AV.ToString()) && repeat > 1) // 重复执行最多1遍
                {
                    --repeat;
                    result = InvokeOne<T, R>(request, repeat);
                }

                return result;
            });
        }

        /// <summary>
        /// 调用大系统（目前只限于RTPAT指令）
        /// </summary>
        /// <param name="appId">应用程序ID</param>
        /// <param name="request">请求参数</param>
        /// <param name="TimeOut"></param>
        /// <returns>返回对RT指令返回结果解析好的对象</returns>
        private CommandResult<JetermEntity.Response.SeekPNR> Invoke(int appId, Command<JetermEntity.Request.SeekPNR> request, TimeSpan TimeOut)
        {
            string.Format(JMetricsHelper.JetermCount, "RTPATCommon").MeterMark("次");// 计数

            DateTime dt;
            string dtStr = string.Empty;

            string innerErrorMessage = string.Empty;
            CommandResult<JetermEntity.Response.SeekPNR> result = null;

            return string.Format(JMetricsHelper.JetermExecTime, "RTPATCommon").HistogramUpdate(() =>
            {
                string method = string.Empty;

                string url = string.Empty;
                string sret = string.Empty;
                string requestJson = string.Empty;

                string paramsStr = string.Empty;
                string key = string.Empty;

                try
                {
                    method = request.request.GetType().Name;

                    requestJson = JsonConvert.SerializeObject(request);
                    paramsStr = JsonConvert.SerializeObject(request.request);
                    key = string.Format("140106{0}", MD5Helper.GetMD5(string.Format("RTPATCommon_{0}", paramsStr)));

                    if (request.CacheTime > 0)
                    {
                        result = RedisHelper.tGet<CommandResult<JetermEntity.Response.SeekPNR>>(key);
                        if (result != null && ((DateTime.Now - result.reqtime) > TimeSpan.FromSeconds((int)request.CacheTime)))
                        {
                            result = null;
                        }
                    }

                    if (result == null)
                    {
                        url = string.Format("http://114.80.75.26:12306/GetPnrInfo?pnr={0}&airline={1}&Type=RTPAT", request.request.Pnr, request.request.Airline);
                        int ntimeout = (int)TimeOut.TotalMilliseconds;
                        dt = DateTime.Now;
                        dtStr = dt.ToString("yyyy-MM-dd HH:mm:ss ffff");
                        sret = HttpService.HttpGet(url, ntimeout, dtStr);

                        JetermEntity.Parser.SeekPNR seekPNRParser = new JetermEntity.Parser.SeekPNR();
                        result = seekPNRParser.ParseCmdResult(sret);
                        if (result != null)
                        {
                            result.ServerUrl = url;
                        }
                        if (result != null && result.state && result.SaveTime > 0)
                        {
                            // 缓存
                            RedisHelper.tSet<CommandResult<JetermEntity.Response.SeekPNR>>(key, result, TimeSpan.FromSeconds((int)result.SaveTime));
                        }
                        if (result == null)
                        {
                            string.Format(JMetricsHelper.JetermErrCount, "RTPATCommon").MeterMark("次");//失败计数

                            innerErrorMessage = string.Format("JetermClient.RTPATCommon大系统请求：返回的result为null。{0}请求时间：{1}{0}请求url：{0}{2}{0}应用程序ID[{3}]{0}MD5请求[{4}]{0}请求数据:{0}{5}{0}RTPAT指令返回：{0}{6}。", Environment.NewLine, dtStr, url, appId, key, requestJson, (string.IsNullOrWhiteSpace(sret) ? "RTPAT指令返回为空" : sret));
                            result = GetError<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(request, innerErrorMessage, url);
                            log.Error(innerErrorMessage);

                            return result;
                        }
                        if (!result.state)
                        {
                            string.Format(JMetricsHelper.JetermErrCount, "RTPATCommon").MeterMark("次");//失败计数                            

                            innerErrorMessage = string.Format("JetermClient.RTPATCommon大系统请求（返回的result不为null）：{0}返回给result.error.ErrorMessage的信息为：{0}{1}{0}请求时间：{2}{0}请求url：{0}{3}{0}应用程序ID[{4}]{0}MD5请求[{5}]{0}请求数据:{0}{6}{0}RTPAT指令返回：{0}{7}。", Environment.NewLine, (result.error == null ? string.Empty : result.error.ErrorMessage), dtStr, url, appId, key, requestJson, (string.IsNullOrWhiteSpace(sret) ? "RTPAT指令返回为空" : sret));
                            if (result.error == null)
                            {
                                result.error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
                                result.error.ErrorMessage = innerErrorMessage;
                            }
                            result.error.InnerDetailedErrorMessage = innerErrorMessage;
                            log.Error(innerErrorMessage);

                            return result;
                        }
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    string.Format(JMetricsHelper.JetermErrCount, "RTPATCommon").MeterMark("次");//失败计数

                    dt = DateTime.Now;
                    dtStr = dt.ToString("yyyy-MM-dd HH:mm:ss ffff");
                    innerErrorMessage = string.Format("JetermClient.RTPATCommon大系统请求抛异常：{0}返回给result.error.ErrorMessage的信息为：{0}{1}{0}抛异常时间：{2}{0}请求url：{0}{3}{0}应用程序ID[{4}]{0}MD5请求[{5}]{0}请求数据:{0}{6}{0}RTPAT指令返回为：{0}{7}{0}异常信息为：{0}【{0}{8}{0}】。", Environment.NewLine, ((result == null || result.error == null) ? string.Empty : result.error.ErrorMessage), dtStr, url, appId, key, requestJson, (string.IsNullOrWhiteSpace(sret) ? "RTPAT指令返回为空" : sret), ex.ToString());
                    if (result == null)
                    {
                        result = GetError<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(request, innerErrorMessage, url);
                    }
                    else
                    {
                        result.state = false;
                        result.ServerUrl = url;
                        if (result.error == null)
                        {
                            result.error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
                            result.error.ErrorMessage = innerErrorMessage;
                        }
                        result.error.InnerDetailedErrorMessage = innerErrorMessage;
                    }
                    log.Error(innerErrorMessage);

                    return result;
                }
            });
        }

        /// <summary>
        /// 获取服务器地址
        /// </summary>
        /// <param name="request"></param>  
        /// <returns></returns>
        private string ServerUrl<T>(Command<T> request)
            where T : new()
        {
            List<Config> list = RedisHelper.tGet<List<Config>>("140106_140110_EtermUrl");
            if (list != null)
            {
                list.RemoveAll(l => l == null);
            }
            if (list == null || list.Count == 0)
            {
                list = new JetermClient.DAL.EtermConfig().GetConfigs();
                if (list != null && list.Count > 0)
                {
                    RedisHelper.tSet("140106_140110_EtermUrl", list, TimeSpan.FromDays(30));
                }
            }

            if (list == null || list.Count == 0) return string.Empty;

            string method = request.request.GetType().Name;

            IEnumerable<Config> configs = from l in list where l.cmdType.Contains((EtermCommand.CmdType)Enum.Parse(typeof(EtermCommand.CmdType), method)) select l;

            if (!string.IsNullOrEmpty(request.officeNo))
                configs = from l in configs where l.OfficeNo == request.officeNo select l;
            if (!string.IsNullOrEmpty(request.ConfigName))
                configs = from l in configs where l.ConfigList.Contains(request.ConfigName) select l;
            if (method.Equals(EtermCommand.CmdType.Booking.ToString()))
            {
                if (request.request != null)
                {
                    JetermEntity.Request.Booking booking = (request.request as JetermEntity.Request.Booking);
                    if (booking != null && booking.FlightList.Count > 0)
                    {
                        string FlightNo = booking.FlightList.FirstOrDefault().FlightNo;
                        if (!string.IsNullOrEmpty(FlightNo) && FlightNo.Length > 2)
                        {
                            configs = from l in configs where l.AllowAirLine.Contains(FlightNo.Substring(0, 2)) select l;
                        }
                    }
                }
            }
            else if (method.Equals(EtermCommand.CmdType.AV.ToString()))
            {
                if (request.request != null)
                {
                    JetermEntity.Request.AV av = (request.request as JetermEntity.Request.AV);
                    if (av != null)
                    {
                        if (!string.IsNullOrEmpty(av.FlightNo) && av.FlightNo.Length > 2)
                        {
                            configs = from l in configs where l.AllowAirLine.Contains(av.FlightNo.Substring(0, 2)) select l;
                        }
                    }
                }
            }
            else if (method.Equals(EtermCommand.CmdType.AVH.ToString()))
            {
                if (request.request != null)
                {
                    JetermEntity.Request.AVH avh = (request.request as JetermEntity.Request.AVH);
                    if (avh != null && !string.IsNullOrWhiteSpace(avh.Airline))
                    {
                        configs = from l in configs where l.AllowAirLine.Contains(avh.Airline) select l;
                    }
                }
            }

            if (configs == null || configs.Count() == 0) return string.Empty;

            return ((Config)configs.ElementAt(new Random(DateTime.Now.GetHashCode()).Next(0, configs.Count()))).ServerUrl;
        }

        private CommandResult<R> GetError<T, R>(Command<T> request, string errorMessage, string serviceUrl = "")
            where T : new()
            where R : new()
        {
            CommandResult<R> result = new CommandResult<R>();

            result.state = false;
            result.OfficeNo = request.officeNo;
            result.config = request.ConfigName;
            result.ServerUrl = serviceUrl;
            result.error = new Error(EtermCommand.ERROR.SELFDEFINE_ERROR_MESSAGE);
            result.error.ErrorMessage = errorMessage;
            result.error.InnerDetailedErrorMessage = errorMessage;

            return result;
        }
    }
}
