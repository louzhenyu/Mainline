using JetermClient.BLL;
using JetermEntity;
using JetermEntity.Request;
using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JEtermClientDemo
{
    public partial class EtermServerDemo
    {
        // 返回结果：{"PassengerList":[{"name":"邹丽媛","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812192001934"}],"FlightList":[{"FlightNo":"MU5522","Airline":"MU","Cabin":"R","SCity":"MDG","ECity":"TAO","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1444377600000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":1220.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1320.00},"TicketStatus":1}
        public static void EtermServerDemo_TicketByBigPnr()
        {
            // 定义请求对象
            Command<JetermEntity.Request.TicketByBigPnr> cmd = new Command<JetermEntity.Request.TicketByBigPnr>();

            // 设置应用程序编号
            cmd.AppId = 100001;

            // 根据各自的业务需求，设置缓存返回结果时长
            cmd.CacheTime = EtermCommand.CacheTime.min30;          

            cmd.request = new JetermEntity.Request.TicketByBigPnr();

            // 设置请求参数
            cmd.request.BigPnr = "PG5ZRE";
            cmd.request.FlightNo = "MU5522";
            cmd.request.SCity = "MDG";
            cmd.request.ECity = "TAO";

            // 定义EtermClient对象
            EtermClient client = new EtermClient();

            // 调用Invoke以处理业务
            CommandResult<JetermEntity.Response.TicketByBigPnr> result = client.Invoke<JetermEntity.Request.TicketByBigPnr, JetermEntity.Response.TicketByBigPnr>(cmd);
            
            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.TicketByBigPnr>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.InnerDetailedErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}TicketByBigPnr指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                Console.ReadLine();
                return;
            }

            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }            
            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();    
        }
    }
}
