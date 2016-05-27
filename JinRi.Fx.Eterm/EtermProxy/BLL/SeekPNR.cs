using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EtermProxy.Utility;
using System.Text.RegularExpressions;
using JetermEntity.Request;
using JetermEntity.Response;
using JetermEntity;
using JetermEntity.Parser;

namespace EtermProxy.BLL
{
    public class SeekPNR : Business<JetermEntity.Request.SeekPNR, CommandResult<JetermEntity.Response.SeekPNR>>
    {
        public SeekPNR(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：【提取编码RT】指令返回结果解析
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>返回结果</returns>
        public override CommandResult<JetermEntity.Response.SeekPNR> BusinessDispose(JetermEntity.Request.SeekPNR request)
        {
            JetermEntity.Parser.SeekPNR seekPNR = new JetermEntity.Parser.SeekPNR(this.config, this.OfficeNo);

            this.Cmd = seekPNR.ParseCmd(request);
            if (string.IsNullOrEmpty(this.Cmd))
            {
                return seekPNR.Response;
            }

            GetPriceCmd(request);
            ExcuteCmd();

            seekPNR.Response.error = ParserHelper.ValidRTResult(this.CmdResult, request.Pnr, request.PassengerType);
            if (seekPNR.Response.error != null)
            {
                seekPNR.Response.error.CmdResultBag = this.CmdResult;
                return seekPNR.Response;
            }

            try
            {
                seekPNR.Response = seekPNR.ParseCmdResult(this.CmdResult);
            }
            catch
            {
                seekPNR.Response.error = new Error(EtermCommand.ERROR.PARSE_FAIL);
                seekPNR.Response.error.CmdResultBag = this.CmdResult;
                return seekPNR.Response;
            }
            return seekPNR.Response;
        }

        #region Helper

        private string priceCmd = string.Empty;

        protected internal override void ExcuteCmd()
        {
            this.CmdResult = ExecuteRTCmd(this.Cmd);
            if (!string.IsNullOrWhiteSpace(priceCmd))
            {
                string getPriceCmdResult = system(priceCmd);
                this.CmdResult += string.Format("{0}{1}{2}", Environment.NewLine, Environment.NewLine, getPriceCmdResult);
            }
        }

        private void GetPriceCmd(JetermEntity.Request.SeekPNR request)
        {
            priceCmd = string.Empty;
            if (request.GetPrice)
            {
                switch (request.PassengerType)
                {
                    case EtermCommand.PassengerType.Adult:
                        priceCmd = "PAT:A";
                        break;
                    case EtermCommand.PassengerType.Children:
                        priceCmd = "PAT:A*CH";
                        break;
                    case EtermCommand.PassengerType.Baby:
                        priceCmd = "PAT:A*IN";
                        break;
                }
            }
        }

        #endregion
    }
}
