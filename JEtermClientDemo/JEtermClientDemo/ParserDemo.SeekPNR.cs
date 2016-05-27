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
    /// 解析工具使用Demo
    /// </summary>
    public partial class ParserDemo
    {
        /// <summary>
        /// 解析工具中的ParseCmd方法使用Demo
        /// </summary>
        /// <remarks>
        /// 返回结果：RT JQF0B7
        /// </remarks>
        public static void ParserDemo_ParseCmd_SeekPNR()
        {
            JetermEntity.Request.SeekPNR request = new JetermEntity.Request.SeekPNR();
            request.Pnr = "JQF0B7";
            request.PassengerType = EtermCommand.PassengerType.Adult;
            request.GetPrice = true;

            JetermEntity.Parser.SeekPNR seekPNRParser = new JetermEntity.Parser.SeekPNR();
            string cmd = seekPNRParser.ParseCmd(request);

            Console.WriteLine("获得的RT指令为：" + Environment.NewLine + cmd);
            Console.ReadLine();
        }

        /// <summary>
        /// 解析工具中的ParseCmdResult方法使用Demo
        /// </summary>
        /// <remarks>
        /// 返回结果：{"PassengerList":[{"name":"张洋城","idtype":0,"cardno":"NI420700198103020999","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"JQF0B7","FlightList":[{"FlightNo":"CZ3691","Airline":"","Cabin":"Y","SCity":"KWE","ECity":"CAN","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1429245900000+0800)\/","ArrDate":"\/Date(1429251600000+0800)\/"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["KWE122"],"BigPNR":"PLG89Z","Mobile":"18210003200","OfficeNo":"TPE567","AdultPnr":null,"PriceList":[{"FacePrice":960.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1010.00}],"ResultBag":"  1.张洋城 JQF0B7\r             2.  CZ3691 Y   FR17APR  KWECAN HK1   1245 1420          E T2-- \r                 -CA-PLG89Z \r             3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG   \r             4.TL/1802/16APR/SHA888 \r             5.SSR FOID CZ HK1 NI420700198103020999/P1  \r             6.SSR ADTK 1E BY TPE16APR15/1902 OR CXL CZ BOOKING \r             7.OSI CZ CTCT18210003200   \r             8.RMK TJ AUTH KWE122   \r             9.RMK CA/PLG89Z\r            10.TPE567   \r            \u001e[price]>PAT:A  \r            01 Y FARE:CNY960.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1010.00\r            \u001eSFC:01   \u001eSFN:01   \r            \u001e[eTerm:o77a6491]"}
        /// </remarks>
        public static void ParserDemo_ParseCmdResult_SeekPNR()
        {
            string pnrText = @"  1.张洋城 JQF0B7             2.  CZ3691 Y   FR17APR  KWECAN HK1   1245 1420          E T2--                  -CA-PLG89Z              3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG                4.TL/1802/16APR/SHA888              5.SSR FOID CZ HK1 NI420700198103020999/P1               6.SSR ADTK 1E BY TPE16APR15/1902 OR CXL CZ BOOKING              7.OSI CZ CTCT18210003200                8.RMK TJ AUTH KWE122                9.RMK CA/PLG89Z            10.TPE567               [price]>PAT:A              01 Y FARE:CNY960.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1010.00            SFC:01   SFN:01               [eTerm:o77a6491]";

            JetermEntity.Parser.SeekPNR seekPNRParser = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> response = seekPNRParser.ParseCmdResult(pnrText);

            if (response == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!response.state)
            {
                string cmdResultBag = response.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, response.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResultBag) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResultBag)));
                Console.ReadLine();
                return;
            }

            if (response.result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(response.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();
        }
    }
}
