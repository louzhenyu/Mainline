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
    /// <summary>
    /// 如何调用大系统的Demo
    /// </summary>
    public partial class BigSystemDemo
    {
        // 不传Office号时，如何调大系统的Demo：

        // RTPAT信息返回结果：
        /*
1.DANAHER/MAE XIANG 2.PERRY/KYLIE HONGSHUN 3.PERRY/REBECCA SUE KXN58Y  
4.  CZ6670 Y   SU21JUN  CTUKWL UN3   1305 1450      E T2-- 
5.T BJS/PEK/T-65906699/BEIJING CHINA EXPRESS INTERNATIONAL AI  
6.T BJS/R SERVICE CO. LTD/WEI CHONG
7.T/APT
8.BA/GCP ALDY PAM TOT CNY  
9.SSR FOID CZ HK1 NI517925439/P2   
10.SSR FOID CZ HK1 NI518926188/P3   
11.SSR FOID CZ HK1 NI478536670/P1   
12.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508902/1/P3
13.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508901/1/P2
14.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508900/1/P1                       + 15.SSR TKTL CA SS/ PEK 1200/01JUN15                                            -16.OSI CA CTC CHELI 
17.OSI CZ CTCT18001367952   
18.OSI YY RLOC PEK1EJQPSW6  
19.OSI CZ LSH GCP020150511370151
20.PEK1E/KXN58Y/PEK587  
[price]>PAT:A  
01 Y FARE:CNY980.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1030.00
SFC:01    SFN:01   
RMK CA/NWEXF0
         */
        // 返回结果：
        // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"DANAHER/MAE XIANG","idtype":0,"cardno":"NI478536670","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/KYLIE HONGSHUN","idtype":0,"cardno":"NI517925439","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/REBECCA SUE","idtype":0,"cardno":"NI518926188","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KXN58Y","FlightList":[{"FlightNo":"CZ6670","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CTU","ECity":"KWL","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1434863100000+0800)\/","ArrDate":"\/Date(1434869400000+0800)\/","PNRState":"UN3"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"NWEXF0","Mobile":"18001367952","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":980.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1030.00}],"ResultBag":" 1.DANAHER/MAE XIANG 2.PERRY/KYLIE HONGSHUN 3.PERRY/REBECCA SUE KXN58Y  \r 4.  CZ6670 Y   SU21JUN  CTUKWL UN3   1305 1450      E T2-- \r 5.T BJS/PEK/T-65906699/BEIJING CHINA EXPRESS INTERNATIONAL AI  \r 6.T BJS/R SERVICE CO. LTD/WEI CHONG\r 7.T/APT\r 8.BA/GCP ALDY PAM TOT CNY  \r 9.SSR FOID CZ HK1 NI517925439/P2   \r10.SSR FOID CZ HK1 NI518926188/P3   \r11.SSR FOID CZ HK1 NI478536670/P1   \r12.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508902/1/P3\r13.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508901/1/P2\r14.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508900/1/P1                       + 15.SSR TKTL CA SS/ PEK 1200/01JUN15                                            -16.OSI CA CTC CHELI \r17.OSI CZ CTCT18001367952   \r18.OSI YY RLOC PEK1EJQPSW6  \r19.OSI CZ LSH GCP020150511370151\r20.PEK1E/KXN58Y/PEK587  \r [price]>PAT:A  \r01 Y FARE:CNY980.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1030.00\r SFC:01    SFN:01   \r  RMK CA/NWEXF0"},"reqtime":"\/Date(1434538394371+0800)\/","SaveTime":1800}
        
        public static void BigSystemDemo_SeekPNR1()
        {
            // SeekPNR请求对象
            Command<JetermEntity.Request.SeekPNR> cmd = new Command<JetermEntity.Request.SeekPNR>();

            // 设置应用程序编号
            cmd.AppId = 100203;

            // 根据各自的业务需求，设置缓存返回结果时长
            cmd.CacheTime = EtermCommand.CacheTime.none;

            // 设置SeekPNR请求参数
            cmd.request = new JetermEntity.Request.SeekPNR();
            cmd.request.Pnr = "KXN58Y";
            cmd.request.Airline = "CZ";           

            // 定义EtermClient对象
            EtermClient client = new EtermClient();
            // 调用大系统以处理业务
            CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd, true);            

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.SeekPNR>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResultBag = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.InnerDetailedErrorMessage, string.IsNullOrWhiteSpace(cmdResultBag) ? string.Empty : string.Format("{0}RTPAT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResultBag)));
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

        // 传属于非平台的Office号时，如何调大系统的Demo：
        public static void BigSystemDemo_SeekPNR2()
        {
            // SeekPNR请求对象
            Command<JetermEntity.Request.SeekPNR> cmd = new Command<JetermEntity.Request.SeekPNR>();

            // 设置应用程序编号
            cmd.AppId = 100203;

            // 根据各自的业务需求，设置缓存返回结果时长
            cmd.CacheTime = EtermCommand.CacheTime.none;

            // 设置SeekPNR请求参数
            cmd.request = new JetermEntity.Request.SeekPNR();
            // 返回结果：
            // 此记录第1段航班信息的编号为“HX”状态不是有效记录编号，请检查您的PNR记录编号
            cmd.officeNo = "CAN378";
            cmd.request.Pnr = "HFQB18";
            cmd.request.Airline = "CZ";

            // 返回结果：指令返回为空
            //cmd.officeNo = "CAN378";
            //cmd.request.Pnr = "HFQB18";

            // 定义EtermClient对象
            EtermClient client = new EtermClient();
            // 调用大系统以处理业务
            CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd);

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.SeekPNR>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResultBag = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.InnerDetailedErrorMessage, string.IsNullOrWhiteSpace(cmdResultBag) ? string.Empty : string.Format("{0}RTPAT指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResultBag)));
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
