using JetermEntity;
using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EtermProxy.BLL
{
    public class GetPrice : Business<JetermEntity.Request.GetPrice, CommandResult<JetermEntity.Response.GetPrice>>
    {
        public GetPrice(IntPtr _hwnd, IntPtr _handle, string _config, string _officeNo)
            : base(_hwnd, _handle, _config, _officeNo)
        {

        }

        /// <summary>
        /// 主方法：【获取价格PAT】指令返回结果解析
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns>解析结果</returns>
        public override CommandResult<JetermEntity.Response.GetPrice> BusinessDispose(JetermEntity.Request.GetPrice request)
        {
            JetermEntity.Parser.GetPrice getPrice = new JetermEntity.Parser.GetPrice(this.config, this.OfficeNo);

            try
            {
                this.Cmd = getPrice.ParseCmd(request);
                if (string.IsNullOrEmpty(this.Cmd))
                {
                    return getPrice.Response;
                }

                ExcuteCmd();

                return getPrice.ParseCmdResult(this.CmdResult);                
            }
            catch
            {
                getPrice.Response.error = new Error(EtermCommand.ERROR.PARSE_PRICE_FAIL);
                getPrice.Response.error.CmdResultBag = this.CmdResult;
                return getPrice.Response;
            }
        }      

        protected internal override void ExcuteCmd()
        {
            this.CmdResult = system(this.Cmd);
        }
    }
}
