using JetermEntity.Parser;
using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EtermProxy.BLL
{
    public class AVH : Business<JetermEntity.Request.AVH, CommandResult<JetermEntity.Response.AVH>>
    {
        public AVH(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：解析AVH指令（即：【AVH/{出发城市三字码}{到达城市三字码}/{起飞日期}/{航司}】指令）返回结果
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>解析结果</returns>
        public override CommandResult<JetermEntity.Response.AVH> BusinessDispose(JetermEntity.Request.AVH request)
        {
            JetermEntity.Parser.AVH avh = new JetermEntity.Parser.AVH(this.config, this.OfficeNo);

            this.Cmd = avh.ParseCmd(request);
            if (string.IsNullOrEmpty(this.Cmd))
            {
                return avh.Response;
            }

            string dtStr = ParserHelper.ConvertDateTimeToString(request.DepDate);
            ExcuteCmd(dtStr);

            return avh.ParseCmdResult(this.CmdResult);
        }

        private void ExcuteCmd(string dtStr)
        {
            string cmdResult = system(this.Cmd);
            GetWholeCmdResult(dtStr, ref cmdResult);
            this.CmdResult = cmdResult;
        }

        private void GetWholeCmdResult(string dtStrRequest, ref string cmdResult)
        {
            string nextResult = system("PN");

            // 格式如：09JUN16(
            string newDtStr1 = string.Format("{0}(", dtStrRequest);
            // 格式如：25AUG(
            string newDtStr2 = string.Format("{0}(", dtStrRequest.Substring(0, 5));
            
            if (nextResult.IndexOf(newDtStr1) < 0 && nextResult.IndexOf(newDtStr2) < 0)
            {
                return;
            }

            do
            {
                nextResult = string.Format("{0}{1}", Environment.NewLine, nextResult);
                cmdResult += nextResult;
                nextResult = system("PN");

            } while (nextResult.IndexOf(newDtStr1) > -1 || nextResult.IndexOf(newDtStr2) > -1);            
        }        
    }
}
