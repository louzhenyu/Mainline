using EtermProxy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetermEntity;
using JetermEntity.Request;
using JetermEntity.Response;
using JetermEntity.Parser;
using JetermClient.Utility;

namespace EtermProxy.BLL
{
    public class Booking : Business<JetermEntity.Request.Booking, CommandResult<JetermEntity.Response.Booking>>
    {
        private JetermEntity.Parser.Booking _booking = null;
        private string _key { get; set; }

        public Booking(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：【订位】指令返回结果解析
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>解析结果</returns>
        public override CommandResult<JetermEntity.Response.Booking> BusinessDispose(JetermEntity.Request.Booking request)
        {
            _booking = new JetermEntity.Parser.Booking(this.config, this.OfficeNo);

            try
            {
                this.Cmd = _booking.ParseCmd(request);
                if (string.IsNullOrEmpty(this.Cmd))
                {
                    return _booking.Response;
                }                

                ExcuteCmd();

                _booking.ParseCmdResult(this.CmdResult);
                if (_booking.Response.state)
                {
                    GetBigPnr(_booking.Response.result.Pnr);
                }
            }
            catch (Exception e)
            {
                _booking.Response.error = new Error(EtermCommand.ERROR.PARSE_FAIL);
                _booking.Response.error.CmdResultBag = this.CmdResult;
                _booking.Response.state = false;
                LogWrite.WriteLog(e);
            }

            return _booking.Response;
        }

        #region Helper

        protected internal override void ExcuteCmd()
        {
            this.Cmd = this.Cmd.Replace(Environment.NewLine, "[RN]");

            //取缓存，防止重复订位
            string cmd = new Regex("TKTL\\d{4}/\\d{2}[A-Z]{3}").Replace(this.Cmd,"");
            string key = string.Format("140106_140110_{0}", MD5Helper.GetMD5(cmd));
            LogWrite.WriteLog(string.Format("{0}\r\n{1}", this.Cmd, key));
            string bookCache = RedisHelper.stringGet(key);
            if (!string.IsNullOrEmpty(bookCache))
            {
                LogWrite.WriteLog("从Redis中获取到订位信息：\r\n" + bookCache);

                string pnr=string.Empty;
                ParserHelper.GetNewPnr(bookCache,ref pnr);                
                if (!string.IsNullOrEmpty(pnr))
                {                    
                    string rtdata = system(string.Format("RT {0}", pnr));
                    if (rtdata.IndexOf("CANCELLED") == -1)
                    {
                        //取PNR缓存
                        _key = string.Format("{0}_{1}", key, pnr);
                        string pnrCache = RedisHelper.stringGet(_key);

                        LogWrite.WriteLog("从Redis中获取到RT信息：\r\n" + pnrCache);

                        this.CmdResult = bookCache;
                        if (!string.IsNullOrEmpty(pnrCache))
                        {
                            _booking.Response.result.RTResultBag = pnrCache;
                        }
                        return;
                    }
                    else
                    {
                        LogWrite.WriteLog("此编码位置已取消：\r\n" + rtdata);
                        RedisHelper.delString(key);
                    }
                }
                else
                {
                    RedisHelper.delString(key);
                }
            }

            string iResult = string.Empty;
            int i = 0;
            do
            {                
                iResult = system("i");
                ++i;
            }while (string.IsNullOrWhiteSpace(iResult) && i < 4);
                        
            this.CmdResult = system(this.Cmd);           
        }

        /// <summary>
        /// 获得大编码号
        /// </summary>
        /// <param name="pnr"></param>      
        private void GetBigPnr(string pnr)
        {
            if (string.IsNullOrWhiteSpace(pnr))
            {
                return;
            }
            
            // Step1、获得RT命令结果：
            string rtCMDResult = string.Empty;
            if (!string.IsNullOrEmpty(_booking.Response.result.RTResultBag))
            {
                rtCMDResult = _booking.Response.result.RTResultBag;
            }
            else
            {
                rtCMDResult = system(string.Format("RT {0}", pnr));
                if (Regex.IsMatch(rtCMDResult, @"\s+\+\s*$"))
                {
                    GetWholeEtermApiResult(ref rtCMDResult);
                }

                // 获得RT指令返回结果：
                _booking.Response.result.RTResultBag = rtCMDResult;
            }
            rtCMDResult = Regex.Replace(rtCMDResult, @"\r|\n", string.Empty).Trim();
            rtCMDResult = rtCMDResult.Replace("<li>", string.Empty).Replace("</li>", string.Empty);

            // Step2、验证RT命令结果：
            if (rtCMDResult.Equals("err") || rtCMDResult.ToLower().Contains("error"))
            {
                return;
            }
            if (rtCMDResult.IndexOf("需要授权") != -1)
            {
                return;
            }
            if (rtCMDResult.ToUpper().Equals("NO PNR") || rtCMDResult.IndexOf(pnr) == -1)
            {
                return;
            }
            // 判断记录编号是否被擦除
            if (rtCMDResult.ToUpper().Contains("CANCELLED"))
            {
                return;
            }

            // Step3、获得大编码号
            _booking.Response.result.BigPNR = ParserHelper.GetBigPNR(rtCMDResult);

            if (!string.IsNullOrEmpty(_key))
            {
                RedisHelper.stringSet(_key, rtCMDResult, new TimeSpan(3,0,0));
            }
        }

        #endregion
    }
}
