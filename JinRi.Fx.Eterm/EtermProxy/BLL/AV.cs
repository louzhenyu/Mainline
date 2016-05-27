using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetermEntity.Response;

namespace EtermProxy.BLL
{
    public class AV : Business<JetermEntity.Request.AV, CommandResult<JetermEntity.Response.AV>>
    {
        public AV(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：解析AV指令（即：【AV:{航班号}/{起飞日期}】指令）返回结果
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>解析结果</returns>
        public override CommandResult<JetermEntity.Response.AV> BusinessDispose(JetermEntity.Request.AV request)
        {
            JetermEntity.Parser.AV av = new JetermEntity.Parser.AV(this.config, this.OfficeNo);

            this.Cmd = av.ParseCmd(request);
            if (string.IsNullOrEmpty(this.Cmd))
            {
                return av.Response;
            }
            
            ExcuteCmd();

            return av.ParseCmdResult(this.CmdResult);
        }

        private void ExcuteCmd()
        {           
            this.CmdResult = system(this.Cmd); 
        }
    }
}
