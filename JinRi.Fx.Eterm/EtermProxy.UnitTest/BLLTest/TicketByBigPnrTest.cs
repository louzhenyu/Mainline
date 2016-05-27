using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JetermEntity.Request;
using JetermEntity.Response;
using JetermEntity;

namespace EtermProxy.UnitTest
{
    [TestClass]
    public class TicketByBigPnrTest
    {
        [TestMethod]
        public void Test_BusinessDispose()
        { 
            JetermEntity.Request.TicketByBigPnr request = new JetermEntity.Request.TicketByBigPnr();

            // 解析第1种返回结果的请求参数：
            //// 例3：
            //request.BigPnr = "NK9Y8G";
            //request.FlightNo = "MU5633";
            //request.SCity = "URC";
            //request.ECity = "KHG";
            ////request.FlightNo = "CA1303";
            ////request.SCity = "PEK";
            ////request.ECity = "SZX"; 
           
            // 例4：         
            //request.BigPnr = "PDNL7K";
            //request.FlightNo = "MF8108";
            //request.SCity = "PEK";
            //request.ECity = "FOC";

            // 例6：
            //PCZ0SX
            //request.BigPnr = "PCZ0SX";
            //request.FlightNo = "8L9801";
            //request.SCity = "KMG";
            ////request.SCity = "KMA";
            //request.ECity = "LJG";

            //// 解析第2种返回结果的请求参数：
            // 例1：
            //request.BigPnr = "PCZ0SX";
            //request.FlightNo = "HU7639";
            //request.SCity = "XIY";
            //request.ECity = "NKG";
         
            //request.BigPnr = "NK9Y8G";
            //request.FlightNo = "CA1254";
            //string flightCode = "URCPEK";
            //if (!string.IsNullOrEmpty(flightCode))
            //{
            //    if (flightCode.Length > 3)
            //    {
            //        request.SCity = flightCode.Substring(0, 3);
            //    }

            //    if (flightCode.Length > 5)
            //    {
            //        request.ECity = flightCode.Substring(3, 3);
            //    }
            //}

            // 测试是否能取到票号
            // 测试是否能成功换页
            // 返回结果：
            // {"PassengerList":[{"name":"许涛","idtype":0,"cardno":"330203197209250317","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707999"},{"name":"朱汉民","idtype":0,"cardno":"330227196812169018","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363708000"},{"name":"吴开封","idtype":0,"cardno":"330222197312078235","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707998"},{"name":"王勇","idtype":0,"cardno":"330203197201180318","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707997"},{"name":"李军","idtype":0,"cardno":"330204197412261034","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707996"},{"name":"曹敏君","idtype":0,"cardno":"330224197210074316","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707995"}],"FlightList":[{"FlightNo":"CA1765","Airline":"CA","Cabin":"","SubCabin":"","SCity":"HGH","ECity":"LHW","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1433433600000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":1}
            //request.BigPnr = "NHG4VK";
            //request.FlightNo = "CA1765";
            //request.SCity = "HGH";
            //request.ECity = "LHW";

            // 测试第1种情况的返回结果为什么只解析到了只有1个人的信息
            /*
黑屏返回结果：
?DETR:CN/NTX3P7,C                                                              
?DETR:TN/880-9289078641 ?             NAME: 刘建立                                
    FOID:PF18810983674                      HU7225 /20JUN15/PEKWEF OPEN        
?DETR:TN/880-9289078642 ?             NAME: 刘文玲                                
    FOID:PF18810983674                      HU7225 /20JUN15/PEKWEF OPEN 
             */
            //  返回结果：
            // {"PassengerList":[{"name":"刘建立","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"8809289078641"},{"name":"刘文玲","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"8809289078642"}],"FlightList":[{"FlightNo":"HU7225","Airline":"HU","Cabin":"","SubCabin":"","SCity":"PEK","ECity":"WEF","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1434729600000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":1}
            //request.BigPnr = "NTX3P7";
            //request.FlightNo = "HU7225";
            //request.SCity = "PEK";
            //request.ECity = "WEF";

            /*
黑屏返回结果：
?DETR:CN/NDYJVW,C                                                              
?DETR:TN/781-2191996969 ?             NAME: 肖苏城                                
    FOID:NI450221197705161938               MU5757 /20MAY15/DLUKMG FLOW        
    FOID:NI450221197705161938               MU5767 /20MAY15/KMGNNG FLOW        
?DETR:TN/781-2191996968 ?             NAME: 肖苏城                                
    FOID:NI450221197705161938               MU5768 /16MAY15/NNGKMG FLOW        
    FOID:NI450221197705161938                 VOID/VOID   /KMGDLU VOID         
?DETR:TN/781-2191996967 ?             NAME: 吴乐                                 
    FOID:NI452501197510263267               MU5757 /20MAY15/DLUKMG FLOW        
    FOID:NI452501197510263267               MU5767 /20MAY15/KMGNNG FLOW        
?DETR:TN/781-2191996966 ?             NAME: 吴乐                                 
    FOID:NI452501197510263267               MU5768 /16MAY15/NNGKMG FLOW        +

    FOID:NI452501197510263267                 VOID/VOID   /KMGDLU VOID        - 
             */
            // 返回结果：
            // {"PassengerList":[{"name":"肖苏城","idtype":0,"cardno":"450221197705161938","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812191996969"},{"name":"吴乐","idtype":0,"cardno":"452501197510263267","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812191996967"}],"FlightList":[{"FlightNo":"MU5767","Airline":"MU","Cabin":"","SubCabin":"","SCity":"KMG","ECity":"NNG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432051200000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":5}
            request.BigPnr = "NDYJVW";
            request.FlightNo = "MU5767";
            request.SCity = "KMG";
            request.ECity = "NNG";

            EtermProxy.BLL.TicketByBigPnr logic = new EtermProxy.BLL.TicketByBigPnr(IntPtr.Zero, IntPtr.Zero, "o72fe261", string.Empty);
            CommandResult<JetermEntity.Response.TicketByBigPnr> result = logic.BusinessDispose(request);

            if (result != null && result.state)
            {
                // 运行结果：
                // 第1种返回结果的解析结果：
                // 例3：
                // {"PassengerList":[{"name":"斯坎迪尔穆提拉","idtype":0,"cardno":"653121199401031919","PassType":-1,"Ename":"","BirthDayString":"","TicketNo":"7812180622791"}],"Price":{"FacePrice":0,"TotalPrice":0,"Tax":0,"Fuel":0}}
                // {"PassengerList":[{"name":"徐速","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BirthDayString":"","TicketNo":"9991952037851"}],"Price":{"FacePrice":0,"TotalPrice":0,"Tax":0,"Fuel":0}}
                // 例4：
                // {"PassengerList":[{"name":"陈国辉","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BirthDayString":"","TicketNo":"7312381150793"},{"name":"周井源","idtype":0,"cardno":"132622197911195210","PassType":-1,"Ename":"","BirthDayString":"","TicketNo":"7312381150794"}],"Price":{"FacePrice":0,"TotalPrice":0,"Tax":0,"Fuel":0}}
                // 第2种返回结果的解析结果：
                // 例1：
                // {"PassengerList":[{"name":"茅威涛","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BirthDayString":"","TicketNo":"8802323065499"}],"Price":{"FacePrice":760.00,"TotalPrice":870.00,"Tax":50.00,"Fuel":60.00}}
                // {"PassengerList":[{"name":"茅威涛","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BirthDayString":"","TicketNo":"8802323065499"}],"Price":{"FacePrice":0,"TotalPrice":0,"Tax":0,"Fuel":0}}
                string str = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
                Console.WriteLine("运行结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
                return;
            }
            if (result.error != null)
            {
                Console.WriteLine("运行错误，错误信息：" + result.error.ErrorMessage);
            }
        }

        /*
        指令返回结果：
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
         */
        [TestMethod]
        public void Test_TicketByBigPnr_Invoke1()
        {
            // TicketByBigPnr请求对象
            Command<JetermEntity.Request.TicketByBigPnr> cmd = new Command<JetermEntity.Request.TicketByBigPnr>();

            // 设置应用程序编号
            cmd.AppId = 900630;

            //// 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "SHA243";

            cmd.request = new JetermEntity.Request.TicketByBigPnr();

            #region TicketByBigPnr请求参数

            // 第2种格式的指令返回结果：
            // 只有1个航段+只有到达航站楼：
            // （已测）返回结果：
            // {"PassengerList":[{"name":"谢煜琳","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7842172138302"}],"FlightList":[{"FlightNo":"CZ6509","Airline":"CZ","Cabin":"V","SCity":"SHE","ECity":"PVG","DepTerminal":"","ArrTerminal":"T2","DepDate":"\/Date(1429872600000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/"}],"Price":{"FacePrice":630.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":680.00},"TicketStatus":5}
            //cmd.request.BigPnr = "PCZ0SX";
            //cmd.request.FlightNo = "CZ6509";
            //cmd.request.SCity = "SHE";
            //cmd.request.ECity = "PVG";

            // 只有1个航段+有出发和到达航站楼：
            // （已测）返回结果：
            // {"PassengerList":[{"name":"刘国平","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7312382483125"}],"FlightList":[{"FlightNo":"MF8154","Airline":"MF","Cabin":"T","SCity":"TYN","ECity":"XMN","DepTerminal":"T2","ArrTerminal":"T3","DepDate":"\/Date(1432869600000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/"}],"Price":{"FacePrice":650.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":700.00},"TicketStatus":1}
            //cmd.request.BigPnr = "NVF1WW";
            //cmd.request.FlightNo = "MF8154";
            //cmd.request.SCity = "TYN";
            //cmd.request.ECity = "XMN";

            // 只有1个航段+姓名是全英文的情况：
            // （已测）返回结果：
            // {"PassengerList":[{"name":"LIU/JOANNE","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7847589111741"}],"FlightList":[{"FlightNo":"CZ6178","Airline":"CZ","Cabin":"Y","SCity":"CGQ","ECity":"HFE","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1429162800000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/"}],"Price":{"FacePrice":246.00,"Tax":8.20,"Fuel":0.0,"TotalPrice":254.20},"TicketStatus":5}
            cmd.request.BigPnr = "NK9Y8G";
            cmd.request.FlightNo = "CZ6178";
            cmd.request.SCity = "CGQ";
            cmd.request.ECity = "HFE";

            // 有2个航段的情况：
            // （已测）（已测）求第1航段的航班信息+符合条件：
            // 返回结果：
            // {"PassengerList":[{"name":"邹丽媛","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812192001934"}],"FlightList":[{"FlightNo":"MU5511","Airline":"MU","Cabin":"R","SCity":"TAO","ECity":"MDG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1443762000000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/"}],"Price":{"FacePrice":1220.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1320.00},"TicketStatus":1}
            //cmd.request.BigPnr = "PG5ZRE";
            //cmd.request.FlightNo = "MU5511";
            //cmd.request.SCity = "TAO";
            //cmd.request.ECity = "MDG";

            // 求第1航段的航班信息+不符合条件：
            // （已测）返回结果：没有符合查询条件的记录
            //cmd.request.BigPnr = "PG5ZRE";
            //cmd.request.FlightNo = "MU5511";
            //cmd.request.SCity = "TAO";
            //cmd.request.ECity = "TAO";

            // 求第2航段的航班信息+符合条件：
            // （已测）（已测）返回结果：
            // {"PassengerList":[{"name":"邹丽媛","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812192001934"}],"FlightList":[{"FlightNo":"MU5522","Airline":"MU","Cabin":"R","SCity":"MDG","ECity":"TAO","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1444377600000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/"}],"Price":{"FacePrice":1220.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1320.00},"TicketStatus":1}
            //cmd.request.BigPnr = "PG5ZRE";
            //cmd.request.FlightNo = "MU5522";
            //cmd.request.SCity = "MDG";
            //cmd.request.ECity = "TAO";

            // 求第2航段的航班信息+不符合条件：
            // （已测）返回结果：没有符合查询条件的记录
            //cmd.request.BigPnr = "PG5ZRE";
            //cmd.request.FlightNo = "MU5522";
            //cmd.request.SCity = "MDG";
            //cmd.request.ECity = "SHA";

            // （未测）选个票号不存在的情况：


            // 第1种格式的指令返回结果：
            // （已测）一个人中有2个航程的情况：
            // 返回结果：
            // {"PassengerList":[{"name":"肖苏城","idtype":0,"cardno":"450221197705161938","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812191996969"},{"name":"吴乐","idtype":0,"cardno":"452501197510263267","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812191996967"}],"FlightList":[{"FlightNo":"MU5767","Airline":"","Cabin":"","SCity":"KMG","ECity":"NNG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432051200000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/"}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":1}
            //cmd.request.BigPnr = "NDYJVW";
            //cmd.request.FlightNo = "MU5767";
            //cmd.request.SCity = "KMG";
            //cmd.request.ECity = "NNG";

            // （已测）返回结果：
            // {"PassengerList":[{"name":"肖苏城","idtype":0,"cardno":"450221197705161938","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812191996968"},{"name":"吴乐","idtype":0,"cardno":"452501197510263267","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812191996966"}],"FlightList":null,"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":2}
            //cmd.request.BigPnr = "NDYJVW";
            //cmd.request.FlightNo = "VOID";
            //cmd.request.SCity = "KMG";
            //cmd.request.ECity = "DLU";

            // （已测）找不到结果
            //cmd.request.BigPnr = "NDYJVW";
            //cmd.request.FlightNo = "MU8888";
            //cmd.request.SCity = "KMG";
            //cmd.request.ECity = "DLU";

            // （已测）一个人中有1个航程的情况（票号已找不到）：
            // 返回结果：
            //cmd.request.BigPnr = "NE8SJT";
            //cmd.request.FlightNo = "MU5522";
            //cmd.request.SCity = "MDG";
            //cmd.request.ECity = "TAO";

            #endregion

            #region 调用Invoke以处理业务

            //EtermClient client = new EtermClient();
            //cmd.officeNo = "SHA243";
            //cmd.ConfigName = "O77124B1";
            //CommandResult<JetermEntity.Response.TicketByBigPnr> result = client.Invoke<JetermEntity.Request.TicketByBigPnr, JetermEntity.Response.TicketByBigPnr>(cmd);

            EtermProxy.BLL.TicketByBigPnr logic = new EtermProxy.BLL.TicketByBigPnr(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            logic.OfficeNo = "SHA243";
            logic.config = "O77124B1";
            CommandResult<JetermEntity.Response.TicketByBigPnr> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}TicketByBigPnr指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }

        [TestMethod]
        public void Test_TicketByBigPnr_ParseCmdResult()
        {
            // 返回结果：
            // {"PassengerList":[{"name":"唐双林","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"3242356682726"}],"FlightList":[{"FlightNo":"SC4729","Airline":"SC","Cabin":"","SCity":"TAO","ECity":"NKG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1418227200000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/"}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":5}
            string cmdResult =
            //string cmdResult1 =
@"
■DETR:CN/NE8SJT,C
DETR:CN/NE8SJT,C                                                              
DETR:TN/784-2158602564 	             NAME: 张细志                                
    FOID:RP4670443338                       CZ3461 /26DEC14/CSXCTU FLOW        
DETR:TN/324-2356682726 	             NAME: 唐双林                                
    FOID:RP6112532251                       SC4729 /11DEC14/TAONKG FLOW 
";

            string cmdResult22 =
            //string cmdResult =
@"
?DETR:CN/PLRGCW,C                                                              ?DETR:TN/784-2180237936 ?             NAME: 边惠敏                                    FOID:NI410104197704224520               CZ3479 /27MAY15/CGOCKG FLOW        ?DETR:TN/784-2180237938 ?             NAME: 李汶静                                    FOID:NI410303197402251026               CZ3479 /27MAY15/CGOCKG FLOW        ?DETR:TN/784-2180237937 ?             NAME: 海伟                                     FOID:NI410104197504041017               CZ3479 /27MAY15/CGOCKG FLOW 
";

            string cmdResult3 =
            //string cmdResult =
@"
▶DETR:CN/MEXE1E,C                                                              
▶DETR:TN/826-9288796767 ▪             NAME: 李石山                                
    FOID:PF13760312136                      GS7489 /02JUN15/URCKRL OPEN        
▶DETR:TN/781-2192090015 ▪             NAME: 胡骏                                 
    FOID:RP1826339188                       FM9422 /17MAY15/CKGSHA FLOW 
";

            JetermEntity.Parser.TicketByBigPnr ticketByBigPnrParser = new JetermEntity.Parser.TicketByBigPnr();
            // 设置查询条件：          
            ticketByBigPnrParser.FlightNo = "SC4729";
            ticketByBigPnrParser.SCity = "TAO";
            ticketByBigPnrParser.ECity = "NKG";
  
            //ticketByBigPnrParser.FlightNo = "CZ3479";
            //ticketByBigPnrParser.SCity = "CGO";
            //ticketByBigPnrParser.ECity = "CKG";

            // 测试TicketNo返回为?：
            // 返回结果：
            // {"PassengerList":[{"name":"李石山","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"8269288796767"}],"FlightList":[{"FlightNo":"GS7489","Airline":"GS","Cabin":"","SubCabin":"","SCity":"URC","ECity":"KRL","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1433174400000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":1}
            // {"PassengerList":[{"name":"李石山","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"8269288796767"}],"FlightList":[{"FlightNo":"GS7489","Airline":"GS","Cabin":"","SubCabin":"","SCity":"URC","ECity":"KRL","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1433174400000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":1}
            //ticketByBigPnrParser.FlightNo = "GS7489";
            //ticketByBigPnrParser.SCity = "URC";
            //ticketByBigPnrParser.ECity = "KRL";
            CommandResult<JetermEntity.Response.TicketByBigPnr> response = ticketByBigPnrParser.ParseCmdResult(cmdResult);

            if (response == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!response.state)
            {
                string cmdResult2 = response.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, response.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}TicketByBigPnr指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (response.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(response.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();
        }
        
        [TestMethod]
        public void Test_EtermProxy_1000()
        {
            //string strPost = "{\"ClassName\" : \"TicketByBigPnr\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            // 返回结果：
            // TicketNo的返回值没有返回?
            // {"state":true,"error":null,"config":"o72fe271","OfficeNo":"","result":{"PassengerList":[{"name":"边惠敏","idtype":0,"cardno":"410104197704224520","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7842180237936"},{"name":"李汶静","idtype":0,"cardno":"410303197402251026","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7842180237938"},{"name":"海伟","idtype":0,"cardno":"410104197504041017","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7842180237937"}],"FlightList":[{"FlightNo":"CZ3479","Airline":"CZ","Cabin":"","SubCabin":"","SCity":"CGO","ECity":"CKG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432656000000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":5},"reqtime":"\/Date(1432793408178+0800)\/","SaveTime":2592000}
            string strPost = "{\"ClassName\" : \"TicketByBigPnr\", \"Config\" : \"o72fe271\",  \"OfficeNo\" : \"\" }";
            //string ss = "{\"FlightList\":[{\"FlightNo\":\"MU5137\",\"Cabin\":\"H\",\"SCity\":\"SHA\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1430064000000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"MU5156\",\"Cabin\":\"B\",\"SCity\":\"PEK\",\"ECity\":\"SHA\",\"DepDate\":\"\\/Date(1430323200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"干园\",\"idtype\":0,\"cardno\":\"650121199412242866\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"},{\"name\":\"张杰\",\"idtype\":0,\"cardno\":\"140525198401186312\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"13472634765\",\"RMKOfficeNoList\":[],\"RMKRemark\":null,\"Pnr\":null}";
            //string ss = "{\"BigPnr\":\"NVF1WW\",\"FlightNo\":\"MF8154\",\"SCity\":\"TYN\",\"ECity\":\"XMN\"}";
            // 返回结果：{"state":false,"error":{"ErrorCode":70,"ErrorMessage":"票号不存在","CmdResultBag":"?DETR:CN/NVZSHM,C                                                              \rTICKET NOT FOUND                                                               \r                                                                               \r"},"config":"O77124B1","OfficeNo":"SHA243","result":{"PassengerList":null,"FlightList":null,"Price":null,"TicketStatus":0},"reqtime":"\/Date(1432553530423+0800)\/","SaveTime":1800}
            //string ss = "{\"BigPnr\":\"NVZSHM\",\"FlightNo\":\"MU5844\",\"SCity\":\"CTU\",\"ECity\":\"KMG\"}";
            // 测试TicketNo是否返回为?：
            // 返回结果：
            // 
            string ss = "{\"BigPnr\":\"PLRGCW\",\"FlightNo\":\"CZ3479\",\"SCity\":\"CGO\",\"ECity\":\"CKG\"}";

            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：{"state":true,"error":null,"config":"o72fd431","OfficeNo":"SHA243","result":{"Pnr":"JVL94L","OfficeNo":"SHA243","BookingState":0,"BigPNR":"NWZJTC","Command":"SS: CZ6178/Y/24JUN/CGQCSX/1\r\nSS: CZ3937/M/25JUN/CSXCGQ/1\r\nNM 1张龙\r\nTKTL1636/06MAY/SHA888\r\nSSR FOID CZ HK/NI610103197010032517/P1\r\nOSI CZ CTCT 18101810679\r\nRMK TJ AUTH CGQ203\r\n\\","ResultBag":"JVL94L -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  CZ6178  Y WE24JUN  CGQCSX DK1   1340 1830                                    \r  CZ3937  M TH25JUN  CSXCGQ DK1   1340 1840                                    \r 『航空公司使用自动出票时限, 请检查PNR』                                                        \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1430897792270+0800)\/","SaveTime":1800}
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, ss);
            Console.WriteLine("返回结果：" + sret);
        }
    }
}
