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
    public class CancelPnr : Business<JetermEntity.Request.CancelPnr, CommandResult<JetermEntity.Response.CancelPnr>>
    {
        public CancelPnr(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：插编码指令（即：解析【XEPNR\】指令）返回结果解析
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public override CommandResult<JetermEntity.Response.CancelPnr> BusinessDispose(JetermEntity.Request.CancelPnr request)
        {
            JetermEntity.Parser.CancelPnr cancelPnr = new JetermEntity.Parser.CancelPnr(this.config, this.OfficeNo);        

            // 2、验证请求参数
            this.Cmd = cancelPnr.ParseCmd(request);
            if (string.IsNullOrEmpty(this.Cmd))
            {
                return cancelPnr.Response;
            }

            // 3、先后执行RT指令和擦编码XEPNR\指令：
            GetRTCmd(request);
            ExcuteCmd();

            rtResult = Regex.Replace(rtResult, @"\r|\n", string.Empty).Trim();
            rtResult = rtResult.Replace("<li>", string.Empty).Replace("</li>", string.Empty);

            // 4、验证RT指令返回结果：
            cancelPnr.Response.error = ParserHelper.ValidRTResult(rtResult, request.Pnr);
            if (cancelPnr.Response.error != null)
            {
                cancelPnr.Response.error.CmdResultBag = resultBag;
                return cancelPnr.Response;
            }

            // 如果request.CancelOut传的是false，则还需要从RT指令返回结果中解析出编码状态。
            // 当解析出的编码状态是以RR为开头的，则不允许插编码。
            if (!request.CancelOut)
            {
                string pnrState = string.Empty;

                MatchCollection mc = Regex.Matches(rtResult, @"\sRR[1-9](\d*)\s");
                if (mc != null && mc.Count > 0)
                {
                    pnrState = mc[0].Value.Trim();

                    // 根据编码状态，判断是否允许擦编码
                    if (!string.IsNullOrWhiteSpace(pnrState) && pnrState.StartsWith("RR"))
                    {
                        cancelPnr.Response.error = new Error(EtermCommand.ERROR.CANCEL_PNR_STATE_RR);
                        cancelPnr.Response.error.CmdResultBag = resultBag;
                        return cancelPnr.Response;
                    }
                }
            }

            // 5、解析【XEPNR\】指令返回结果
            return cancelPnr.ParseCmdResult(this.CmdResult);
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

        private void GetRTCmd(JetermEntity.Request.CancelPnr request)
        {
            rtCmd = string.Format("RT {0}", request.Pnr);
        }

        #endregion
    }
}
