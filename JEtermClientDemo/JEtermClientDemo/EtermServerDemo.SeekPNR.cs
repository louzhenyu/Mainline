using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetermEntity.Request;
using JetermEntity;
using JetermEntity.Response;
using JetermClient.BLL;

namespace JEtermClientDemo
{ 
    /// <summary>
    /// 新调用方式的Demo，即调用EtermServer方式的Demo
    /// </summary>    
    public partial class EtermServerDemo
    {
        // 返回结果：{"PassengerList":[{"name":"孙伟哲","idtype":0,"cardno":"NI150105198605034390","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HSB9K6","FlightList":[{"FlightNo":"CZ3723","Airline":"","Cabin":"Y","SCity":"CAN","ECity":"XMN","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1436828400000+0800)\/","ArrDate":"\/Date(1436832600000+0800)\/"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["SHA111"],"BigPNR":"MGMP4Z","Mobile":"18101810679","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":950.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1000.00}],"ResultBag":" 1.孙伟哲 HSB9K6                                                                  \r 2.  CZ3723 Y   TU14JUL  CANXMN HK1   0700 0810          E                     \r 3.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     \r     ABCDEFG                                                                   \r 4.TL/1224/04MAY/KHN117                                                        \r 5.SSR FOID CZ HK1 NI150105198605034390/P1                                     \r 6.SSR ADTK 1E BY SHA14MAY15/1124 OR CXL CZ BOOKING                            \r 7.OSI CZ CTCT18101810679                                                      \r 8.RMK TJ AUTH SHA111                                                          \r 9.RMK CA/MGMP4Z                                                               \r10.SHA243                                                                      \r\r\n\r\n>PAT:A                                                                         \r01 Y FARE:CNY950.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1000.00                   \r?SFC:01   ?SFN:01                                                              \r"}
        public static void EtermServerDemo_SeekPNR()
        {
            // 定义请求对象
            Command<JetermEntity.Request.SeekPNR> cmd = new Command<JetermEntity.Request.SeekPNR>();

            // 设置应用程序编号
            cmd.AppId = 100001;

            // 设置Office号
            // 可以设置，也可以不设置。根据业务需要来决定是否设置。
            cmd.officeNo = "SHA243";

            // 根据各自的业务需求，设置缓存返回结果时长
            cmd.CacheTime = EtermCommand.CacheTime.min30;            

            // 设置请求参数
            cmd.request = new JetermEntity.Request.SeekPNR();           
            cmd.request.Pnr = "HSB9K6";          
            cmd.request.PassengerType = EtermCommand.PassengerType.Adult;
            cmd.request.GetPrice = true;           

            // 定义EtermClient对象
            EtermClient client = new EtermClient();

            // 调用Invoke以处理业务
            CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd);

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.SeekPNR>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResultBag = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.InnerDetailedErrorMessage, string.IsNullOrWhiteSpace(cmdResultBag) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResultBag)));
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
