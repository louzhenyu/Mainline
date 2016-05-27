using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EtermProxy.BLL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JetermEntity.Request;
using JetermEntity.Response;

namespace EtermProxy.UnitTest
{ 
    [TestClass]
    public class TicketInfoTest
    {
        [TestMethod]
        public void Test_BusinessDispose()
        {
#warning 补测另外种情况：【O FM:1CSX CZ    3461  U 26DEC 1425 OK U          26DEC4/26DEC4 20K OPEN FOR USE】，当第5个显示为【OPEN】时（不是【26DEC】），起飞日期是不是被解析成string.Empty？
            JetermEntity.Request.TicketInfo request = new JetermEntity.Request.TicketInfo();
            request.TicketNo = "784-2158602564";
            //request.TicketNo = "7842133192747";
            //request.TicketNo = "7842130024027";

            EtermProxy.BLL.TicketInfo logic = new EtermProxy.BLL.TicketInfo(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.TicketInfo> result = logic.BusinessDispose(request);

            if (result != null && result.state)
            {
                // 运行结果，如：
                // {"TicketNo":"7842158602564","SCity":"CSX","ECity":"CTU","PassengerName":"张细志","Airline":"CZ","Cabin":"U","SDate":"26DEC","TicketStatus":"OPEN FOR USE","BigPnr":"NE8SJT"}
                // {"TicketNo":"7842158602564","SCity":"CSX","ECity":"CTU","PassengerName":"张细志","Airline":"CZ","Cabin":"U","SDate":"26DEC","TicketStatus":"USED/FLOWN","BigPnr":"NE8SJT"}
                Console.WriteLine("运行结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
                return;
            }
            if (result.error != null)
            {
                Console.WriteLine("运行错误，错误信息：" + result.error.ErrorMessage);
            }
        }

        // 2015-08-07（星期五），修复完Bug后，测试是否能得到正确的票号状态
        [TestMethod]
        public void Test_BusinessDispose2()
        {
            JetermEntity.Request.TicketInfo request = new JetermEntity.Request.TicketInfo();
            // 运行结果：
            request.TicketNo = "876-2353347683";
            // 运行结果：
            //request.TicketNo = "781-2198667987";
            // 运行结果：
            //request.TicketNo = "8769627417393";

            EtermProxy.BLL.TicketInfo logic = new EtermProxy.BLL.TicketInfo(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.TicketInfo> result = logic.BusinessDispose(request);

            if (result != null && result.state)
            {
                // 运行结果，如：
                // 
                // 
                Console.WriteLine("运行结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
                return;
            }
            if (result.error != null)
            {
                Console.WriteLine("运行错误，错误信息：" + result.error.ErrorMessage);
            }
        }

        // 2015-08-07（星期五），修复完Bug后，测试是否能得到正确的票号状态
        [TestMethod]
        public void Test_ParseCmdResult1()
        {
            //string cmdResult =
            string cmdResult1 =
@"
ISSUED BY:                           ORG/DST: CKG/LHW                 ARL-D    
E/R: 仅允许同价签转                                                                   
TOUR CODE:                                                                     
PASSENGER: 刘贵学                                                                 
EXCH:                               CONJ TKT:                                  
O FM:1CKG 3U    8851  Y 06AUG 1500 OK Y                        20K CHECKED IN  
     T2-- RL:PZVV81  /                                                         
  TO: LHW                                                                      
FARE:                      |FOP:                                               
TAX:                          |OI:                                             
TOTAL:                     |TKTN: 876-2353347683 
";

            //string cmdResult =
            string cmdResult222 =
@"
ISSUED BY:                           ORG/DST: CAN/SHA                 ARL-D    
E/R: Q/不得签转/变更退票收费                                                             
TOUR CODE:                                                                     
PASSENGER: 丁欢庆                                                                 
EXCH:                               CONJ TKT:                                  
O FM:1CAN MU    5316  B 06AUG 1730 OK B                        20K CHECKED IN  
     --T2 RL:                                                                  
  TO: SHA                                                                      
FARE:                      |FOP:                                               
TAX:                          |OI:                                             
TOTAL:                     |TKTN: 781-2198667987
";

            string cmdResult =
            //string cmdResult3 =
@"
ISSUED BY:                           ORG/DST: WUH/CTU                 BSP-D    
E/R: 不得签转                                                                      
TOUR CODE:                                                                     
PASSENGER: 吴建华                                                                 
EXCH:                               CONJ TKT:                                  
O FM:1WUH 3U    8986  I 06AUG 2230 OK FI                       40K CHECKED IN  
     --T1 RL:NCXB18  /KDXJ8Q1E                                                 
  TO: CTU                                                                      
FC:                                                                            
FARE:                      |FOP:                                               
TAX:                          |OI:                                             
TOTAL:                     |TKTN: 876-9627417393
";

            JetermEntity.Request.TicketInfo request = new JetermEntity.Request.TicketInfo();
            // 运行结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"TicketNo":"8762353347683","SCity":"CKG","ECity":"LHW","PassengerName":"刘贵学","Airline":"3U","Cabin":"Y","SDate":"06AUG","TicketStatus":4,"BigPnr":"PZVV81","LianChengTicketList":[],"ResultBag":"\r\nISSUED BY:                           ORG/DST: CKG/LHW                 ARL-D    \r\nE/R: 仅允许同价签转                                                                   \r\nTOUR CODE:                                                                     \r\nPASSENGER: 刘贵学                                                                 \r\nEXCH:                               CONJ TKT:                                  \r\nO FM:1CKG 3U    8851  Y 06AUG 1500 OK Y                        20K CHECKED IN  \r\n     T2-- RL:PZVV81  /                                                         \r\n  TO: LHW                                                                      \r\nFARE:                      |FOP:                                               \r\nTAX:                          |OI:                                             \r\nTOTAL:                     |TKTN: 876-2353347683 \r\n"},"reqtime":"\/Date(1438927883777+0800)\/","SaveTime":1800}
            //request.TicketNo = "876-2353347683";
            // 运行结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"TicketNo":"7812198667987","SCity":"CAN","ECity":"SHA","PassengerName":"丁欢庆","Airline":"MU","Cabin":"B","SDate":"06AUG","TicketStatus":4,"BigPnr":null,"LianChengTicketList":[],"ResultBag":"\r\nISSUED BY:                           ORG/DST: CAN/SHA                 ARL-D    \r\nE/R: Q/不得签转/变更退票收费                                                             \r\nTOUR CODE:                                                                     \r\nPASSENGER: 丁欢庆                                                                 \r\nEXCH:                               CONJ TKT:                                  \r\nO FM:1CAN MU    5316  B 06AUG 1730 OK B                        20K CHECKED IN  \r\n     --T2 RL:                                                                  \r\n  TO: SHA                                                                      \r\nFARE:                      |FOP:                                               \r\nTAX:                          |OI:                                             \r\nTOTAL:                     |TKTN: 781-2198667987\r\n"},"reqtime":"\/Date(1438928225978+0800)\/","SaveTime":1800}
            //request.TicketNo = "781-2198667987";
            // 运行结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"","result":{"TicketNo":"8769627417393","SCity":"WUH","ECity":"CTU","PassengerName":"吴建华","Airline":"3U","Cabin":"I","SDate":"06AUG","TicketStatus":4,"BigPnr":"NCXB18","LianChengTicketList":[],"ResultBag":"\r\nISSUED BY:                           ORG/DST: WUH/CTU                 BSP-D    \r\nE/R: 不得签转                                                                      \r\nTOUR CODE:                                                                     \r\nPASSENGER: 吴建华                                                                 \r\nEXCH:                               CONJ TKT:                                  \r\nO FM:1WUH 3U    8986  I 06AUG 2230 OK FI                       40K CHECKED IN  \r\n     --T1 RL:NCXB18  /KDXJ8Q1E                                                 \r\n  TO: CTU                                                                      \r\nFC:                                                                            \r\nFARE:                      |FOP:                                               \r\nTAX:                          |OI:                                             \r\nTOTAL:                     |TKTN: 876-9627417393\r\n"},"reqtime":"\/Date(1438928316755+0800)\/","SaveTime":1800}
            request.TicketNo = "8769627417393";

            JetermEntity.Parser.TicketInfo ticketInfo = new JetermEntity.Parser.TicketInfo(string.Empty, string.Empty);
            //ticketInfo.ParseCmd(request);
            CommandResult<JetermEntity.Response.TicketInfo> result = ticketInfo.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}TicketInfo指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);
        }

        //===============added by Li Yang, April 13th, 2015================================
#warning code here--补测：联程票解析

        //2015-10-23（星期五），测试往返的信息解析是否正确，尤其是TicketStatus
        [TestMethod]
        public void Test_ParseCmdResult2()
        {
            //返回结果：
            //{"state":true,"error":null,"config":"","OfficeNo":"","result":{"TicketNo":"7848502092703","FlightList":[{"FlightNo":"","Airline":"CZ","Cabin":"E","SubCabin":"","SCity":"ZUH","ECity":"CGQ","DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":null,"DepDateString":"21OCT","TicketStatus":5},{"FlightNo":"","Airline":"CZ","Cabin":"E","SubCabin":"","SCity":"CGQ","ECity":"ZUH","DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":null,"DepDateString":"","TicketStatus":8}],"PassengerName":"周朋","BigPnr":"PW63PH","LianChengTicketList":[],"ResultBag":"\r\n\u0010DETR:TN/784-8502092703,AIR/CZ                                                  \r\nISSUED BY:                           ORG/DST: ZUH/ZUH                 vod-D     \r\nE/R: BUDEQIANZHUAN不得签转/BIANGENGTUIPIAOSHOUFEI变更退票收费                   \r\nTOUR CODE:                                                                      \r\nPASSENGER: 周朋                                                                 \r\nEXCH:                               CONJ TKT:                                   \r\nO FM:1ZUH CZ    3737  E 21OCT 0900 OK E          21OCT5/21OCT5 20K USED/FLOWN   \r\n          RL:PW63PH  /HS5T6V1E                                                  \r\nO TO:2CGQ CZ    OPEN  E OPEN          E          23OCT5/23OCT5 20K EXCHANGED    \r\n          RL:PW63PH  /HS5T6V1E                                                  \r\n  TO: ZUH                                                                       \r\nFC:                                                                             \r\nFARE:                      |FOP:                                                \r\nTAX:                       |OI:                                                 \r\nTOTAL:                     |TKTN: 784-8502092703\r\n"},"reqtime":"2015-10-23T11:20:11.2571078+08:00","SaveTime":1800,"ServerUrl":null}
            string cmdResult =
@"
DETR:TN/784-8502092703,AIR/CZ                                                  
ISSUED BY:                           ORG/DST: ZUH/ZUH                 vod-D     
E/R: BUDEQIANZHUAN不得签转/BIANGENGTUIPIAOSHOUFEI变更退票收费                   
TOUR CODE:                                                                      
PASSENGER: 周朋                                                                 
EXCH:                               CONJ TKT:                                   
O FM:1ZUH CZ    3737  E 21OCT 0900 OK E          21OCT5/21OCT5 20K USED/FLOWN   
          RL:PW63PH  /HS5T6V1E                                                  
O TO:2CGQ CZ    OPEN  E OPEN          E          23OCT5/23OCT5 20K EXCHANGED    
          RL:PW63PH  /HS5T6V1E                                                  
  TO: ZUH                                                                       
FC:                                                                             
FARE:                      |FOP:                                                
TAX:                       |OI:                                                 
TOTAL:                     |TKTN: 784-8502092703
";

            JetermEntity.Request.TicketInfo request = new JetermEntity.Request.TicketInfo();  
            //
            //request.TicketNo = "784-8502092703";    
            //返回结果：
            //{"state":true,"error":null,"config":"","OfficeNo":"","result":{"TicketNo":"7848502092703","FlightList":[{"FlightNo":"","Airline":"CZ","Cabin":"E","SubCabin":"","SCity":"ZUH","ECity":"CGQ","DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":null,"DepDateString":"21OCT","TicketStatus":5},{"FlightNo":"","Airline":"CZ","Cabin":"E","SubCabin":"","SCity":"CGQ","ECity":"ZUH","DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":null,"DepDateString":"","TicketStatus":8}],"PassengerName":"周朋","BigPnr":"PW63PH","LianChengTicketList":[],"ResultBag":"\r\n\u0010DETR:TN/784-8502092703,AIR/CZ                                                  \r\nISSUED BY:                           ORG/DST: ZUH/ZUH                 vod-D     \r\nE/R: BUDEQIANZHUAN不得签转/BIANGENGTUIPIAOSHOUFEI变更退票收费                   \r\nTOUR CODE:                                                                      \r\nPASSENGER: 周朋                                                                 \r\nEXCH:                               CONJ TKT:                                   \r\nO FM:1ZUH CZ    3737  E 21OCT 0900 OK E          21OCT5/21OCT5 20K USED/FLOWN   \r\n          RL:PW63PH  /HS5T6V1E                                                  \r\nO TO:2CGQ CZ    OPEN  E OPEN          E          23OCT5/23OCT5 20K EXCHANGED    \r\n          RL:PW63PH  /HS5T6V1E                                                  \r\n  TO: ZUH                                                                       \r\nFC:                                                                             \r\nFARE:                      |FOP:                                                \r\nTAX:                       |OI:                                                 \r\nTOTAL:                     |TKTN: 784-8502092703\r\n"},"reqtime":"2015-10-23T11:20:11.2571078+08:00","SaveTime":1800,"ServerUrl":null}
            request.TicketNo = "7848502092703";

            JetermEntity.Parser.TicketInfo ticketInfo = new JetermEntity.Parser.TicketInfo(string.Empty, string.Empty);
            //ticketInfo.ParseCmd(request);
            CommandResult<JetermEntity.Response.TicketInfo> result = ticketInfo.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{0}{2}", Environment.NewLine, result.error.InnerDetailedErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("TicketInfo指令返回结果为：{0}{1}", Environment.NewLine, cmdResult2)));
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
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);
        }

        //2015-10-23（星期五），测试联程的信息解析是否正确，尤其是TicketStatus
        [TestMethod]
        public void Test_ParseCmdResult3()
        {
            //返回结果：
            //{"state":true,"error":null,"config":"","OfficeNo":"","result":{"TicketNo":"7819642166753","FlightList":[{"FlightNo":"","Airline":"MU","Cabin":"Z","SubCabin":"","SCity":"WUX","ECity":"CKG","DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":null,"DepDateString":"26OCT","TicketStatus":1},{"FlightNo":"","Airline":"MU","Cabin":"R","SubCabin":"","SCity":"CKG","ECity":"PVG","DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":null,"DepDateString":"28OCT","TicketStatus":1}],"PassengerName":"焦志勇","BigPnr":"PFNSZP","LianChengTicketList":[],"ResultBag":"\r\n\u0010DETR:TN/781-9642166753                                                        \r\nISSUED BY:                           ORG/DST: WUX/SHA                 BSP-D     \r\nE/R: 不得签转/改期退票收费                                                      \r\nTOUR CODE:                                                                      \r\nPASSENGER: 焦志勇                                                               \r\nEXCH:                               CONJ TKT:                                   \r\nO FM:1WUX MU    2985  Z 26OCT 1425 OK Z                        20K OPEN FOR USE \r\n     --T2 RL:PFNSZP  /JSKN081E                                                  \r\nO TO:2CKG MU    5426  R 28OCT 1310 OK R                        20K OPEN FOR USE \r\n     T2T1 RL:PFNSZP  /JSKN081E                                                  \r\n  TO: PVG                                                                       \r\nFC:                                                                             \r\nFARE:                      |FOP:                                                \r\nTAX:                       |OI:                                                 \r\nTOTAL:                     |TKTN: 781-9642166753\r\n"},"reqtime":"2015-10-23T11:31:27.8791078+08:00","SaveTime":1800,"ServerUrl":null}
            string cmdResult =
@"
DETR:TN/781-9642166753                                                        
ISSUED BY:                           ORG/DST: WUX/SHA                 BSP-D     
E/R: 不得签转/改期退票收费                                                      
TOUR CODE:                                                                      
PASSENGER: 焦志勇                                                               
EXCH:                               CONJ TKT:                                   
O FM:1WUX MU    2985  Z 26OCT 1425 OK Z                        20K OPEN FOR USE 
     --T2 RL:PFNSZP  /JSKN081E                                                  
O TO:2CKG MU    5426  R 28OCT 1310 OK R                        20K OPEN FOR USE 
     T2T1 RL:PFNSZP  /JSKN081E                                                  
  TO: PVG                                                                       
FC:                                                                             
FARE:                      |FOP:                                                
TAX:                       |OI:                                                 
TOTAL:                     |TKTN: 781-9642166753
";

            JetermEntity.Request.TicketInfo request = new JetermEntity.Request.TicketInfo();
            //返回结果：
            //{"state":true,"error":null,"config":"","OfficeNo":"","result":{"TicketNo":"7819642166753","FlightList":[{"FlightNo":"","Airline":"MU","Cabin":"Z","SubCabin":"","SCity":"WUX","ECity":"CKG","DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":null,"DepDateString":"26OCT","TicketStatus":1},{"FlightNo":"","Airline":"MU","Cabin":"R","SubCabin":"","SCity":"CKG","ECity":"PVG","DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":null,"DepDateString":"28OCT","TicketStatus":1}],"PassengerName":"焦志勇","BigPnr":"PFNSZP","LianChengTicketList":[],"ResultBag":"\r\n\u0010DETR:TN/781-9642166753                                                        \r\nISSUED BY:                           ORG/DST: WUX/SHA                 BSP-D     \r\nE/R: 不得签转/改期退票收费                                                      \r\nTOUR CODE:                                                                      \r\nPASSENGER: 焦志勇                                                               \r\nEXCH:                               CONJ TKT:                                   \r\nO FM:1WUX MU    2985  Z 26OCT 1425 OK Z                        20K OPEN FOR USE \r\n     --T2 RL:PFNSZP  /JSKN081E                                                  \r\nO TO:2CKG MU    5426  R 28OCT 1310 OK R                        20K OPEN FOR USE \r\n     T2T1 RL:PFNSZP  /JSKN081E                                                  \r\n  TO: PVG                                                                       \r\nFC:                                                                             \r\nFARE:                      |FOP:                                                \r\nTAX:                       |OI:                                                 \r\nTOTAL:                     |TKTN: 781-9642166753\r\n"},"reqtime":"2015-10-23T11:31:27.8791078+08:00","SaveTime":1800,"ServerUrl":null}
            request.TicketNo = "781-9642166753";    
            //返回结果：
            //
            //request.TicketNo = "7819642166753";

            JetermEntity.Parser.TicketInfo ticketInfo = new JetermEntity.Parser.TicketInfo(string.Empty, string.Empty);
            //ticketInfo.ParseCmd(request);
            CommandResult<JetermEntity.Response.TicketInfo> result = ticketInfo.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{0}{2}", Environment.NewLine, result.error.InnerDetailedErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("TicketInfo指令返回结果为：{0}{1}", Environment.NewLine, cmdResult2)));
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
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);
        }

