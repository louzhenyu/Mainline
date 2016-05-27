using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EtermProxy.Utility;
using JetermEntity.Response;
using JetermEntity.Request;
using JetermEntity;

namespace EtermProxy.BLL
{
    public class TicketInfo : Business<JetermEntity.Request.TicketInfo, CommandResult<JetermEntity.Response.TicketInfo>>
    {
        public TicketInfo(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：获取票号信息指令（即：解析【DETR:TN/{票号}】指令）返回结果解析
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>解析结果</returns>
        public override CommandResult<JetermEntity.Response.TicketInfo> BusinessDispose(JetermEntity.Request.TicketInfo request)
        {
            JetermEntity.Parser.TicketInfo ticketInfo = new JetermEntity.Parser.TicketInfo(this.config, this.OfficeNo);

            this.Cmd = ticketInfo.ParseCmd(request);
            if (string.IsNullOrEmpty(this.Cmd))
            {
                return ticketInfo.Response;
            }

            ExcuteCmd();

            return ticketInfo.ParseCmdResult(this.CmdResult);
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
