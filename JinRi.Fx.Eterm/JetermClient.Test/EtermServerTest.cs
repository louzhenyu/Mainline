using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JetermEntity.Request;
using JetermEntity;
using JetermEntity.Response;
using JetermClient.BLL;

namespace JetermClient.Test
{
    [TestClass]
    public class EtermServerTest
    {
        //public static void Main3(string[] args)
        public static void Main(string[] args)
        {
            //            string s = "26AUG(";
            //            string cmdResult =
            //@"25AUG(TUE) SHEBJS VIA CA                                                      
            //1-  CA1602  DS# F2 A2 YA BS MS HS KS LS QS GS  SHEPEK 0930   1110   738 0^N  E  
            //>               SS NS VS US TS ES                                   -- T3  1:40
            //               ** M1S H1S K1S L1S Q1S V1S                                      
            //2   CA1652  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1100   1245   738 0^   E  
            //>               SA NS VS US TS ES                                   -- T3  1:45
            //               ** M1S H1S K1S L1S Q1S V1S                                      
            //3   CA1658  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1545   1720   738 0^   E  
            //>               SA NS VS US TS ES                                   -- T3  1:35
            //               ** M1S H1S K1S L1S Q1S V1S                                      
            //4   CA1636  DS# F8 A2 YA BS MS HS KS LS QS GS  SHEPEK 1910   2040   738 0^   E  
            //>               SA NS VS US TS ES                                   -- T3  1:30
            //               ** M1S H1S K1S L1S Q1S V1S                                      
            //5+  CA1626  DS# FA A4 YA BA MA HA KA LA QS GS  SHEPEK 2105   2240   73L 0^   E  
            //>               S8 NS VS US TS ES                                   -- T3  1:35
            //               ** M1A H1A K1A L1A Q1S V1S                                      
            //『**  CZ  PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』                   
            //『**  JD5100-JD5800 PLEASE CHECK IN 45 MINUTES BEFORE DEPARTURE AT SHE』 
            //";
            //            int i = cmdResult.IndexOf(s);

            //DateTime dt = DateTime.Now;
            //string yearStr = dt.Year.ToString().Substring(2, 2);
            //return;

            EtermServerTest test = new EtermServerTest();
            //test.Test_SeekPNR_Invoke31();
            //test.Test_SeekPNR_Adult_Invoke1();
            //test.Test_AdultBookingInvoke2();
            //test.Test_AdultBookingInvoke100();
            //test.Test_TicketByBigPnr_Invoke1();
            //test.Test_TicketInfoByF_Invoke1();
            //test.Test_TicketInfoByS_Invoke1();
            //test.Test_TicketByBigPnr_Invoke1();
            //test.Test_AdultBookingSS();
            //test.Test_AdultBookingInvoke4();
            //test.AVHTest_Invoke2();
            //test.Test_SeekPNR_Invoke1000();
            test.Test_SeekPNR_Invoke1000();
            return;

            EtermClient client = new EtermClient();
            Command<JetermEntity.Request.SeekPNR> seekPnr = new Command<JetermEntity.Request.SeekPNR>();
            seekPnr.AppId = 100001;
            seekPnr.request = new JetermEntity.Request.SeekPNR();
            seekPnr.request.Pnr = "KMZXN2";
            seekPnr.request.PassengerType = EtermCommand.PassengerType.Adult;
            seekPnr.request.GetPrice = true;
            CommandResult<JetermEntity.Response.SeekPNR> r1 = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(seekPnr);
            Console.ReadLine();
        }

        // 2015-07-22（星期三），帮助政策组寻找返回为null的原因
        public void AVHTest_Invoke2()
        {
            // AVH请求对象
            Command<JetermEntity.Request.AVH> cmd = new Command<JetermEntity.Request.AVH>();

            // 设置应用程序编号
            cmd.AppId = 100201;

            // 根据各自的业务需求，设置缓存返回结果时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "KHN117";
            //cmd.officeNo = "TPE567";
            //cmd.ConfigName = "o77a6411";

            cmd.request = new JetermEntity.Request.AVH();

            cmd.request.Airline = "MU";
            cmd.request.SCity = "PEK";
            cmd.request.ECity = "SHA";
            cmd.request.DepDate = Convert.ToDateTime("2015-07-24");

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();
            CommandResult<JetermEntity.Response.AVH> result = client.Invoke<JetermEntity.Request.AVH, JetermEntity.Response.AVH>(cmd);

            #endregion

            #region 业务处理

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.AVH>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}AVH指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult + Environment.NewLine + "返回的OfficeNo：" + result.OfficeNo + "；返回的配置名：" + result.config);