        //2015-10-23（星期五），测试单程的信息解析是否正确，尤其是TicketStatus
        [TestMethod]
        public void Test_ParseCmdResult4()
        {
            //返回结果：
            string cmdResult =
@"
■DETR:TN/784-2158602564
DETR:TN/784-2158602564,AIR/CZ                                                 
ISSUED BY:                           ORG/DST: CSX/CTU                 vod-D    
E/R: BUDEQIANZHUAN不得签转/BIANGENGTUIPIAOSHOUFEI变更退票收费                            
TOUR CODE:                                                                     
PASSENGER: 张细志                                                                 
EXCH:                               CONJ TKT:                                  
O FM:1CSX CZ    3461  U 26DEC 1425 OK U          26DEC4/26DEC4 20K OPEN FOR USE 
     --T2 RL:NE8SJT  /HWP8PF1E                                                 
  TO: CTU                                                                      
FC:                                                                            
FARE:                      |FOP:                                               
TAX:                       |OI:                                                
TOTAL:                     |TKTN: 784-2158602564
";

            JetermEntity.Request.TicketInfo request = new JetermEntity.Request.TicketInfo();
            //返回结果：
            //{"state":true,"error":null,"config":"","OfficeNo":"","result":{"TicketNo":"7842158602564","FlightList":[{"FlightNo":"","Airline":"CZ","Cabin":"U","SubCabin":"","SCity":"CSX","ECity":"CTU","DepTerminal":null,"ArrTerminal":null,"DepDate":"0001-01-01T00:00:00","ArrDate":"0001-01-01T00:00:00","PNRState":null,"DepDateString":"26DEC","TicketStatus":1}],"PassengerName":"张细志","BigPnr":"NE8SJT","LianChengTicketList":[],"ResultBag":"\r\n■DETR:TN/784-2158602564\r\n\u000eDETR:TN/784-2158602564,AIR/CZ                                                 \r\nISSUED BY:                           ORG/DST: CSX/CTU                 vod-D    \r\nE/R: BUDEQIANZHUAN不得签转/BIANGENGTUIPIAOSHOUFEI变更退票收费                            \r\nTOUR CODE:                                                                     \r\nPASSENGER: 张细志                                                                 \r\nEXCH:                               CONJ TKT:                                  \r\nO FM:1CSX CZ    3461  U 26DEC 1425 OK U          26DEC4/26DEC4 20K OPEN FOR USE \r\n     --T2 RL:NE8SJT  /HWP8PF1E                                                 \r\n  TO: CTU                                                                      \r\nFC:                                                                            \r\nFARE:                      |FOP:                                               \r\nTAX:                       |OI:                                                \r\nTOTAL:                     |TKTN: 784-2158602564\r\n"},"reqtime":"2015-10-23T11:37:29.4921078+08:00","SaveTime":1800,"ServerUrl":null}
            request.TicketNo = "784-2158602564";
            //返回结果：
            //
            //request.TicketNo = "7842158602564";

            JetermEntity.Parser.TicketInfo ticketInfo = new JetermEntity.Parser.TicketInfo(string.Empty, string.Empty);
            //ticketInfo.ParseCmd(request);
            CommandResult<JetermEntity.Response.TicketInfo> result = ticketInfo.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{0}{2}", Environment.NewLine, result.error.InnerDetailedErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("TicketInfo指令返回结果为：{0}{1}", Environment.NewLine, cmdResult2)));
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
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);
        }
    }
}
