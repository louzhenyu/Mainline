using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 解析授权RMK TJ AUTH指令
    /// </summary>
    [Serializable]
    public class Rmk : ParserBase<JetermEntity.Request.Rmk, CommandResult<JetermEntity.Response.Rmk>>
    {
        #region 成员变量

        private JetermEntity.Request.Rmk _request = null;

        private CommandResult<JetermEntity.Response.Rmk> _response = null;

        /// <summary>
        /// 请求对象
        /// </summary>
        public override JetermEntity.Request.Rmk Request { get { return this._request; } }

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.Rmk> Response { get { return this._response; } }

        #endregion

        /// <summary>
        /// 构造授权RMK TJ AUTH指令返回对象
        /// </summary>
        public Rmk()
        {
            _response = new CommandResult<JetermEntity.Response.Rmk>();

            _response.result = new JetermEntity.Response.Rmk();
        }

        public Rmk(string config, string officeNo)
            : this()
        {
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回授权RMK TJ AUTH指令；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.Rmk request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            string rmkAuthCommand = string.Empty;
            for (int i = 0; i < request.RmkOfficeNoList.Count; ++i)
            {
                string rmkOfficeNo = request.RmkOfficeNoList[i].Trim();
                if (string.IsNullOrWhiteSpace(rmkOfficeNo))
                {
                    continue;
                }

                rmkOfficeNo = Regex.Replace(rmkOfficeNo, @"\s", string.Empty).ToUpper();
                if (i != request.RmkOfficeNoList.Count - 1)
                {
                    rmkAuthCommand += string.Format("RMK TJ AUTH {0}[RN]", rmkOfficeNo);
                    continue;
                }
                rmkAuthCommand += string.Format("RMK TJ AUTH {0}[RN]\\", rmkOfficeNo);
            }

            return rmkAuthCommand;
        }

        /// <summary>
        /// 解析授权RMK TJ AUTH指令返回结果
        /// </summary>
        /// <param name="cmdResult">授权RMK TJ AUTH指令返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.Rmk> ParseCmdResult(string cmdResult)
        {
            //_response.result.IsSuccess = false;

            if (!ValidCmdResult(cmdResult))
            {
                _response.error.CmdResultBag = cmdResult;
                return _response;
            }

            string regResult = @"\s*HK\d{1,}\s|\s*DK\d{1,}\s|\s*KK\d{1,}\s|\s*HN\d{1,}\s|\s*HL\d{1,}\s|\s*DW\d{1,}\s|\s*KL\d{1,}\s|\s*NN\d{1,}\s";
            string newPnr = string.Empty;
            if (Regex.IsMatch(cmdResult, regResult))
            {
                _response.result.IsSuccess = IsSuccessRMK(cmdResult, ref newPnr);
            }

            _response.state = true;
            return _response;
        }

        #region Helper
        
        protected internal override bool ValidRequest()
        {
            if (_request == null
                || string.IsNullOrWhiteSpace(_request.Pnr)
                || (_request.RmkOfficeNoList == null || !_request.RmkOfficeNoList.Any())
               )
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_PARAM);
                return false;
            }

            _request.Pnr = Regex.Replace(_request.Pnr, @"\s", string.Empty).Trim().ToUpper();

            return true;
        }

        protected internal override bool ValidCmdResult(string cmdResult)
        {
            if (cmdResult.Equals("err")
                || cmdResult.ToLower().Contains("error"))
            {
                _response.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
                return false;
            }

            if (Regex.IsMatch(cmdResult, @"\d{1,}\.")
                || cmdResult.Contains("UNABLE TO")
                || !Regex.IsMatch(cmdResult, @"[A-Za-z0-9]{1,}"))
            {
                _response.error = new Error(EtermCommand.ERROR.RMK_FAIL);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否授权成功
        /// </summary>
        /// <param name="cmdResult">授权命令返回结果</param>     
        /// <param name="newPnr"></param>
        /// <returns></returns>
        private bool IsSuccessRMK(string cmdResult, ref string newPnr)
        {
            string pnr = string.Empty;
            return ParserHelper.GetNewPnr(cmdResult, ref newPnr);
        }

        #endregion
    }
}
