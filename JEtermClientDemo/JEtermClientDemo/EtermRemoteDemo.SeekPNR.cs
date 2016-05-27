using JetermClient.BLL;
using JetermEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetermEntity.Response;

namespace JEtermClientDemo
{
    /// <summary>
    /// 旧调用方式的Demo
    /// </summary>
    public partial class EtermRemoteDemo
    {
        // 返回结果：{"PassengerList":[{"name":"干园","idtype":0,"cardno":"NI650121199412242866","PassType":0,"Ename":"","BirthDayString":"","ChildBirthDayDate":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"张杰","idtype":0,"cardno":"NI140525198401186312","PassType":0,"Ename":"","BirthDayString":"","ChildBirthDayDate":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HS8LF7","FlightList":[{"FlightNo":"MU5623","Airline":"","Cabin":"R","SCity":"PVG","ECity":"DLC","DepTerminal":"T1","ArrTerminal":"","DepDate":"\/Date(1432169100000+0800)\/","ArrDate":"\/Date(1432175400000+0800)\/"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"PKEVW0","Mobile":"13472634765","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":570.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":620.00}],"ResultBag":" 1.干园 2.张杰 HS8LF7   \r 3.  MU5623 R   TH21MAY  PVGDLC HK2   0845 1030          E T1-- \r     -CA-PKEVW0 \r 4.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG  \r     ABCDEFG\r 5.TL/1818/29APR/SHA888 \r 6.SSR FOID MU HK1 NI140525198401186312/P2  \r 7.SSR FOID MU HK1 NI650121199412242866/P1  \r 8.SSR FQTV MU HK1 PVGDLC 5623 R21MAY MU600287397934/P2 \r 9.SSR ADTK 1E BY SHA01MAY15/1718 OR CXL MU5623 R21MAY  \r10.OSI MU CTCT13472634765   \r11.RMK CA/PKEVW0                                                               +\r\n\u001e12.SHA243                                                                      -\r\n\u001e[price]>PAT:A  \r01 R FARE:CNY570.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:620.00 \r\u001eSFC:01   \u001eSFN:01   \r\u001e[eTerm:o72fe231]"}
        public static void EtermRemoteDemo_SeekPNR()
        {
            EtermClient client = new EtermClient();            
            //string str = client.Invoke(100001, "http://114.80.79.158:8084/HelloWorld.rem", "RTR/JQF0B7", EtermCommand.ServerSource.EtermRemote, TimeSpan.FromSeconds(5));
            string str = string.Empty;
            try
            {
                str = client.Invoke(100001, "http://114.80.79.158:8084/HelloWorld.rem", "RTR/HS8LF7", EtermCommand.ServerSource.EtermRemote, TimeSpan.FromSeconds(5));
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("JEtermClient抛出异常，异常信息为：{0}", ex.Message));
            }            

            JetermEntity.Parser.SeekPNR seekPNRParser = new JetermEntity.Parser.SeekPNR();
            CommandResult<JetermEntity.Response.SeekPNR> result = seekPNRParser.ParseCmdResult(str);

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.SeekPNR>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResultBag = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResultBag) ? string.Empty : string.Format("{0}RT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResultBag)));
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
