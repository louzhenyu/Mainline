using EtermProxy.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetermEntity.Request;
using JetermEntity.Response;
using JetermEntity;

namespace EtermProxy.BLL
{
    public class TicketInfoByS : Business<JetermEntity.Request.TicketInfoByS, CommandResult<JetermEntity.Response.TicketInfoByS>>
    {
        public TicketInfoByS(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：【DETR:TN/{票号},S】指令返回结果解析
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>解析结果</returns>
        public override CommandResult<JetermEntity.Response.TicketInfoByS> BusinessDispose(JetermEntity.Request.TicketInfoByS request)
        {
            JetermEntity.Parser.TicketInfoByS ticketInfoByS = new JetermEntity.Parser.TicketInfoByS(this.config, this.OfficeNo);     

            this.Cmd = ticketInfoByS.ParseCmd(request);
            if (string.IsNullOrEmpty(this.Cmd))
            {
                return ticketInfoByS.Response;
            }

            ExcuteCmd();

            return ticketInfoByS.ParseCmdResult(this.CmdResult);
        }

        protected internal override void ExcuteCmd()
        {
            string cmdResult = system(this.Cmd);
            if (Regex.IsMatch(cmdResult, @"\s+\+\s*$") || Regex.IsMatch(cmdResult, @"\s*\+\s*$"))
            {
                GetWholeEtermApiResult(ref cmdResult);
            }

            this.CmdResult = cmdResult;          
        }
    }
}