            #endregion
        }

        // 测试是否会抛出Newtonsoft.Json.JsonReaderException异常：
        // 测试在老平台订的PNR，通过新平台是否能被解析：
        // 测试当不传office号时，调用大系统是否能解析成功
        // 测试由非平台生成的PNR，调用大系统是否能解析成功
        // 测试：此PNR由非平台生成，测下走大系统后是否能被调出信息
        public void Test_SeekPNR_Invoke31()
        {
            // SeekPNR请求对象
            Command<JetermEntity.Request.SeekPNR> cmd = new Command<JetermEntity.Request.SeekPNR>();

            // 设置应用程序编号
            cmd.AppId = 100203;

            // 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.none;
            //cmd.CacheTime = EtermCommand.CacheTime.min10;

            //cmd.officeNo = "SHA243";

            cmd.request = new JetermEntity.Request.SeekPNR();

            #region SeekPNR请求参数

            // 返回的结果：
            // {"state":false,"error":{"ErrorCode":27,"ErrorMessage":"此记录编号无效，状态为已取消","CmdResultBag":"   *THIS PNR WAS ENTIRELY CANCELLED*                                           \r005     HDQCA 9983 0337 05JUN /RLC4                                            \r     X1.陈振海(001) X2.胡丽娅(001) X3.阮红艳(001) X4.孙巍(001)       X5.吴天文(001) X6.夏伟平(001\r) X7.杨本平(001) X8.杨玲(001)         KEZKFC                                        \r001 X9.  TV9878 B   TU09JUN  CKGLZY XX8   0630 0855          E T2--            \r       NN(001)  DK(001)  HK(001)  XX(004)                                      \r001X10.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG  \r        ABCDEFG                                                                \r001X11.TL/1213/05JUN/SHA888                                                    \r001X12.SSR FOID TV XX1 NI420102196804113320                                    \r       HK(001)   XX(004)   XX(004)                                             +\r\r\n001X13.SSR FOID TV XX1 NI420103195505291233                                    -\r       HK(001)   XX(004)   XX(004)                                             \r001X14.SSR FOID TV XX1 NI420103196312280874                                    \r       HK(001)   XX(004)   XX(004)                                             \r001X15.SSR FOID TV XX1 NI420123197601011726                                    \r       HK(001)   XX(004)   XX(004)                                             \r001X16.SSR FOID TV XX1 NI420104196212271635                                    \r       HK(001)   XX(004)   XX(004)                                             \r001X17.SSR FOID TV XX1 NI420103196701160841                                    \r       HK(001)   XX(004)   XX(004)                                             \r001X18.SSR FOID TV XX1 NI420700197903301620                                    \r       HK(001)   XX(004)   XX(004)                                             +\r\r\n001X19.SSR FOID TV XX1 NI420102197410112012                                    -\r       HK(001)   XX(004)   XX(004)                                             \r003X20.SSR ADTK 1E BY SHA05JUN15/1215 OR CXL TV9878 B09JUN                     \r001X21.OSI TV CTCT18971552053                                                  \r001X22.RMK TJ AUTH HGH157                                                      \r002X23.RMK CA/NHYQNB                                                           \r001 24.SHA243                                                                  +\r\r\n001     SHA243 88700 0315 05JUN                                                -\r002     HDQCA 9983 0315 05JUN /RLC1                                            \r003     HDQCA 9983 0315 05JUN /1                                               \r004     SHA243 88700 0337 05JUN                                                \r005     HDQCA 9983 0337 05JUN /RLC4                                            \r\r\n\r\nNO ACTIVE SEGMENTS                                                             \r"},"config":"o72fd431","OfficeNo":"SHA243","result":{"PassengerList":[],"PNR":null,"FlightList":[],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":null,"Mobile":null,"OfficeNo":null,"AdultPnr":null,"PriceList":[],"ResultBag":null},"reqtime":"\/Date(1433476718411+0800)\/","SaveTime":1800}
            //cmd.request.Pnr = "KEZKFC";
            //cmd.request.PassengerType = EtermCommand.PassengerType.Adult;
            //cmd.request.GetPrice = true;

            // 测试是否会抛出Newtonsoft.Json.JsonReaderException异常：
            // 返回结果：
            // {"PassengerList":[{"name":"陈振海","idtype":0,"cardno":"NI420102197410112012","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"胡丽娅","idtype":0,"cardno":"NI420700197903301620","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"阮红艳","idtype":0,"cardno":"NI420103196701160841","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"孙巍","idtype":0,"cardno":"NI420104196212271635","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"吴天文","idtype":0,"cardno":"NI420123197601011726","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"夏伟平","idtype":0,"cardno":"NI420103196312280874","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"杨本平","idtype":0,"cardno":"NI420103195505291233","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"杨玲","idtype":0,"cardno":"NI420102196804113320","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HYEHSM","FlightList":[{"FlightNo":"TV9878","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CKG","ECity":"LZY","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1435703400000+0800)\/","ArrDate":"\/Date(1435712100000+0800)\/","PNRState":"HK8"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["HGH157"],"BigPNR":"MCGHY5","Mobile":"18971552053","OfficeNo":"SHA243","AdultPnr":null,"PriceList":[{"FacePrice":1510.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1560.00}],"ResultBag":" 1.陈振海 2.胡丽娅 3.阮红艳 4.孙巍 5.吴天文 6.夏伟平                                            \r 7.杨本平 8.杨玲 HYEHSM                                                             \r 9.  TV9878 Y   WE01JUL  CKGLZY HK8   0630 0855          E T2--                \r10.SHA/T SHA/T021-36412780/SHANGHAI FENGRUI INDUSTRY CO.LTD/ZHAO YAN ZHONG     \r     ABCDEFG                                                                   \r11.TL/1454/05JUN/SHA888                                                        \r12.SSR FOID TV HK1 NI420102196804113320/P8                                     \r13.SSR FOID TV HK1 NI420103195505291233/P7                                     \r14.SSR FOID TV HK1 NI420103196312280874/P6                                     \r15.SSR FOID TV HK1 NI420123197601011726/P5                                     \r16.SSR FOID TV HK1 NI420104196212271635/P4                                     \r17.SSR FOID TV HK1 NI420103196701160841/P3                                     +\r\r\n18.SSR FOID TV HK1 NI420700197903301620/P2                                     -\r19.SSR FOID TV HK1 NI420102197410112012/P1                                     \r20.SSR ADTK 1E BY SHA05JUN15/1455 OR CXL TV9878 Y01JUL                         \r21.OSI TV CTCT18971552053                                                      \r22.RMK TJ AUTH HGH157                                                          \r23.RMK CA/MCGHY5                                                               \r24.SHA243                                                                      \r\r\n\r\n>PAT:A                                                                         \r01 Y FARE:CNY1510.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1560.00                  \r?SFC:01   ?SFN:01                                                              \r"}
            //cmd.request.Pnr = "HYEHSM";
            //cmd.request.PassengerType = EtermCommand.PassengerType.Adult;
            //cmd.request.GetPrice = true;

            // 测试在老平台订的PNR，通过新平台是否能被解析：
            // 返回结果：
            // {"state":true,"error":null,"config":"o77a6431","OfficeNo":"TPE567","result":{"PassengerList":[{"name":"陆文婷","idtype":0,"cardno":"NI450104198708232044","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HNEX54","FlightList":[{"FlightNo":"CZ3237","Airline":"","Cabin":"E","SubCabin":"","SCity":"NNG","ECity":"SZX","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1434537300000+0800)\/","ArrDate":"\/Date(1434541800000+0800)\/","PNRState":"RR1"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["NNG168"],"BigPNR":"PTYNJH","Mobile":"13207714566","OfficeNo":"TPE567","AdultPnr":null,"PriceList":[{"FacePrice":450.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":500.00}],"ResultBag":"  **ELECTRONIC TICKET PNR**                                                    \r 1.陆文婷 HNEX54                                                                  \r 2.  CZ3237 E   WE17JUN  NNGSZX RR1   1835 1950          E                     \r 3.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG      \r 4.T/0000000000000                                                             \r 5.SSR FOID CZ HK1 NI450104198708232044/P1                                     \r 6.SSR ADTK 1E BY TPE10JUN15/2126 OR CXL CZ BOOKING                            \r 7.SSR TKNE CZ HK1 NNGSZX 3237 E17JUN 7842182455363/1/P1                       \r 8.OSI CZ CTCT13207714566                                                      \r 9.RMK TJ AUTH NNG168                                                          \r10.RMK CA/PTYNJH                                                               \r11.TPE567                                                                      \r\r\n\r\n>PAT:A                                                                         \r01 E FARE:CNY450.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:500.00                    \r?SFC:01   ?SFN:01                                                              \r"},"reqtime":"\/Date(1434453092911+0800)\/","SaveTime":1800}
            //cmd.officeNo = "TPE567";
            //cmd.request.Pnr = "HNEX54";
            //cmd.request.PassengerType = EtermCommand.PassengerType.Adult;
            //cmd.request.GetPrice = true;

            // 测试在老平台订的PNR，通过新平台是否能被解析：
            // 返回结果：
            // {"state":true,"error":null,"config":"o77a6421","OfficeNo":"TPE567","result":{"PassengerList":[{"name":"唐ZHE","idtype":0,"cardno":"NI330103197606270033","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"张克","idtype":0,"cardno":"NI340321197310086558","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"林中","idtype":-1,"cardno":"","PassType":2,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"HRMEWW","FlightList":[{"FlightNo":"CZ3823","Airline":"","Cabin":"Y","SubCabin":"","SCity":"WUH","ECity":"HGH","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1434704400000+0800)\/","ArrDate":"\/Date(1434709200000+0800)\/","PNRState":"RR2"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":["WUH239"],"BigPNR":"NGSF6H","Mobile":"13605819159","OfficeNo":"TPE567","AdultPnr":null,"PriceList":[{"FacePrice":710.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":760.00}],"ResultBag":"  **ELECTRONIC TICKET PNR**                                                    \r 1.唐ZHE 2.张克 HRMEWW                                                            \r 3.  CZ3823 Y   FR19JUN  WUHHGH RR2   1700 1820          E                     \r 4.TPE/T TPE/T0285514894/BO TSE TRAVEL SERVICE CO LTD/WU RUEI BIN ABCDEFG      \r 5.T/0000000000000                                                             \r 6.SSR FOID CZ HK1 NI340321197310086558/P2                                     \r 7.SSR FOID CZ HK1 NI330103197606270033/P1                                     \r 8.SSR ADTK 1E BY TPE13JUN15/0939 OR CXL CZ BOOKING                            \r 9.SSR TKNE CZ HK1 WUHHGH 3823 Y19JUN 7842182722684/1/P1                       \r10.SSR TKNE CZ HK1 WUHHGH 3823 Y19JUN 7842182722685/1/P2                       \r11.SSR INFT CZ 『KK1』WUHHGH 3823 Y19JUN LIN/ZHONG 01JUN15/P1                    \r12.OSI CZ CTCT13605819159                                                      +\r\r\n13.OSI YY 1INF LINZHONGINF/P1                                                  -\r14.RMK TJ AUTH WUH239                                                          \r15.RMK CA/NGSF6H                                                               \r16.XN/IN/林中INF(JUN15)/P1                                                       \r17.TPE567                                                                      \r\r\n\r\n>PAT:A                                                                         \r01 Y FARE:CNY710.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:760.00                    \r?SFC:01   ?SFN:01                                                              \r"},"reqtime":"\/Date(1434453678630+0800)\/","SaveTime":1800}
            //cmd.officeNo = "TPE567";
            //cmd.request.Pnr = "HRMEWW";
            //cmd.request.PassengerType = EtermCommand.PassengerType.Adult;
            //cmd.request.GetPrice = true;

            // 测试当不传office号时，调用大系统是否成功
            // office号传了个来自非平台的office号：
            // 返回结果：
            // ErrorMessage: 系统出现故障
            //cmd.officeNo = "CAN378";
            //cmd.request.Pnr = "HFQB18";
            //cmd.request.Airline = "PN";
            ////cmd.request.PassengerType = EtermCommand.PassengerType.Adult;
            ////cmd.request.GetPrice = true;

            // RTPAT指令返回结果：
            /*
 1.区晓东 HFQB18
 2.  CZ3525 Y   TU16JUN15CANSHA HX1   1300 1525      E --T2 
 3.T CAN/CAN/T 020-86121141/BAI YUN AIRPORT YI JI SHANG LV LTD  
 4.T CAN//CHEN FENG QI  
 5.T/APT
 6.BA/CNPI ALDY PAM TOT CNY 
 7.SSR FOID CZ HK1 NI440104198109253415/P1  
 8.SSR OTHS 1E *FLT CZ3537/16JUN CNL REBOOK CZ3525/16JUN PLS ADV PAX
 9.SSR TKTL CA SS/ CAN 1900/15JUN15 
10.SSR FQTV CZ HK/ CZ080028514356/C/P1  
11.OSI CA CTC   
12.OSI YY CTCT                                                                 + 13.OSI CZ CTCT                                                                 -14.OSI CZ LSH CNPI020150615241050   
15.RMK PFS OFFLK CZ3525 Y 16JUN CANSHA  
16.PEK1E/HFQB18/CAN378  
 [price]>PAT:A  
01 Y FARE:CNY1350.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1400.00   
 SFC:01    SFN:01   
  RMK CA/PJD345
             */
            // 返回结果：
            // ErrorMessage: 此记录第1段航班信息的编号为“HX”状态不是有效记录编号，请检查您的PNR记录编号
            //cmd.officeNo = "CAN378";
            //cmd.request.Pnr = "HFQB18";
            //cmd.request.Airline = "CZ";

            //cmd.officeNo = "CAN378";
            //cmd.request.Pnr = "HFQB18";

            // 不传office号的情况：
            //cmd.request.Pnr = "HRMEWW";
            //cmd.request.Airline = "CZ";

            //cmd.request.Pnr = "HRMEWW";

            //cmd.request.Pnr = "JQ35WF";           

            //cmd.request.Pnr = "JQ35WF";
            //cmd.request.Airline = "MU";

            //cmd.request.Pnr = "JQ35WF";
            //cmd.request.Airline = "CZ";

            //cmd.request.Pnr = "JQ35WF";
            //cmd.request.Airline = "CA";

            //cmd.request.Pnr = "JQ35WF";
            //cmd.request.Airline = "PN";

            //cmd.request.Pnr = "KF1QET";

            //RTPAT指令返回结果：
            /*
              **ELECTRONIC TICKET PNR** 
             1.李YUE 2.张蓉 KF1QET  
             3.  CA4172 Q   TH18JUN  PEKKMG RR2   1150 1520      E T3-- 
             4.T KMG/KMG/T0871-65162666/EAST COAST AVIATION SERVICES CO.
             5.T KMG/LTD IN YUNNAN/LUO WENQIANG 
             6.T
             7.SSR FOID CA HK1 NI610302198301140568/P2  
             8.SSR FOID CA HK1 NI620102198111104620/P1  
             9.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787183/1/P2
            10.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787182/1/P1
            11.SSR OTHS 1E 1 CAAIRLINES ET PNR  
            12.SSR OTHS 1E 1 PNR RR AND PRINTED                                            +

            13.SSR OTHS 1E CA BKG CXLD DUE TO TKT TIME EXPIRED                             -

            14.SSR TKTL CA XX/ KMG 1900/17JUN15 
            15.SSR FQTV CA HK1 PEKKMG 4172 Q18JUN CA008334377481/C/P2   
            16.OSI CA CTC 63154444  
            17.OSI YY CTCT63154444  
            18.OSI CA CTCT1388*****49   
            19.OSI 1E CAET TN/9992375787182-9992375787183   
            20.RMK B2BPLATFORM WEB IMPORT   
            21.FN/M/FCNY1310.00/SCNY1310.00/C6.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/  
                ACNY1360.00 
            22.TN/999-2375787182/P1 
            23.TN/999-2375787183/P2                                                        +

            24.FP/CC/Y1                                                                    -

            25.PEK1E/KF1QET/KMG168  
            [price]>PAT:A  
            01 Q FARE:CNY1310.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1360.00   
            SFC:01   SFN:01   
            [eTerm:caa01] RMK CA/MJYPC2
             */
            // 返回结果：
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"李YUE","idtype":0,"cardno":"NI620102198111104620","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"99923757871829992375787183"},{"name":"张蓉","idtype":0,"cardno":"NI610302198301140568","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KF1QET","FlightList":[{"FlightNo":"CA4172","Airline":"","Cabin":"Q","SubCabin":"","SCity":"PEK","ECity":"KMG","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1434599400000+0800)\/","ArrDate":"\/Date(1434612000000+0800)\/","PNRState":"RR2"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"MJYPC2","Mobile":"63154444","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":1310.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1360.00}],"ResultBag":"  **ELECTRONIC TICKET PNR** \r 1.李YUE 2.张蓉 KF1QET  \r 3.  CA4172 Q   TH18JUN  PEKKMG RR2   1150 1520      E T3-- \r 4.T KMG/KMG/T0871-65162666/EAST COAST AVIATION SERVICES CO.\r 5.T KMG/LTD IN YUNNAN/LUO WENQIANG \r 6.T\r 7.SSR FOID CA HK1 NI610302198301140568/P2  \r 8.SSR FOID CA HK1 NI620102198111104620/P1  \r 9.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787183/1/P2\r10.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787182/1/P1\r11.SSR OTHS 1E 1 CAAIRLINES ET PNR  \r12.SSR OTHS 1E 1 PNR RR AND PRINTED                                            +\r\n\u001e13.SSR OTHS 1E CA BKG CXLD DUE TO TKT TIME EXPIRED                             -\r\n14.SSR TKTL CA XX/ KMG 1900/17JUN15 \r15.SSR FQTV CA HK1 PEKKMG 4172 Q18JUN CA008334377481/C/P2   \r16.OSI CA CTC 63154444  \r17.OSI YY CTCT63154444  \r18.OSI CA CTCT1388*****49   \r19.OSI 1E CAET TN/9992375787182-9992375787183   \r20.RMK B2BPLATFORM WEB IMPORT   \r21.FN/M/FCNY1310.00/SCNY1310.00/C6.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/  \r    ACNY1360.00 \r22.TN/999-2375787182/P1 \r23.TN/999-2375787183/P2                                                        +\r\n\u001e24.FP/CC/Y1                                                                    -\r\n25.PEK1E/KF1QET/KMG168  \r\u001e[price]>PAT:A  \r01 Q FARE:CNY1310.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1360.00   \r\u001eSFC:01   \u001eSFN:01   \r\u001e[eTerm:caa01] RMK CA/MJYPC2"},"reqtime":"\/Date(1434528800859+0800)\/","SaveTime":1800}
            //cmd.request.Pnr = "KF1QET";
            //cmd.request.Airline = "CA";

            // RTPAT信息：
            // AUTHORITY  
            // 返回结果：
            // 需要授权
            cmd.request.Pnr = "KF1QET";
            cmd.request.Airline = "EU";

            // 测试：此PNR由非平台生成，测下走大系统后是否能被调出信息
            //cmd.request.Pnr = "KP9613";

            // RTPAT信息返回结果：
            /*
              **ELECTRONIC TICKET PNR** 
             1.李YUE 2.张蓉 KF1QET  
             3.  CA4172 Q   TH18JUN  PEKKMG RR2   1150 1520      E T3-- 
             4.NC   
             5.NC   
             6.T
             7.SSR FOID CA HK1 NI610302198301140568/P2  
             8.SSR FOID CA HK1 NI620102198111104620/P1  
             9.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787183/1/P2
            10.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787182/1/P1
            11.SSR OTHS 1E 1 CAAIRLINES ET PNR  
            12.SSR OTHS 1E 1 PNR RR AND PRINTED                                            + 13.SSR OTHS 1E CA BKG CXLD DUE TO TKT TIME EXPIRED                             -14.SSR TKTL CA XX/ KMG 1900/17JUN15 
            15.SSR FQTV CA HK1 PEKKMG 4172 Q18JUN CA008334377481/C/P2   
            16.OSI CA CTC   
            17.OSI YY CTCT  
            18.OSI CA CTCT  
            19.OSI 1E CAET TN/9992375787182-9992375787183   
            20.RMK B2BPLATFORM WEB IMPORT   
            21.FN/M/FCNY1310.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/ACNY1360.00 
            22.TN/999-2375787182/P1 
            23.TN/999-2375787183/P2 
            24.FP/CC/                                                                      + 25.PEK888                                                                      - [price]>PAT:A  
            01 Q FARE:CNY1310.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1360.00   
             SFC:01    SFN:01   
              RMK CA/MJYPC2
             */
            // 返回结果：
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"李YUE","idtype":0,"cardno":"NI620102198111104620","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"99923757871829992375787183"},{"name":"张蓉","idtype":0,"cardno":"NI610302198301140568","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KF1QET","FlightList":[{"FlightNo":"CA4172","Airline":"","Cabin":"Q","SubCabin":"","SCity":"PEK","ECity":"KMG","DepTerminal":"T3","ArrTerminal":"","DepDate":"\/Date(1434599400000+0800)\/","ArrDate":"\/Date(1434612000000+0800)\/","PNRState":"RR2"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"MJYPC2","Mobile":"","OfficeNo":"PEK888","AdultPnr":null,"PriceList":[{"FacePrice":1310.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1360.00}],"ResultBag":"  **ELECTRONIC TICKET PNR** \r 1.李YUE 2.张蓉 KF1QET  \r 3.  CA4172 Q   TH18JUN  PEKKMG RR2   1150 1520      E T3-- \r 4.NC   \r 5.NC   \r 6.T\r 7.SSR FOID CA HK1 NI610302198301140568/P2  \r 8.SSR FOID CA HK1 NI620102198111104620/P1  \r 9.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787183/1/P2\r10.SSR TKNE CA HK1 PEKKMG 4172 Q18JUN 9992375787182/1/P1\r11.SSR OTHS 1E 1 CAAIRLINES ET PNR  \r12.SSR OTHS 1E 1 PNR RR AND PRINTED                                            + 13.SSR OTHS 1E CA BKG CXLD DUE TO TKT TIME EXPIRED                             -14.SSR TKTL CA XX/ KMG 1900/17JUN15 \r15.SSR FQTV CA HK1 PEKKMG 4172 Q18JUN CA008334377481/C/P2   \r16.OSI CA CTC   \r17.OSI YY CTCT  \r18.OSI CA CTCT  \r19.OSI 1E CAET TN/9992375787182-9992375787183   \r20.RMK B2BPLATFORM WEB IMPORT   \r21.FN/M/FCNY1310.00/XCNY50.00/TCNY50.00CN/TEXEMPTYQ/ACNY1360.00 \r22.TN/999-2375787182/P1 \r23.TN/999-2375787183/P2 \r24.FP/CC/                                                                      + 25.PEK888                                                                      - [price]>PAT:A  \r01 Q FARE:CNY1310.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1360.00   \r SFC:01    SFN:01   \r  RMK CA/MJYPC2"},"reqtime":"\/Date(1434529637581+0800)\/","SaveTime":1800}
            //cmd.request.Pnr = "KF1QET";
            //cmd.request.Airline = "CZ";

            // ErrorMessage: 指令返回为空
            //cmd.request.Pnr = "KF1QET";
            //cmd.request.Airline = "MU";

            // RTPAT信息返回：err
            // ErrorMessage: 系统出现故障
            //cmd.request.Pnr = "KF1QET";
            //cmd.request.Airline = "PN";

            // ErrorMessage: 指令返回为空
            //cmd.request.Pnr = "KXN58Y";

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
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"DANAHER/MAE XIANG","idtype":0,"cardno":"NI478536670","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/KYLIE HONGSHUN","idtype":0,"cardno":"NI517925439","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/REBECCA SUE","idtype":0,"cardno":"NI518926188","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KXN58Y","FlightList":[{"FlightNo":"CZ6670","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CTU","ECity":"KWL","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1434863100000+0800)\/","ArrDate":"\/Date(1434869400000+0800)\/","PNRState":"UN3"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"NWEXF0","Mobile":"18001367952","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":980.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1030.00}],"ResultBag":" 1.DANAHER/MAE XIANG 2.PERRY/KYLIE HONGSHUN 3.PERRY/REBECCA SUE KXN58Y  \r 4.  CZ6670 Y   SU21JUN  CTUKWL UN3   1305 1450      E T2-- \r 5.T BJS/PEK/T-65906699/BEIJING CHINA EXPRESS INTERNATIONAL AI  \r 6.T BJS/R SERVICE CO. LTD/WEI CHONG\r 7.T/APT\r 8.BA/GCP ALDY PAM TOT CNY  \r 9.SSR FOID CZ HK1 NI517925439/P2   \r10.SSR FOID CZ HK1 NI518926188/P3   \r11.SSR FOID CZ HK1 NI478536670/P1   \r12.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508902/1/P3\r13.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508901/1/P2\r14.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508900/1/P1                       + 15.SSR TKTL CA SS/ PEK 1200/01JUN15                                            -16.OSI CA CTC CHELI \r17.OSI CZ CTCT18001367952   \r18.OSI YY RLOC PEK1EJQPSW6  \r19.OSI CZ LSH GCP020150511370151\r20.PEK1E/KXN58Y/PEK587  \r [price]>PAT:A  \r01 Y FARE:CNY980.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1030.00\r SFC:01    SFN:01   \r  RMK CA/NWEXF0"},"reqtime":"\/Date(1434529959057+0800)\/","SaveTime":1800}
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"DANAHER/MAE XIANG","idtype":0,"cardno":"NI478536670","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/KYLIE HONGSHUN","idtype":0,"cardno":"NI517925439","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/REBECCA SUE","idtype":0,"cardno":"NI518926188","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KXN58Y","FlightList":[{"FlightNo":"CZ6670","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CTU","ECity":"KWL","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1434863100000+0800)\/","ArrDate":"\/Date(1434869400000+0800)\/","PNRState":"UN3"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"NWEXF0","Mobile":"18001367952","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":980.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1030.00}],"ResultBag":" 1.DANAHER/MAE XIANG 2.PERRY/KYLIE HONGSHUN 3.PERRY/REBECCA SUE KXN58Y  \r 4.  CZ6670 Y   SU21JUN  CTUKWL UN3   1305 1450      E T2-- \r 5.T BJS/PEK/T-65906699/BEIJING CHINA EXPRESS INTERNATIONAL AI  \r 6.T BJS/R SERVICE CO. LTD/WEI CHONG\r 7.T/APT\r 8.BA/GCP ALDY PAM TOT CNY  \r 9.SSR FOID CZ HK1 NI517925439/P2   \r10.SSR FOID CZ HK1 NI518926188/P3   \r11.SSR FOID CZ HK1 NI478536670/P1   \r12.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508902/1/P3\r13.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508901/1/P2\r14.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508900/1/P1                       + 15.SSR TKTL CA SS/ PEK 1200/01JUN15                                            -16.OSI CA CTC CHELI \r17.OSI CZ CTCT18001367952   \r18.OSI YY RLOC PEK1EJQPSW6  \r19.OSI CZ LSH GCP020150511370151\r20.PEK1E/KXN58Y/PEK587  \r [price]>PAT:A  \r01 Y FARE:CNY980.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1030.00\r SFC:01    SFN:01   \r  RMK CA/NWEXF0"},"reqtime":"\/Date(1434531182317+0800)\/","SaveTime":1800}
            // cmd.CacheTime = EtermCommand.CacheTime.min10;
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"DANAHER/MAE XIANG","idtype":0,"cardno":"NI478536670","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/KYLIE HONGSHUN","idtype":0,"cardno":"NI517925439","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/REBECCA SUE","idtype":0,"cardno":"NI518926188","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KXN58Y","FlightList":[{"FlightNo":"CZ6670","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CTU","ECity":"KWL","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1434863100000+0800)\/","ArrDate":"\/Date(1434869400000+0800)\/","PNRState":"UN3"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"NWEXF0","Mobile":"18001367952","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":980.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1030.00}],"ResultBag":" 1.DANAHER/MAE XIANG 2.PERRY/KYLIE HONGSHUN 3.PERRY/REBECCA SUE KXN58Y  \r 4.  CZ6670 Y   SU21JUN  CTUKWL UN3   1305 1450      E T2-- \r 5.T BJS/PEK/T-65906699/BEIJING CHINA EXPRESS INTERNATIONAL AI  \r 6.T BJS/R SERVICE CO. LTD/WEI CHONG\r 7.T/APT\r 8.BA/GCP ALDY PAM TOT CNY  \r 9.SSR FOID CZ HK1 NI517925439/P2   \r10.SSR FOID CZ HK1 NI518926188/P3   \r11.SSR FOID CZ HK1 NI478536670/P1   \r12.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508902/1/P3\r13.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508901/1/P2\r14.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508900/1/P1                       + 15.SSR TKTL CA SS/ PEK 1200/01JUN15                                            -16.OSI CA CTC CHELI \r17.OSI CZ CTCT18001367952   \r18.OSI YY RLOC PEK1EJQPSW6  \r19.OSI CZ LSH GCP020150511370151\r20.PEK1E/KXN58Y/PEK587  \r [price]>PAT:A  \r01 Y FARE:CNY980.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1030.00\r SFC:01    SFN:01   \r  RMK CA/NWEXF0"},"reqtime":"\/Date(1434531182317+0800)\/","SaveTime":1800}
            // cmd.CacheTime = EtermCommand.CacheTime.none;
            // {"state":true,"error":null,"config":null,"OfficeNo":null,"result":{"PassengerList":[{"name":"DANAHER/MAE XIANG","idtype":0,"cardno":"NI478536670","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/KYLIE HONGSHUN","idtype":0,"cardno":"NI517925439","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""},{"name":"PERRY/REBECCA SUE","idtype":0,"cardno":"NI518926188","PassType":0,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":""}],"PNR":"KXN58Y","FlightList":[{"FlightNo":"CZ6670","Airline":"","Cabin":"Y","SubCabin":"","SCity":"CTU","ECity":"KWL","DepTerminal":"T2","ArrTerminal":"","DepDate":"\/Date(1434863100000+0800)\/","ArrDate":"\/Date(1434869400000+0800)\/","PNRState":"UN3"}],"ShareFlight":false,"FlightType":0,"RMKOfficeNoList":[],"BigPNR":"NWEXF0","Mobile":"18001367952","OfficeNo":null,"AdultPnr":null,"PriceList":[{"FacePrice":980.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":1030.00}],"ResultBag":" 1.DANAHER/MAE XIANG 2.PERRY/KYLIE HONGSHUN 3.PERRY/REBECCA SUE KXN58Y  \r 4.  CZ6670 Y   SU21JUN  CTUKWL UN3   1305 1450      E T2-- \r 5.T BJS/PEK/T-65906699/BEIJING CHINA EXPRESS INTERNATIONAL AI  \r 6.T BJS/R SERVICE CO. LTD/WEI CHONG\r 7.T/APT\r 8.BA/GCP ALDY PAM TOT CNY  \r 9.SSR FOID CZ HK1 NI517925439/P2   \r10.SSR FOID CZ HK1 NI518926188/P3   \r11.SSR FOID CZ HK1 NI478536670/P1   \r12.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508902/1/P3\r13.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508901/1/P2\r14.SSR TKNE CZ XX1 CTUKWL 6670 Y21JUN 7842178508900/1/P1                       + 15.SSR TKTL CA SS/ PEK 1200/01JUN15                                            -16.OSI CA CTC CHELI \r17.OSI CZ CTCT18001367952   \r18.OSI YY RLOC PEK1EJQPSW6  \r19.OSI CZ LSH GCP020150511370151\r20.PEK1E/KXN58Y/PEK587  \r [price]>PAT:A  \r01 Y FARE:CNY980.00 TAX:CNY50.00 YQ:TEXEMPTYQ  TOTAL:1030.00\r SFC:01    SFN:01   \r  RMK CA/NWEXF0"},"reqtime":"\/Date(1434531445280+0800)\/","SaveTime":1800}
            // ErrorMessage：此记录编号无效，状态为已取消
            //cmd.request.Pnr = "KXN58Y";
            //cmd.request.Airline = "CZ";

            #endregion

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd);
            CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd, true);

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
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}SeekPNR指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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

            //Console.ReadLine();

            #endregion
        }

        public void Test_TicketByBigPnr_Invoke1()
        {
            // TicketByBigPnr请求对象
            Command<JetermEntity.Request.TicketByBigPnr> cmd = new Command<JetermEntity.Request.TicketByBigPnr>();

            // 设置应用程序编号
            cmd.AppId = 900630;

            // 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "SHA243";
            //cmd.ConfigName = "o72fe261";
            //cmd.ConfigName = "O77124B1";

            cmd.request = new JetermEntity.Request.TicketByBigPnr();

            #region TicketInfoByS请求参数

            // BigPnr = "NDDFGN", FlightNo = "3U8705", SCity = "CTU", ECity = "SZX" 

            //cmd.request.BigPnr = "NK9Y8G";
            //cmd.request.FlightNo = "CZ6178";
            //cmd.request.SCity = "CGQ";
            //cmd.request.ECity = "HFE";

            // 返回结果：{"PassengerList":[{"name":"郭庭财","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"8762348556932"}],"FlightList":null,"Price":{"FacePrice":740.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":790.00},"TicketStatus":1}
            //cmd.request.BigPnr = "NDDFGN";
            //cmd.request.FlightNo = "3U8705";
            //cmd.request.SCity = "CTU";
            //cmd.request.ECity = "SZX";

            // 返回结果：{"PassengerList":[{"name":"邹丽媛","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"7812192001934"}],"FlightList":[{"FlightNo":"MU5522","Airline":"MU","Cabin":"R","SCity":"MDG","ECity":"TAO","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1444377600000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":1220.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":1320.00},"TicketStatus":1}
            //cmd.request.BigPnr = "PG5ZRE";
            //cmd.request.FlightNo = "MU5522";
            //cmd.request.SCity = "MDG";
            //cmd.request.ECity = "TAO";

            // 测试TicketNo是否返回为?
            // 返回结果：
            // {"PassengerList":[{"name":"边惠敏","idtype":0,"cardno":"410104197704224520","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"?"},{"name":"李汶静","idtype":0,"cardno":"410303197402251026","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"?"},{"name":"海伟","idtype":0,"cardno":"410104197504041017","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"?"}],"FlightList":[{"FlightNo":"CZ3479","Airline":"CZ","Cabin":"","SubCabin":"","SCity":"CGO","ECity":"CKG","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1432656000000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":1}
            //cmd.request.BigPnr = "PLRGCW";
            //cmd.request.FlightNo = "CZ3479";
            //cmd.request.SCity = "CGO";
            //cmd.request.ECity = "CKG";

            // 测试是否能取到票号
            // 测试是否能成功换页
            // 返回结果：
            // {"PassengerList":[{"name":"许涛","idtype":0,"cardno":"330203197209250317","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707999"},{"name":"朱汉民","idtype":0,"cardno":"330227196812169018","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363708000"},{"name":"吴开封","idtype":0,"cardno":"330222197312078235","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707998"},{"name":"王勇","idtype":0,"cardno":"330203197201180318","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707997"},{"name":"李军","idtype":0,"cardno":"330204197412261034","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707996"}],"FlightList":[{"FlightNo":"CA1765","Airline":"CA","Cabin":"","SubCabin":"","SCity":"HGH","ECity":"LHW","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1433433600000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":1}
            // {"PassengerList":[{"name":"许涛","idtype":0,"cardno":"330203197209250317","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707999"},{"name":"朱汉民","idtype":0,"cardno":"330227196812169018","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363708000"},{"name":"吴开封","idtype":0,"cardno":"330222197312078235","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707998"},{"name":"王勇","idtype":0,"cardno":"330203197201180318","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707997"},{"name":"李军","idtype":0,"cardno":"330204197412261034","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707996"},{"name":"曹敏君","idtype":0,"cardno":"330224197210074316","PassType":-1,"Ename":"","BabyBirthday":"\/Date(-62135596800000+0800)\/","ChildBirthday":"\/Date(-62135596800000+0800)\/","TicketNo":"9992363707995"}],"FlightList":[{"FlightNo":"CA1765","Airline":"CA","Cabin":"","SubCabin":"","SCity":"HGH","ECity":"LHW","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1433433600000+0800)\/","ArrDate":"\/Date(-62135596800000+0800)\/","PNRState":null}],"Price":{"FacePrice":0.0,"Tax":0.0,"Fuel":0.0,"TotalPrice":0.0},"TicketStatus":1}
            //cmd.request.BigPnr = "NHG4VK";
            //cmd.request.FlightNo = "CA1765";
            //cmd.request.SCity = "HGH";
            //cmd.request.ECity = "LHW";

            // 测试第1种情况的返回结果为什么只解析到了只有1个人的信息
            cmd.request.BigPnr = "NTX3P7";
            cmd.request.FlightNo = "HU7225";
            cmd.request.SCity = "PEK";
            cmd.request.ECity = "WEF";

            #endregion

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();

            CommandResult<JetermEntity.Response.TicketByBigPnr> result = client.Invoke<JetermEntity.Request.TicketByBigPnr, JetermEntity.Response.TicketByBigPnr>(cmd);

            #endregion

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}TicketByBigPnr指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }

            // 返回结果：
            // {"PassengerList":[{"name":"LIU/JOANNE","idtype":-1,"cardno":"","PassType":-1,"Ename":"","BirthDayString":"","ChildBirthDayDate":"\/Date(-62135596800000+0800)\/","TicketNo":"7847589111741"}],"FlightList":[],"Price":{"FacePrice":1500.00,"Tax":8.20,"Fuel":0.0,"TotalPrice":254.20},"TicketStatus":5}
            string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();

            #endregion
        }

        public void Test_TicketInfoByF_Invoke1()
        {
            // TicketInfoByF请求对象
            Command<JetermEntity.Request.TicketInfoByF> cmd = new Command<JetermEntity.Request.TicketInfoByF>();

            // 设置应用程序编号
            cmd.AppId = 900630;

            // 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "SHA243";
            //cmd.ConfigName = "av06"; 
            //cmd.ConfigName = "av07";
            cmd.ConfigName = "av08";

            cmd.request = new JetermEntity.Request.TicketInfoByF();

            #region TicketInfo请求参数

            //cmd.request.TicketNo = "784-7589111741";
            //cmd.request.TicketNo = "7847589111741"; 
            //cmd.request.TicketNo = "999-8906177682 "; // 返回：{"TicketNo":"9998906177682","PassengerName":"刘军","PassengerCardNo":"230304196506075432","IsSchedule":true}
            //cmd.request.TicketNo = "999-8906177682";
            cmd.request.TicketNo = "999-8906177682";  // config: av08，结果Ok

            #endregion

            #region 调用Invoke以处理业务

            //EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.TicketInfoByF> result = client.Invoke<JetermEntity.Request.TicketInfoByF, JetermEntity.Response.TicketInfoByF>(cmd);

            EtermProxy.BLL.TicketInfoByF logic = new EtermProxy.BLL.TicketInfoByF(IntPtr.Zero, IntPtr.Zero, "av08", string.Empty);
            CommandResult<JetermEntity.Response.TicketInfoByF> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}TicketInfoByF指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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

            #endregion
        }

        public void Test_TicketInfoByS_Invoke1()
        {
            // TicketInfoByS请求对象
            Command<JetermEntity.Request.TicketInfoByS> cmd = new Command<JetermEntity.Request.TicketInfoByS>();

            // 设置应用程序编号
            cmd.AppId = 100630;

            // 根据各自的业务需求，设置缓存时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "KHN117";

            cmd.ConfigName = "av06"; // 返回未知错误
            //cmd.ConfigName = "av07"; // 
            //cmd.ConfigName = "av08";

            cmd.request = new JetermEntity.Request.TicketInfoByS();

            #region TicketInfoByS请求参数

            //cmd.request.TicketNo = "784-2158602564";
            // 返回结果：{"TicketNo":"9122340227002","PassengerName":"沈含笑","Price":{"FacePrice":360.00,"Tax":50.00,"Fuel":0.0,"TotalPrice":410.00},"FlightList":[{"FlightNo":"QW9792","Airline":"QW","Cabin":"Z","SCity":"HGH","ECity":"TAO","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1430898600000+0800)\/","ArrDate":"\/Date(1430905200000+0800)\/"}],"TicketStatus":13}
            //cmd.request.TicketNo = "912-2340227002";
            // 返回结果：{"TicketNo":"7846762540170","PassengerName":"齐峰","Price":{"FacePrice":3980.00,"Tax":100.00,"Fuel":0.0,"TotalPrice":4080.00},"FlightList":[{"FlightNo":"CZ6337","Airline":"CZ","Cabin":"H","SCity":"DLC","ECity":"HAK","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1424475900000+0800)\/","ArrDate":"\/Date(1424448000000+0800)\/"},{"FlightNo":"CZ8334","Airline":"CZ","Cabin":"Y","SCity":"HAK","ECity":"DLC","DepTerminal":null,"ArrTerminal":null,"DepDate":"\/Date(1424865600000+0800)\/","ArrDate":"\/Date(1424793600000+0800)\/"}],"TicketStatus":13}
            //cmd.request.TicketNo = "784-6762540170";
            cmd.request.TicketNo = "999-8906177682"; // 返回未知错误   
            //cmd.request.TicketNo = "784-6762540170";

            #endregion

            #region 调用Invoke以处理业务

            //EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.TicketInfoByS> result = client.Invoke<JetermEntity.Request.TicketInfoByS, JetermEntity.Response.TicketInfoByS>(cmd);

            EtermProxy.BLL.TicketInfoByS logic = new EtermProxy.BLL.TicketInfoByS(IntPtr.Zero, IntPtr.Zero, "av06", string.Empty);
            CommandResult<JetermEntity.Response.TicketInfoByS> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}TicketInfoByS指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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

            #endregion
        }

        public void Test_AdultBookingInvoke100()
        {
            // 订位请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();

            // 设置应用程序编号
            cmd.AppId = 100630;

            // 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "TPE566";
            //cmd.officeNo = "TPE999";
            //cmd.officeNo = "TPE111";
            //cmd.officeNo = "SHA243";
            //cmd.ConfigName = "O77124B1";
            //cmd.ConfigName = "o72fd421";
            cmd.officeNo = "TPE577";

            cmd.request = new JetermEntity.Request.Booking();

            #region 订位请求参数

            JetermEntity.Flight flight = new JetermEntity.Flight();

            flight.FlightNo = "CA1603";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-05-20");
            flight.SCity = "PEK";
            flight.ECity = "HRB";
            cmd.request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();

            passenger.name = "林忠秀";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "134541";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            cmd.request.OfficeNo = "SHA888";

            cmd.request.Mobile = "13641601096";

            cmd.request.RMKOfficeNoList.Add("TSN180");


            #endregion

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();

            CommandResult<JetermEntity.Response.Booking> result = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(cmd);

            //EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            //CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                result = new CommandResult<JetermEntity.Response.Booking>();
                result.error = new Error(EtermCommand.ERROR.SYSTEM_FAULT);
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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

            #endregion
        }

        // 测试成人往返票订位
        //[TestMethod]
        public void Test_AdultBookingInvoke2()
        {
            // 订位请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();

            // 设置应用程序编号
            cmd.AppId = 100630;

            // 根据各自的业务需求，设置缓存时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            cmd.officeNo = "SHA243";
            //cmd.officeNo = "SHA244";
            cmd.ConfigName = "O77124B1";

            cmd.request = new JetermEntity.Request.Booking();

            #region 订位请求参数

            JetermEntity.Flight flight = new JetermEntity.Flight();

            flight = new JetermEntity.Flight();
            //flight.FlightNo = "CA1884";
            //flight.Cabin = "B";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-05-30");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";
            //flight.FlightNo = "MU5110";
            //flight.Cabin = "Y";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-07-25");
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";

            //flight.FlightNo = "CA1893";
            //flight.Cabin = "G";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-04-24");
            //flight.SCity = "PVG";
            //flight.ECity = "SZX";
            //cmd.request.FlightList.Add(flight);

            //flight = new JetermEntity.Flight();
            //flight.FlightNo = "CA919";
            //flight.Cabin = "Q";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-04-29");
            //flight.SCity = "SZX";
            //flight.ECity = "PVG";
            //cmd.request.FlightList.Add(flight);

            //flight.FlightNo = "MU5137";
            //flight.Cabin = "H";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-04-27");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";
            //cmd.request.FlightList.Add(flight);

            //flight = new JetermEntity.Flight();
            //flight.FlightNo = "MU5156";
            //flight.Cabin = "B";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-04-30");
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";
            //cmd.request.FlightList.Add(flight);

            flight.FlightNo = "MU5623";
            flight.Cabin = "R";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-05-26");
            flight.SCity = "PVG";
            flight.ECity = "DLC";
            cmd.request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "MU5624";
            flight.Cabin = "V";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-06-05");
            flight.SCity = "DLC";
            flight.ECity = "PVG";
            cmd.request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            //passenger.name = "朱伟坚";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "440106196510042095";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            //cmd.request.PassengerList.Add(passenger);

            //passenger = new JetermEntity.Passenger();
            //passenger.name = "张路";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "341281198703102834";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            ////passenger.Ename = "zhuweijian";            

            passenger.name = "干园";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "650121199412242866";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            //passenger.name = "李刚";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "110101198810018696";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            ////passenger.Ename = "zhuweijian";

            //passenger.name = "张路";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "341281198703102834";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            ////passenger.Ename = "zhuweijian";

            //passenger.name = "徐大莹CHD";
            //passenger.Ename = "XuXiaoYIng";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "612732198903210342";
            //passenger.PassType = EtermCommand.PassengerType.Children;
            ////passenger.Ename = "zhuweijian";

            passenger = new JetermEntity.Passenger();
            passenger.name = "张杰";
            //passenger.Ename = "XuXiaoYIng";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "140525198401186312";
            //passenger.cardno = "650121199412242866";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            //passenger.name = "沈璐";
            ////passenger.Ename = "XuXiaoYIng";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "12312";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            ////passenger.Ename = "zhuweijian";

            //cmd.request.PassengerList.Add(passenger);

            //JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            //passenger.name = "张业华";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "310107198812044435";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhangyehua";
            //cmd.request.PassengerList.Add(passenger);

            //passenger = new JetermEntity.Passenger();
            //passenger.name = "杜俊强";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "310107198312044435";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            ////passenger.Ename = "dujunqiang";
            //cmd.request.PassengerList.Add(passenger);

            //cmd.request.OfficeNo = "KHN117";
            cmd.request.OfficeNo = "SHA888";
            //cmd.request.OfficeNo = "BJS579";

            cmd.request.Mobile = "13472634765";

            //cmd.request.RMKOfficeNoList.Add("KHN117");
            //cmd.request.RMKOfficeNoList.Add("SHA777");
            //cmd.request.RMKOfficeNoList.Add("PEK513");
            cmd.request.RMKOfficeNoList.Add("PEK277");
            cmd.request.RMKOfficeNoList.Add("CKG234");
            //cmd.request.RMKOfficeNoList.Add("CKG24");
            //cmd.request.RMKOfficeNoList.Add("PEK112");
            //cmd.request.RMKOfficeNoList.Add("SHA123");
            //cmd.request.RMKOfficeNoList.Add("PEK530");

            //cmd.request.RMKRemark = "test";         

            #endregion

            #region 调用Invoke以处理业务

            //EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.Booking> result = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(cmd);

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();

            #endregion
        }

        // 测试8位成人单程票的订位      
        public void Test_AdultBookingInvoke3()
        {
            // 订位请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();

            // 设置应用程序编号
            cmd.AppId = 100203;

            // 根据各自的业务需求，设置缓存时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "SHA243";
            //cmd.officeNo = "SHA244";
            //cmd.ConfigName = "O77124B1";

            cmd.request = new JetermEntity.Request.Booking();

            #region 订位请求参数

            JetermEntity.Flight flight = new JetermEntity.Flight();

            flight = new JetermEntity.Flight();
            //flight.FlightNo = "TV9878";
            //flight.Cabin = "Y";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-07-01");
            //flight.SCity = "CKG";
            //flight.ECity = "LZY";
            //cmd.request.FlightList.Add(flight);
            flight.FlightNo = "MU5262";
            flight.Cabin = "Y";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-07-10");
            flight.SCity = "SZX";
            flight.ECity = "KHN";
            cmd.request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "陈振海";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420102197410112012";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "胡丽娅";
            //passenger.Ename = "XuXiaoYIng";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420700197903301620";
            //passenger.cardno = "650121199412242866";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "阮红艳";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420103196701160841";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "孙巍";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420104196212271635";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "吴天文";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420123197601011726";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "夏伟平";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420103196312280874";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "杨本平";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420103195505291233";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "杨玲";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420102196804113320";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            cmd.request.PassengerList.Add(passenger);

            //cmd.request.OfficeNo = "KHN117";
            cmd.request.OfficeNo = "SHA888";
            //cmd.request.OfficeNo = "BJS579";

            cmd.request.Mobile = "18971552053";

            //cmd.request.RMKOfficeNoList.Add("KHN117");
            //cmd.request.RMKOfficeNoList.Add("SHA777");
            //cmd.request.RMKOfficeNoList.Add("PEK513");
            cmd.request.RMKOfficeNoList.Add("HGH157");
            //cmd.request.RMKOfficeNoList.Add("CKG24");
            //cmd.request.RMKOfficeNoList.Add("PEK112");
            //cmd.request.RMKOfficeNoList.Add("SHA123");
            //cmd.request.RMKOfficeNoList.Add("PEK530");

            //cmd.request.RMKRemark = "test";         

            #endregion

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();

            CommandResult<JetermEntity.Response.Booking> result = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(cmd);

            //EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            //CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();

            #endregion
        }

        // 测试是否能够重复订位1次
        public void Test_AdultBookingInvoke4()
        {
            // 订位请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();

            // 设置应用程序编号
            cmd.AppId = 100203;

            // 根据各自的业务需求，设置缓存时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "SHA243";
            //cmd.officeNo = "SHA244";
            //cmd.ConfigName = "O77124B1";

            cmd.request = new JetermEntity.Request.Booking();

            #region 订位请求参数

            cmd.officeNo = "TPE578";

            JetermEntity.Flight flight = new JetermEntity.Flight();

            flight = new JetermEntity.Flight();
            //flight.FlightNo = "CZ9273";
            //flight.Cabin = "Y";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-07-17");
            //flight.SCity = "BJS";
            //flight.ECity = "SHA";
            //cmd.request.FlightList.Add(flight);
            flight.FlightNo = "MU5262";
            flight.Cabin = "Y";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-07-10");
            flight.SCity = "SZX";
            flight.ECity = "KHN";
            cmd.request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "陈振海";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420102197410112012";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "胡丽娅";
            //passenger.Ename = "XuXiaoYIng";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420700197903301620";
            //passenger.cardno = "650121199412242866";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            //cmd.request.OfficeNo = "KHN117";
            cmd.request.OfficeNo = "SHA888";
            //cmd.request.OfficeNo = "BJS579";

            cmd.request.Mobile = "18971552053";

            //cmd.request.RMKOfficeNoList.Add("KHN117");
            //cmd.request.RMKOfficeNoList.Add("SHA777");
            //cmd.request.RMKOfficeNoList.Add("PEK513");
            cmd.request.RMKOfficeNoList.Add("HGH157");
            //cmd.request.RMKOfficeNoList.Add("CKG24");
            //cmd.request.RMKOfficeNoList.Add("PEK112");
            //cmd.request.RMKOfficeNoList.Add("SHA123");
            //cmd.request.RMKOfficeNoList.Add("PEK530");

            //cmd.request.RMKRemark = "test";         

            #endregion

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();
            CommandResult<JetermEntity.Response.Booking> result = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(cmd);

            //EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            //CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            Console.ReadLine();

            #endregion
        }

        // 测试姓名含有生僻字的订位
        public void Test_AdultBookingSS()
        {
            // 订位请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();

            // 设置应用程序编号
            cmd.AppId = 100203;

            // 根据各自的业务需求，设置缓存时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "SHA243";
            //cmd.officeNo = "SHA244";
            //cmd.ConfigName = "O77124B1";
            //cmd.ConfigName = "";

            cmd.request = new JetermEntity.Request.Booking();

            #region 订位请求参数

            JetermEntity.Flight flight = new JetermEntity.Flight();

            flight = new JetermEntity.Flight();
            //flight.FlightNo = "TV9878";
            //flight.Cabin = "Y";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-07-01");
            //flight.SCity = "CKG";
            //flight.ECity = "LZY";
            //cmd.request.FlightList.Add(flight);
            flight.FlightNo = "MU5393";
            flight.Cabin = "Y";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-07-18");
            flight.SCity = "LHW";
            flight.ECity = "NKG";
            cmd.request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "刘翾";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420102197410112012";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "周翀";
            //passenger.Ename = "XuXiaoYIng";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420700197903301620";
            //passenger.cardno = "650121199412242866";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "王夔";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420103196701160841";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "仝一";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "420104196212271635";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            cmd.request.PassengerList.Add(passenger);

            //passenger = new JetermEntity.Passenger();
            //passenger.name = "吴天文";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "420123197601011726";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            //cmd.request.PassengerList.Add(passenger);

            //passenger = new JetermEntity.Passenger();
            //passenger.name = "夏伟平";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "420103196312280874";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            //cmd.request.PassengerList.Add(passenger);

            //passenger = new JetermEntity.Passenger();
            //passenger.name = "杨本平";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "420103195505291233";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            //cmd.request.PassengerList.Add(passenger);

            //passenger = new JetermEntity.Passenger();
            //passenger.name = "杨玲";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "420102196804113320";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            //cmd.request.PassengerList.Add(passenger);

            //cmd.request.OfficeNo = "KHN117";
            cmd.request.OfficeNo = "SHA888";
            //cmd.request.OfficeNo = "BJS579";

            cmd.request.Mobile = "18971552053";

            //cmd.request.RMKOfficeNoList.Add("KHN117");
            //cmd.request.RMKOfficeNoList.Add("SHA777");
            //cmd.request.RMKOfficeNoList.Add("PEK513");
            cmd.request.RMKOfficeNoList.Add("HGH157");
            //cmd.request.RMKOfficeNoList.Add("CKG24");
            //cmd.request.RMKOfficeNoList.Add("PEK112");
            //cmd.request.RMKOfficeNoList.Add("SHA123");
            //cmd.request.RMKOfficeNoList.Add("PEK530");

            //cmd.request.RMKRemark = "test";         

            #endregion

            #region 调用Invoke以处理业务

            //EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.Booking> result = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(cmd);

            string bookingSS = new JetermEntity.Parser.Booking().ParseCmd(cmd.request);

            //EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            //CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(cmd.request);

            #endregion

            #region 业务处理

            //if (result == null)
            //{
            //    Console.WriteLine("没有返回结果");
            //    Console.ReadLine();
            //    return;
            //}
            //if (!result.state)
            //{
            //    string cmdResult2 = result.error.CmdResultBag;
            //    Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
            //    Console.ReadLine();
            //    return;
            //}
            //if (result.result == null)
            //{
            //    Console.WriteLine("没有返回结果");
            //    Console.ReadLine();
            //    return;
            //}

            //string parseResult = Newtonsoft.Json.JsonConvert.SerializeObject(result.result);
            //string parseResult2 = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            //Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }

        // 2015-09-25(5)测试：SeekPNR走非大系统时，当解析失败时，是否会获得具体的InnerDetailedErrorMessage。
        public void Test_SeekPNR_Invoke1000()
        {
            // SeekPNR请求对象
            Command<JetermEntity.Request.SeekPNR> cmd = new Command<JetermEntity.Request.SeekPNR>();

            // 设置应用程序编号
            cmd.AppId = 100203;

            // 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.none;
            //cmd.CacheTime = EtermCommand.CacheTime.min10;

            //cmd.officeNo = "SHA243";

            cmd.request = new JetermEntity.Request.SeekPNR();

            #region SeekPNR请求参数

            //返回结果：
            // 
            //cmd.officeNo = "SHA243";
            //cmd.ConfigName = "o72fe3b1";
            //cmd.ConfigName = "o72fe411";            
            //cmd.request.Pnr = "HGNLDE";
            cmd.request.Pnr = "HGNLDF";
            cmd.request.PassengerType = EtermCommand.PassengerType.Adult;
            cmd.request.Airline = "CZ";
            cmd.request.GetPrice = true;

            #endregion

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd);
            CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd);
            //CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd, true);

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
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.InnerDetailedErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}SeekPNR指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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

            //Console.ReadLine();

            #endregion
        }

        // 2015-09-25(5)测试：SeekPNR走大系统时，当抛出异常时，是否会获得具体的InnerDetailedErrorMessage。
        public void Test_SeekPNR_Invoke1001()
        {
            // SeekPNR请求对象
            Command<JetermEntity.Request.SeekPNR> cmd = new Command<JetermEntity.Request.SeekPNR>();

            // 设置应用程序编号
            cmd.AppId = 100203;

            // 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.none;
            //cmd.CacheTime = EtermCommand.CacheTime.min10;

            //cmd.officeNo = "SHA243";

            cmd.request = new JetermEntity.Request.SeekPNR();

            #region SeekPNR请求参数

            //返回结果：
            // 
            //cmd.officeNo = "SHA243";
            //cmd.ConfigName = "o72fe3b1";
            //cmd.ConfigName = "o72fe411";            
            cmd.request.Pnr = "HP9QFX";
            cmd.request.PassengerType = EtermCommand.PassengerType.Adult;
            cmd.request.Airline = "FM";
            cmd.request.GetPrice = true;

            #endregion

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd);
            CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd, true);
            //CommandResult<JetermEntity.Response.SeekPNR> result = client.Invoke<JetermEntity.Request.SeekPNR, JetermEntity.Response.SeekPNR>(cmd, true);

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
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.InnerDetailedErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}SeekPNR指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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

            //Console.ReadLine();

            #endregion
        }

        // （未测）测试：SeekPNR走非大系统时，当抛出异常时，是否会获得具体的InnerDetailedErrorMessage。
    }
}
