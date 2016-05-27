using EtermProxy.Utility;
using JetermEntity;
using JetermEntity.Request;
using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EtermProxy.BLL
{
    public class TicketInfoByF : Business<JetermEntity.Request.TicketInfoByF, CommandResult<JetermEntity.Response.TicketInfoByF>>
    {
        public TicketInfoByF(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：【DETR:TN/{票号},F】指令返回结果解析
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>解析结果</returns>
        public override CommandResult<JetermEntity.Response.TicketInfoByF> BusinessDispose(JetermEntity.Request.TicketInfoByF request)
        {
            JetermEntity.Parser.TicketInfoByF ticketInfoByF = new JetermEntity.Parser.TicketInfoByF(this.config, this.OfficeNo);
           
            this.Cmd = ticketInfoByF.ParseCmd(request);
            if (string.IsNullOrEmpty(this.Cmd))
            {
                return ticketInfoByF.Response;
            }

            ExcuteCmd();

            return ticketInfoByF.ParseCmdResult(this.CmdResult);
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
