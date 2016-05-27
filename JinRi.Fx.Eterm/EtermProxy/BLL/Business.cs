using EtermProxy.Utility;
using JetermEntity;
using JetermEntity.Request;
using JetermEntity.Response;
using JetermEntity.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EtermProxy.BLL
{
    public abstract class Business<T, R>
        where T : new()
        where R : new()
    {

        public Business(IntPtr _hwnd, IntPtr _handle, string _config,string _officeNo)
        {
            hwnd = _hwnd;
            handle = _handle;
            config = _config;
            OfficeNo = _officeNo;
        }

        /// <summary>
        /// Eterm指针
        /// </summary>
        public IntPtr hwnd { get; set; }

        /// <summary>
        /// 信号句柄
        /// </summary>
        public IntPtr handle { get; set; }

        /// <summary>
        /// 配置号
        /// </summary>
        public string config { get; set; }

        /// <summary>
        /// 当前配置对应OfficeNo
        /// </summary>
        public string OfficeNo { get; set; }

        /// <summary>
        /// 返回错误信息标识常量
        /// </summary>
        protected internal const string ErrorState = "[ERROR]";

        /// <summary>
        /// Eterm指令
        /// </summary>
        protected internal string Cmd
        {
            get;
            set;
        }

        /// <summary>
        /// Eterm指令返回结果
        /// </summary>   
        protected internal string CmdResult
        {
            get;
            set;
        }

        /// <summary>
        /// Eterm调用方法
        /// </summary>
        /// <param name="cmd">Eterm指令</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>Eterm返回字符串</returns>
        /// <returns></returns>
        public string system(string cmd, uint timeout = 20*1000)
        {
#if DEBUG            
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://114.80.81.156:5888/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://192.168.2.224:15252/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://192.168.5.165:15252/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://120.132.136.92:5555/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://114.80.69.227:5555/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://192.168.5.213:15252/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);

            //return EtermProxy.Utility.HttpHelper.HttpPost("http://114.80.79.152:6666/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            return EtermProxy.Utility.HttpHelper.HttpPost("http://114.80.79.154:5555/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);       
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://120.132.136.91:18082/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);

            //return EtermProxy.Utility.HttpHelper.HttpPost("http://114.80.69.243:18082/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://114.80.79.158:9999/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://114.80.69.243:9999/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);

            //return EtermProxy.Utility.HttpHelper.HttpPost("http://120.132.136.91:18082/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://114.80.75.21:18082/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
            //return EtermProxy.Utility.HttpHelper.HttpPost("http://120.132.136.91:18083/", string.Format("system(\"{0}\");\r\nreturn DATA;", cmd), 5000);
#else
            lock (config)
            {
                EtermProxy.Utility.EtermHelper etermHelper = new EtermHelper();
                return etermHelper.system(hwnd, handle, config, cmd, timeout);
            }
#endif
        }

        /// <summary>
        /// 主方法：事务处理（每一个继承此类的类中必须重写此方法)  
        /// </summary>    
        /// <param name="request">请求对象</param>
        /// <returns>返回解析结果</returns>
        public virtual R BusinessDispose(T request)
        {
            return new R();
        }

        /// <summary>
        /// 执行RT指令
        /// </summary>
        /// <param name="rtCmd"></param>
        /// <returns></returns>
        protected internal string ExecuteRTCmd(string rtCmd)
        {
            string rtResult = system(rtCmd);
            if (Regex.IsMatch(rtResult, @"\s+\+\s*$"))
            {
                GetWholeEtermApiResult(ref rtResult);
            }

            return rtResult;
        }

        /// <summary>
        /// 获得全的Eterm命令返回结果
        /// </summary>
        /// <param name="etermApiResult"></param>       
        protected internal void GetWholeEtermApiResult(ref string etermApiResult)
        {
            if (!Regex.IsMatch(etermApiResult, @"\s+\+\s*$") && !Regex.IsMatch(etermApiResult, @"\s*\+\s*$"))
            {
                return;
            }       

            string nextResult = "\r\n" + system("PN");
            etermApiResult += nextResult;
            while (Regex.IsMatch(nextResult, @"\s+\+\s*$") || Regex.IsMatch(nextResult, @"\s*\+\s*$"))
            {
                nextResult = "\r\n" + system("PN");
                etermApiResult += nextResult;
            }
        }        

        /// <summary>
        /// 执行Eterm指令
        /// </summary> 
        /// <remarks>返回执行后的结果CmdResult</remarks>
        protected internal virtual void ExcuteCmd()
        {
            return;
        }
    }
}
