using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JetermEntity.Parser
{
    /// <summary>
    /// 解析擦编码XEPNR指令
    /// </summary>
    [Serializable]
    public class CancelPnr : ParserBase<JetermEntity.Request.CancelPnr, CommandResult<JetermEntity.Response.CancelPnr>>
    {
        #region 成员变量

        private JetermEntity.Request.CancelPnr _request = null;

        private CommandResult<JetermEntity.Response.CancelPnr> _response = null;

        /// <summary>
        /// 请求对象
        /// </summary>
        public override JetermEntity.Request.CancelPnr Request { get { return this._request; } }

        /// <summary>
        /// 返回对象
        /// </summary>
        public override CommandResult<JetermEntity.Response.CancelPnr> Response { get { return this._response; } }

        #endregion

        /// <summary>
        /// 构造擦编码XEPNR指令返回对象
        /// </summary>
        public CancelPnr()
        {
            _response = new CommandResult<JetermEntity.Response.CancelPnr>();

            _response.result = new JetermEntity.Response.CancelPnr();
        }

        public CancelPnr(string config, string officeNo)
            : this()
        {        
            _response.config = config;
            _response.OfficeNo = officeNo;
        }

        /// <summary>
        /// 解析请求对象
        /// </summary>
        /// <param name="request">请求对象</param>  
        /// <returns>若请求参数验证通过，则返回擦编码XEPNR指令；否则返回为空。</returns>
        public override string ParseCmd(JetermEntity.Request.CancelPnr request)
        {
            _request = request;
            if (!ValidRequest())
            {
                return string.Empty;
            }

            return "XEPNR\\";
        }

        /// <summary>
        /// 解析擦编码XEPNR指令返回结果
        /// </summary>
        /// <param name="cmdResult">擦编码XEPNR指令返回结果</param>
        /// <returns>解析结果对象</returns>
        public override CommandResult<JetermEntity.Response.CancelPnr> ParseCmdResult(string cmdResult)
        {
            //_response.result.IsSuccess = false;
            if (!string.IsNullOrWhiteSpace(cmdResult) && cmdResult.ToUpper().Contains("CANCELLED"))
            {
                _response.result.IsSuccess = true;
            }

            _response.state = true;

            return _response;
        }

        #region Helper        
    
        protected internal override bool ValidRequest()
        {
            if (_request == null || string.IsNullOrWhiteSpace(_request.Pnr))
            {
                _response.error = new Error(EtermCommand.ERROR.EMPTY_REQUEST_PARAM);
                return false;
            }

            _request.Pnr = Regex.Replace(_request.Pnr, @"\s", string.Empty).Trim().ToUpper();

            return true;
        }

        #endregion
    }
}
