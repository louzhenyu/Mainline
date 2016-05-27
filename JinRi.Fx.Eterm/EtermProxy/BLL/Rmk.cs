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
    public class Rmk : Business<JetermEntity.Request.Rmk, CommandResult<JetermEntity.Response.Rmk>>
    {
        private JetermEntity.Parser.Rmk _rmk = null;

        public Rmk(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：授权指令（即：解析【RMK TJ AUTH】指令）返回结果解析
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override CommandResult<JetermEntity.Response.Rmk> BusinessDispose(JetermEntity.Request.Rmk request)
        {
            _rmk = new JetermEntity.Parser.Rmk(this.config, this.OfficeNo);

            // 1、验证请求参数
            this.Cmd = _rmk.ParseCmd(request);
            if (string.IsNullOrEmpty(this.Cmd))
            {
                return _rmk.Response;
            }

            // 2、分别执行RT指令和授权RMK TJ AUTH指令
            GetRTCmd(request);
            ExcuteCmd();

            rtResult = Regex.Replace(rtResult, @"\r|\n", string.Empty).Trim();
            rtResult = rtResult.Replace("<li>", string.Empty).Replace("</li>", string.Empty);

            // 3、验证RT指令返回结果：          
            _rmk.Response.error = ParserHelper.ValidRTResult(rtResult, request.Pnr);
            if (_rmk.Response.error != null)
            {
                _rmk.Response.error.CmdResultBag = resultBag;
                return _rmk.Response;
            }

            // 4、解析授权指令返回结果
            _rmk.Response = _rmk.ParseCmdResult(this.CmdResult);
            ReSetIsSuccess(request);
            if (!_rmk.Response.state)
            {
                _rmk.Response.error.CmdResultBag = resultBag;
            }

            return _rmk.Response;
        }

        #region Helper

        private string rtCmd = string.Empty;
        private string rtResult = string.Empty;
        private string resultBag = string.Empty;

        protected internal override void ExcuteCmd()
        {
            rtResult = ExecuteRTCmd(rtCmd);
            resultBag = rtResult;

            this.CmdResult = system(this.Cmd);
            resultBag += string.Format("{0}{1}{2}", Environment.NewLine, Environment.NewLine, this.CmdResult);
        }

        private void GetRTCmd(JetermEntity.Request.Rmk request)
        {
            rtCmd = string.Format("RT {0}", request.Pnr);
        }

        private void ReSetIsSuccess(JetermEntity.Request.Rmk request)
        {
            if (!_rmk.Response.state || !_rmk.Response.result.IsSuccess)
            {
                return;
            }

            string newPnr = string.Empty;
            ParserHelper.GetNewPnr(this.CmdResult, ref newPnr);
            if (string.IsNullOrWhiteSpace(newPnr) || !newPnr.Equals(request.Pnr, StringComparison.CurrentCultureIgnoreCase))
            {
                _rmk.Response.result.IsSuccess = false;
            }
        }

        #endregion
    }
}
