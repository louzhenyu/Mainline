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
    public class TicketByBigPnr : Business<JetermEntity.Request.TicketByBigPnr, CommandResult<JetermEntity.Response.TicketByBigPnr>>
    {
        public TicketByBigPnr(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        public override CommandResult<JetermEntity.Response.TicketByBigPnr> BusinessDispose(JetermEntity.Request.TicketByBigPnr request)
        {
            JetermEntity.Parser.TicketByBigPnr ticketByBigPnr = new JetermEntity.Parser.TicketByBigPnr(this.config, this.OfficeNo);

            this.Cmd = ticketByBigPnr.ParseCmd(request);
            if (string.IsNullOrEmpty(this.Cmd))
            {
                return ticketByBigPnr.Response;
            }

            ExcuteCmd();
         
            return ticketByBigPnr.ParseCmdResult(this.CmdResult);
        }

        protected internal override void ExcuteCmd()
        {
            string cmdResult = system(this.Cmd);
            if (Regex.IsMatch(cmdResult, @"\s+\+\s*$") || Regex.IsMatch(cmdResult, @"s*\+\s*$"))
            {
                GetWholeEtermApiResult(ref cmdResult);
            }

            this.CmdResult = cmdResult;
        }
    }
}
