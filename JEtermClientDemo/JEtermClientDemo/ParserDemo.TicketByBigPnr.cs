using JetermEntity;
using JetermEntity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JEtermClientDemo
{
    public partial class ParserDemo
    {
        /// <summary>
        /// 解析工具中的ParseCmd方法使用Demo
        /// </summary>
        /// <remarks>
        /// 返回结果：DETR:CN/NK9Y8G,C
        /// </remarks>
        public static void ParserDemo_ParseCmd_TicketByBigPnr()
        {
            JetermEntity.Request.TicketByBigPnr request = new JetermEntity.Request.TicketByBigPnr();
            request.BigPnr = "NK9Y8G";
            request.FlightNo = "CZ6178";
            request.SCity = "CGQ";
            request.ECity = "HFE";

            JetermEntity.Parser.TicketByBigPnr ticketByBigPnrParser = new JetermEntity.Parser.TicketByBigPnr();
            string cmd = ticketByBigPnrParser.ParseCmd(request);

            Console.WriteLine("获得的TicketByBigPnr指令为：" + Environment.NewLine + cmd);
            Console.ReadLine();
        }

        /// <summary>
        /// 解析工具中的ParseCmdResult方法使用Demo
        /// </summary>
        /// <remarks>
        /// 解析第1种TicketByBigPnr指令返回结果。
        /// 返回结果：{"PassengerList":[{"name":"唐双林","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"3242356682726"}],"FlightList":[{"FlightNo":"SC4729","Airline":"","Cabin":"","SCity":"TAO","ECity":"NKG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1418227200000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/"}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":5}
        /// </remarks>
        public static void ParserDemo_ParseCmdResult_TicketByBigPnr1()
        {
            string cmdResult =
@"
■DETR:CN/NE8SJT,C
DETR:CN/NE8SJT,C                                                              
DETR:TN/784-2158602564 	             NAME: 张细志                                
    FOID:RP4670443338                       CZ3461 /26DEC14/CSXCTU FLOW        
DETR:TN/324-2356682726 	             NAME: 唐双林                                
    FOID:RP6112532251                       SC4729 /11DEC14/TAONKG FLOW 
";

            JetermEntity.Parser.TicketByBigPnr ticketByBigPnrParser = new JetermEntity.Parser.TicketByBigPnr();
            // 设置查询条件：          
            ticketByBigPnrParser.FlightNo = "SC4729";
            ticketByBigPnrParser.SCity = "TAO";
            ticketByBigPnrParser.ECity = "NKG";
            CommandResult<JetermEntity.Response.TicketByBigPnr> response = ticketByBigPnrParser.ParseCmdResult(cmdResult);

            if (response == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!response.state)
            {
                string cmdResult2 = response.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, response.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}TicketByBigPnr指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
       
        /// <summary>
        /// 解析工具中的ParseCmdResult方法使用Demo
        /// </summary>
        /// <remarks>
        /// 解析第2种TicketByBigPnr指令返回结果。
        /// 返回结果：
        /// 例子1：
        /// {"PassengerList":[{"name":"LIU/JOANNE","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7847589111741"}],"FlightList":null,"Price":{"FacePrice":246.00,"Tax":8.20,"Fuel":0.0,"TotalPrice":254.20},"TicketStatus":5}
        /// 例子2：
        /// {"PassengerList":[{"name":"茅威涛","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"8802323065499"}],"FlightList":null,"Price":{"FacePrice":760.00,"Tax":50.00,"Fuel":60.00,"TotalPrice":870.00},"TicketStatus":1}
        /// </remarks>
        public static void ParserDemo_ParseCmdResult_TicketByBigPnr2()
        {
            string cmdResult =
@"
?DETR:CN/NK9Y8G,C                                                              
ISSUED BY: SABRE                     ORG/DST: CGQ/HFE                 ARL-I    
E/R: Q/NONEND PENALTY APPLS                                                    
TOUR CODE:                                                                     
PASSENGER: LIU/JOANNE                                                          
EXCH:                               CONJ TKT:                                  
O FM:1CGQ CZ    6178  Y 16APR 1340 OK Y          13APR5/16APR6 20K USED/FLOWN   
          RL:NK9Y8G  /RCPJIY1S BG:1/28K BN:112                                 
  TO: HFE                                                                      
FC: CGQ CZ HFE1500CNY1500END                                                   
FARE:           CNY 1500.00|FOP:CC(VI4147XXXXXXXX5855 0417 00252C )            
EQUIV.FARE PD:  USD  246.00|                                                   +

TAX:            USD  8.20CN|OI:                                                -
TOTAL:          USD  254.20|TKTN: 784-7589111741 
";

            string cmdResult22 =
@"
■DETR:CN/PCZ0SX,C
DETR:CN/PCZ0SX,C                                                              
ISSUED BY: HAINAN AIRLINES           ORG/DST: SIA/NKG                 ARL-D    
E/R: 不得签转                                                                      
TOUR CODE:                                                                     
PASSENGER: 茅威涛                                                                 
EXCH:                               CONJ TKT:                                  
O FM:1XIY HU    7639  M 31DEC 1340 OK M                        20K OPEN FOR USE 
     T2-- RL:PCZ0SX  /                                                         
  TO: NKG                                                                      
FC: 31DEC14XIY HU NKG760.00CNY760.00END                                        
FARE:           CNY  760.00|FOP:CASH(CNY)                                      
TAX:            CNY 50.00CN|OI:                                                +

■pn
TAX:            CNY 60.00YQ|                                                   -
TOTAL:          CNY  870.00|TKTN: 880-2323065499
";

            JetermEntity.Parser.TicketByBigPnr ticketByBigPnrParser = new JetermEntity.Parser.TicketByBigPnr();
            // 设置查询条件：
            ticketByBigPnrParser.FlightNo = "CZ6178";
            ticketByBigPnrParser.SCity = "CGQ";
            ticketByBigPnrParser.ECity = "HFE";
            //ticketByBigPnrParser.FlightNo = "HU7639";
            //ticketByBigPnrParser.SCity = "XIY";
            //ticketByBigPnrParser.ECity = "NKG";
            CommandResult<JetermEntity.Response.TicketByBigPnr> response = ticketByBigPnrParser.ParseCmdResult(cmdResult);

            if (response == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!response.state)
            {
                string cmdResult2 = response.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, response.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}TicketByBigPnr指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
