using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 解析【DETR:TN/{票号},F】指令
    /// </summary>
    [Serializable]
    public class TicketInfoByF : ParserBase<JetermEntity.Request.TicketInfoByF, CommandResult<JetermEntity.Response.TicketInfoByF>>
    {
        #region 成员变量

        private JetermEntity.Request.TicketInfoByF _request = null;

        private CommandResult<JetermEntity.Response.TicketInfoByF> _response = null;

        /// <summary>
        /// 请求对象
        /// </summary>
        public override JetermEntity.Request.TicketInfoByF Request { get { return this._request; } }

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.TicketInfoByF> Response { get { return this._response; } }

        #endregion

        /// <summary>
        /// 构造【DETR:TN/{票号},F】指令返回对象
        /// </summary>
        public TicketInfoByF()
        {
            _response = new CommandResult<JetermEntity.Response.TicketInfoByF>();

            _response.result = new JetermEntity.Response.TicketInfoByF();
        }

        public TicketInfoByF(string config, string officeNo)
            : this()
        {
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回【DETR:TN/{票号},F】指令；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.TicketInfoByF request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            return string.Format("DETR:TN/{0},F", request.TicketNo);
        }

        /// <summary>
        /// 解析【DETR:TN/{票号},F】指令返回结果
        /// </summary>
        /// <param name="cmdResult">获取【DETR:TN/{票号},F】指令返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.TicketInfoByF> ParseCmdResult(string cmdResult)
        {
            if (!ValidCmdResult(cmdResult))
            {
                _response.error.CmdResultBag = cmdResult;
                return _response;
            }
          
            Regex reg = new Regex(@"NAME:(.*)TKTN:\s*(\S*)[\s\S]*");
            Match match = reg.Match(cmdResult);
            _response.result.PassengerName = (match.Groups[1].Value ?? string.Empty).Trim(); // 获得乘客名称
            _response.result.TicketNo = (match.Groups[2].Value ?? string.Empty).Replace("-", string.Empty).Trim(); // 获得票号/编码

            // 获得身份证号
            reg = new Regex(@"NI(\S+)");
            if (reg.IsMatch(cmdResult))
            {
                match = reg.Match(cmdResult);
                _response.result.PassengerCardNo = (match.Groups[1].Value ?? string.Empty).Trim(); 
            }            

            // 解析是否已经打印行程单
            _response.result.IsSchedule = false;
            reg = new Regex(@"1\s*RP\S+");
            if (reg.IsMatch(cmdResult))
            {
                _response.result.IsSchedule = true;
            }

            _response.state = true;
            return _response;
        }

        #region Helper
        
        protected internal override bool ValidRequest()
        {
            if (_request == null || string.IsNullOrWhiteSpace(_request.TicketNo))
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_PARAM);
                return false;
            }

            _request.TicketNo = Regex.Replace(_request.TicketNo, @"\s", string.Empty).Trim().ToUpper();

            return true;
        }

        protected internal override bool ValidCmdResult(string cmdResult)
        {
            if (cmdResult.Contains("NO RECORD"))
            {
                _response.error = new JetermEntity.Error(EtermCommand.ERROR.NO_RECORD);
                return false;
            }

            // 验证文本是否符合规范
            if (!ParserHelper.CheckData(cmdResult, new List<string>() { "NAME:", "TKTN:" }))
            {
                _response.error = new JetermEntity.Error(EtermCommand.ERROR.COMMAND_RESULT_FORMAT_INCORRECT);
                return false;
            }

            return true;
        }

        #endregion
    }
}
