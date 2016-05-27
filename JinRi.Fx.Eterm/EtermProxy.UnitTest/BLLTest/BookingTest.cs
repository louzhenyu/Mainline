using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JetermEntity.Response;
using JetermEntity;
using JetermEntity.Request;
using Newtonsoft.Json;

namespace EtermProxy.UnitTest
{
    [TestClass]
    public class BookingTest
    {
        /*
        所有测试案例：
        针对预订输入命令：
        1、（预订命令在EtermServer中运行过）单人预订
        2、提供返回航班信息，且2人或2人以上预订（预订人名中有英文字符的，且ID类型有JetermEntity.EtermCommand.IDtype.Other的）。
           当遇到预订人的ID类型为JetermEntity.EtermCommand.IDtype.Other时，会出现以【SSR FOID YY HK/NI】开头的命令行
          （有几个预订人的ID类型为JetermEntity.EtermCommand.IDtype.Other，就会出现几个这样的命令行）。
        3、出发航司为HU、出发舱位为A，则会出现以【OSI HU CKIN SSAC/S{0}】开头的那些命令行（有几个预订人，就会出现几行这样的命令行）。
        4、出发航司为PN，则会出现以【OSI PN CABIN/{0}】开头的命令行。
        5、（没有测）当对request.RMKRemark赋了值（赋的值中不包含空格）后，则会出现以【RMK】开头的命令行。
        6、当对request.RMKRemark赋了值（赋的值中包含空格）后，则会出现以【RMK】开头的命令行。
        ============================================================
        针对返回结果：
        7、（只测了以DK开头的）（已测的测试案例7_2：编码状态以DK开头的，其预订命令在EtermServer中运行过）命令返回结果中，编码状态以HK或DK开头的，且包含【-】字符的
        8、（只测了以DK开头的）命令返回结果中，编码状态以HK或DK开头的，且不包含【-】字符的
        9、（未找到测试案例）命令返回结果中，编码状态以KK开头的，出发航司是CZ的，且包含【-】字符的
        10、（未找到测试案例）命令返回结果中，编码状态以KK开头的，出发航司是CZ的，且不包含【-】字符的
        
        11、（未找到测试案例）命令返回结果中，编码状态以KK开头的，出发航司不是CZ的，且包含【-】字符的
        12、（未找到测试案例）命令返回结果中，编码状态以KK开头的，出发航司不是CZ的，且不包含【-】字符的
         
        13、（只测了以NN开头的）（已测的测试案例13_5：编码状态以NN开头的，其预订命令在EtermServer中运行过）命令返回结果中，编码状态以HN或HL或DW或KL或NN开头的，且包含【-】字符的        
        14、（未找到测试案例。找到的测试案例属于测试案例19_5）（只测了以NN开头的）命令返回结果中，编码状态以HN或HL或DW或KL或NN开头的，且不包含【-】字符的
        15、（未找到测试案例）返回err
        16、（只测了以DK开头的--未找到测试案例）返回结果中，编码状态以HK或DK开头的，且包含UNABLE TO
        17、（未找到测试案例）返回结果中，编码状态以KK开头的，出发航司是CZ的，且包含UNABLE TO
        18、（未找到测试案例）返回结果中，编码状态以KK开头的，出发航司不是CZ的，且包含UNABLE TO
        19、（只测了以NN开头的）（测试案例19_5，是最终运行案例）（已测的测试案例19_5：编码状态以NN开头的，其预订命令在EtermServer中运行过）命令返回结果中，编码状态以HN或HL或DW或KL或NN开头的，且包含UNABLE TO
        20、（未找到测试案例）返回结果中包含如下格式的：
        “有【.】这个字符，且【.】之前必须是至少有1位的0到9之间的数字字符”
        相应正则表达式：\d{1,}\.
         */

        // ============================================
        // （必须测）针对预订输入命令的测试案例：

        [TestMethod]
        //1、（预订命令在EtermServer中运行过）单人预订     
        public void Test_BusinessDispose1()
        {

            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"CS003\",  \"OfficeNo\" : \"test\" }";
            string ss = "{\"FlightList\": [{\"FlightNo\":\"MU5137\",\"Cabin\":\"B\",\"SCity\":\"\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1427731200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"朱伟 坚\",\"idtype\":0,\"cardno\":\"440106196510042095\",\"PassType\":0,\"Ename\":\"zhuweijian\",\" BirthDayString\":\"\",\"TicketNo\":\"\"}],\"OfficeNo\":\"KHN117\",\"Mobile\":\"13647125256\",\"RMKOfficeNoList\":[\"SHA777\"],\"RMKRemark\":null,\"Pnr\":null}";

            Proxy p = new Proxy();
            p.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, ss);


            /*
            获得的预订输入命令：
SS: CZ3461/Y/28DEC/CSXCTU/1
NM 1沈燕彬
TKTL1527/09JAN/ＳＨＡ888
SSR FOID CZ HK/NI１２３４５５６/P1
OSI CZ CTCT１２３４４
RMK TJ AUTH ＢＢＢＢ123
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "C   Z   3    461";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2014-12-28");
            //flight.SDate = "2  8DE  C  ";
            flight.SCity = "C  S  X";
            flight.ECity = "C   T  U  ";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "          沈                 燕                   彬               ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "１２３４５５６";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   ＳＨＡ    888        ";

            request.Mobile = "１２３４４         -                     1234556    ";

            request.RMKOfficeNoList.Add("   ＢＢＢｂ   1   2  3   ");

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);
        }

        [TestMethod]
        //2、提供返回航班信息，且2人或2人以上预订（预订人名中有英文字符的，且ID类型有JetermEntity.EtermCommand.IDtype.Other的）。
        //   当遇到预订人的ID类型为JetermEntity.EtermCommand.IDtype.Other时，会出现以【SSR FOID YY HK/NI】开头的命令行
        //  （有几个预订人的ID类型为JetermEntity.EtermCommand.IDtype.Other，就会出现几个这样的命令行）。
        public void Test_BusinessDispose2()
        {
            /*
            获得的预订输入命令：
            SS: CZ380/L/08JAN/PVGCAN/4
SS: CZ680/L/08JAN/CANPVG/4
NM 1BD1CA1万够1王HAI
TKTL1523/09JAN/SHA888
SSR FOID YY HK/NI789012/P1
SSR FOID CZ HK/NI7890123456/P2
SSR FOID CZ HK/NI7890/P3
SSR FOID CZ HK/NI123456/P4
OSI CZ CTCT12341234556
RMK TJ AUTH AAA123
RMK TJ AUTH BBB123
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "C   Z   3    80";
            flight.Cabin = "L";
            flight.DepDate = Convert.ToDateTime("2014-12-28");
            flight.SCity = "P  V  G";
            flight.ECity = "C   A  N  ";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "C   Z   6    80";
            flight.Cabin = "L";
            flight.DepDate = Convert.ToDateTime("2014-12-28");
            flight.SCity = "C   A  N  ";
            flight.ECity = "P  V  G";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            //passenger.name = " 王       and        海     and      爸    ";
            //passenger.name = " 王              海          爸    ";
            passenger.name = "王 hai";
            //passenger.name = "    王        海       ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "123456";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "bd";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "789012";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            //passenger.name = " 王      select               偶  ";
            //passenger.name = " 王                   偶  ";
            passenger.name = "    万    够     ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "7890";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "ca";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "7890123456";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   sha    888        ";

            request.Mobile = "1234-       1234556    ";

            request.RMKOfficeNoList.Add("    AaA   1   2  3   ");
            request.RMKOfficeNoList.Add("  bb   B   1   2  3   ");

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);
        }

        [TestMethod]
        //3、出发航司为HU、出发舱位为A，则会出现以【OSI HU CKIN SSAC/S{0}】开头的那些命令行（有几个预订人，就会出现几行这样的命令行）。
        public void Test_BusinessDispose3()
        {
            /*
            获得的预订输入命令：
            SS: HU380/A/08JAN/PVGCAN/4
NM 1BD1CA1万够1王HAI
TKTL1528/09JAN/SHA888
SSR FOID YY HK/NI789012/P1
SSR FOID HU HK/NI7890123456/P2
SSR FOID HU HK/NI7890/P3
SSR FOID HU HK/NI123456/P4
OSI HU CTCT12341234556
OSI HU CKIN SSAC/S1
OSI HU CKIN SSAC/S2
OSI HU CKIN SSAC/S3
OSI HU CKIN SSAC/S4
RMK TJ AUTH AAA123
RMK TJ AUTH BBB123
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            //request.flight = new List<JetermEntity.Flight>();
            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "H   U   3    80";
            flight.Cabin = "A";
            flight.DepDate = Convert.ToDateTime("2014-12-28");
            flight.SCity = "P  V  G";
            flight.ECity = "C   A  N  ";
            request.FlightList.Add(flight);

            //request.Passengers = new List<JetermEntity.Passenger>();

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            //passenger.name = " 王       and        海     and      爸    ";
            //passenger.name = " 王              海          爸    ";
            passenger.name = "王 hai";
            //passenger.name = "    王        海       ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "123456";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "bd";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "789012";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            //passenger.name = " 王      select               偶  ";
            //passenger.name = " 王                   偶  ";
            passenger.name = "    万    够     ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "7890";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "ca";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "7890123456";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   sha    888        ";

            request.Mobile = "1234-       1234556    ";

            request.RMKOfficeNoList.Add("    AaA   1   2  3   ");
            request.RMKOfficeNoList.Add("  bb   B   1   2  3   ");

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);
        }

        [TestMethod]
        //4、出发航司为PN，则会出现以【OSI PN CABIN/{0}】开头的命令行。
        public void Test_BusinessDispose4()
        {
            /*
            获得的预订输入命令：
            SS: PN380/L/08JAN/PVGCAN/4
NM 1BD1CA1万够1王HAI
TKTL1529/09JAN/SHA888
SSR FOID YY HK/NI789012/P1
SSR FOID PN HK/NI7890123456/P2
SSR FOID PN HK/NI7890/P3
SSR FOID PN HK/NI123456/P4
OSI PN CTCT12341234556
OSI PN CABIN/L
RMK TJ AUTH AAA123
RMK TJ AUTH BBB123
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "P   N   3    80";
            flight.Cabin = "L";
            flight.DepDate = Convert.ToDateTime("2014-12-28");
            flight.SCity = "P  V  G";
            flight.ECity = "C   A  N  ";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            //passenger.name = " 王       and        海     and      爸    ";
            //passenger.name = " 王              海          爸    ";
            passenger.name = "王 hai";
            //passenger.name = "    王        海       ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "123456";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "bd";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "789012";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            //passenger.name = " 王      select               偶  ";
            //passenger.name = " 王                   偶  ";
            passenger.name = "    万    够     ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "7890";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "ca";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "7890123456";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   sha    888        ";

            request.Mobile = "1234-       1234556    ";

            request.RMKOfficeNoList.Add("    AaA   1   2  3   ");
            request.RMKOfficeNoList.Add("  bb   B   1   2  3   ");

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);
        }

        //5、（没有测）当对request.RMKRemark赋了值（赋的值中不包含空格）后，则会出现以【RMK】开头的命令行。

        [TestMethod]
        //6、当对request.RMKRemark赋了值（赋的值中包含空格）后，则会出现以【RMK】开头的命令行。
        public void Test_BusinessDispose6()
        {
            /*
            获得的预订输入命令：
            SS: CZ380/L/08JAN/PVGCAN/4
SS: CZ680/L/08JAN/CANPVG/4
NM 1BD1CA1万够1王HAI
TKTL1530/09JAN/SHA888
SSR FOID YY HK/NI789012/P1
SSR FOID CZ HK/NI7890123456/P2
SSR FOID CZ HK/NI7890/P3
SSR FOID CZ HK/NI123456/P4
OSI CZ CTCT12341234556
RMK TJ AUTH AAA123
RMK TJ AUTH BBB123
RMK 海                                      小
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "C   Z   3    80";
            flight.Cabin = "L";
            flight.DepDate = Convert.ToDateTime("2014-12-28");
            flight.SCity = "P  V  G";
            flight.ECity = "C   A  N  ";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "C   Z   6    80";
            flight.Cabin = "L";
            flight.DepDate = Convert.ToDateTime("2014-01-28");
            flight.SCity = "C   A  N  ";
            flight.ECity = "P  V  G";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            //passenger.name = " 王       and        海     and      爸    ";
            //passenger.name = " 王              海          爸    ";
            passenger.name = "王 hai";
            //passenger.name = "    王        海       ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "123456";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "bd";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "789012";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            //passenger.name = " 王      select               偶  ";
            //passenger.name = " 王                   偶  ";
            passenger.name = "    万    够     ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "7890";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "ca";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "7890123456";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   sha    888        ";

            request.Mobile = "1234-       1234556    ";

            request.RMKOfficeNoList.Add("    AaA   1   2  3   ");
            request.RMKOfficeNoList.Add("  bb   B   1   2  3   ");

            request.RMKRemark = "  海                                      小  ";

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);
        }

        // ============================================
        // 针对返回结果的测试案例：

        // 7_1、（没有测）命令返回结果中，编码状态以HK开头的，且包含【-】字符的

        [TestMethod]
        // 7_2、（必须测）（此测试案例提供的预订命令在EtermServer中运行过）命令返回结果中，编码状态以DK开头的，且包含【-】字符的
        public void Test_BusinessDispose7_2()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中运行过：
            /*
            SS: CZ3461/Y/28DEC/CSXCTU/1
NM 1沈燕彬
TKTL1116/23DEC/SHA888
SSR FOID CZ HK/NI513322197103192519/P1
OSI CZ CTCT 13548746642
RMK TJ AUTH CSX141
\
             */
            /*
             实际获得的预订命令：
           SS: CZ3461/Y/28DEC/CSXCTU/1
NM 1沈燕彬
TKTL1704/09JAN/SHA888
SSR FOID CZ HK/NI513322197103192519/P1
OSI CZ CTCT 13548746642
RMK TJ AUTH CSX141
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "C   Z   3    461";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2014-12-28");
            flight.SCity = "C  S  X";
            flight.ECity = "C   T  U  ";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "          沈                 燕                   彬               ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "513322197103192519";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   SHA    888        ";

            request.Mobile = "13548746642";

            request.RMKOfficeNoList.Add("CSX141");

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);

            if (result == null || result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            if (!result.state)
            {
                if (result.error != null)
                {
                    Console.WriteLine(string.Format("返回失败，失败原因：{0}", result.error.ErrorMessage));
                    //Console.ReadLine();
                    return;
                }

                Console.WriteLine("返回失败");
                //Console.ReadLine();
                return;
            }

            // 预订命令返回结果：
            /*
            KFW1V4 -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  
  CZ3461  Y MO28DEC  CSXCTU DK1   1425 1635                                    
  航空公司使用自动出票时限, 请检查PNR                                                         
  *** 预订酒店指令HC, 详情  HC:HELP   ***
             */
            // 运行结果，如：
            // {"Pnr":"KFW1V4","OfficeNo":null,"PnrState":true}
            Console.WriteLine("返回结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
            //Console.ReadLine();        
        }

        // 8_1、（没有测）命令返回结果中，编码状态以HK开头的，且不包含【-】字符的

        [TestMethod]
        // 8_2、（必须测）命令返回结果中，编码状态以DK开头的，且不包含【-】字符的
        public void Test_BusinessDispose8_2()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // SS: JD5176/X/24SEP/XIYSYX/1  NM 1张路  TKTL1739/31JUL/SHA888  SSR FOID JD HK/NI341281198703102834/P1  OSI JD CTCT15900928386  RMK TJ AUTH AAA123  RMK TJ AUTH BBB123  \
            // 而实际获得的预订命令：
            /*
            SS: JD5176/X/24SEP/XIYSYX/1
NM 1张路
TKTL1711/09JAN/SHA888
SSR FOID JD HK/NI341281198703102834/P1
OSI JD CTCT 15900928386
RMK TJ AUTH AAA123
RMK TJ AUTH BBB123
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "J   d   51    76";
            flight.Cabin = "x";
            flight.DepDate = Convert.ToDateTime("2014-09-24");
            flight.SCity = "X  I  Y";
            flight.ECity = "S   Y  X  ";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张   路    ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "341281198703102834";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   sha    888        ";

            request.Mobile = "     159   00    92     8386    ";

            request.RMKOfficeNoList.Add("    AaA   1   2  3   ");
            request.RMKOfficeNoList.Add("  bb   B   1   2  3   ");

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);

            if (result == null || result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            if (!result.state)
            {
                if (result.error != null)
                {
                    Console.WriteLine(string.Format("返回失败，失败原因：{0}", result.error.ErrorMessage));
                    //Console.ReadLine();
                    return;
                }

                Console.WriteLine("返回失败");
                //Console.ReadLine();
                return;
            }

            // 预订命令返回结果：
            // @"  JD5176  X WE24SEP  XIYSYX DK1   0815 1105  JV9787     *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117]"
            // 运行结果，如：
            // {"Pnr":"JV9787","OfficeNo":null,"PnrState":true}
            Console.WriteLine("返回结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
            //Console.ReadLine();    
        }

        [TestMethod]
        // 9、（必须测）（未找到测试案例）命令返回结果中，编码状态以KK开头的，出发航司是CZ的，且包含【-】字符的
        public void Test_BusinessDispose9()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // 
            // 即：
            /*
            
             */
            // 预订命令返回结果：
            // @""


            // 运行结果，如：
            //
        }

        [TestMethod]
        // 10、（必须测）（未找到测试案例）命令返回结果中，编码状态以KK开头的，出发航司是CZ的，且不包含【-】字符的
        public void Test_BusinessDispose10()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // 
            // 即：
            /*
            
             */
            // 预订命令返回结果：
            // @""


            // 运行结果，如：
        }

        [TestMethod]
        // 11、（必须测）（未找到测试案例）命令返回结果中，编码状态以KK开头的，出发航司不是CZ的，且包含【-】字符的
        public void Test_BusinessDispose11()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // 
            // 即：
            /*
            
             */
            // 预订命令返回结果：
            // @""


            // 运行结果，如：
        }

        [TestMethod]
        // 12、（必须测）（未找到测试案例）命令返回结果中，编码状态以KK开头的，出发航司不是CZ的，且不包含【-】字符的
        public void Test_BusinessDispose12()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // 
            // 即：
            /*
            
             */
            // 预订命令返回结果：
            // @""


            // 运行结果，如：
        }

        // 13_1、（没有测）命令返回结果中，编码状态以HN开头的，且包含【-】字符的
        // 13_2、（没有测）命令返回结果中，编码状态以HL开头的，且包含【-】字符的
        // 13_3、（没有测）命令返回结果中，编码状态以DW开头的，且包含【-】字符的
        // 13_4、（没有测）命令返回结果中，编码状态以KL开头的，且包含【-】字符的

        [TestMethod]
        // 13_5、（必须测）（此测试案例提供的预订命令在EtermServer中运行过）命令返回结果中，编码状态以NN开头的，且包含【-】字符的
        public void Test_BusinessDispose13_5()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中运行过：
            /*
            SS: CZ380/L/08JAN/PVGCAN/2
NM 1缪裕亮1沈云锋
TKTL1045/04JAN/SHA888
SSR FOID CZ HK/NI320511198510020755/P1
SSR FOID CZ HK/NI320503198012191535/P2
OSI CZ CTCT 13812760907
RMK TJ AUTH SHA674
\
             */
            /*
            实际获得的预订命令：
                         SS: CZ380/L/08JAN/PVGCAN/2
NM 1缪裕亮1沈云锋
TKTL1726/09JAN/SHA888
SSR FOID CZ HK/NI320511198510020755/P1
SSR FOID CZ HK/NI320503198012191535/P2
OSI CZ CTCT 13812760907
RMK TJ AUTH SHA674
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "C   Z   38    0";
            flight.Cabin = "L";
            flight.DepDate = Convert.ToDateTime("2015-01-28");
            flight.SCity = "P  V  G";
            flight.ECity = "C   A  N  ";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = " 缪   裕  亮    ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "320511198510020755";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = " 沈   云   锋    ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "320503198012191535";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   sha    888        ";

            request.Mobile = "     13812760907    ";

            request.RMKOfficeNoList.Add("    SHA   6   7  4   ");

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);

            if (result == null || result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            if (!result.state)
            {
                if (result.error != null)
                {
                    Console.WriteLine(string.Format("返回失败，失败原因：{0}", result.error.ErrorMessage));
                    //Console.ReadLine();
                    return;
                }

                Console.WriteLine("返回失败");
                //Console.ReadLine();
                return;
            }

            // 预订命令返回结果：
            /*            
              CZ 380  L TH08JAN  PVGCAN NN2   1245 1525                                    
            KFVN9T -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  
              *** 预订酒店指令HC, 详情  HC:HELP   ***
            */
            // 运行结果，如：
            // {"Pnr":"KFVN9T","OfficeNo":null,"PnrState":false}
            Console.WriteLine("返回结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
            //Console.ReadLine(); 
        }

        // 14_1、（没有测）命令返回结果中，编码状态以HN开头的，且不包含【-】字符的
        // 14_2、（没有测）命令返回结果中，编码状态以HL开头的，且不包含【-】字符的
        // 14_3、（没有测）命令返回结果中，编码状态以DW开头的，且不包含【-】字符的
        // 14_4、（没有测）命令返回结果中，编码状态以KL开头的，且不包含【-】字符的

        [TestMethod]
        // 14_5、（必须测）（未找到测试案例。找到的测试案例属于测试案例19_5）命令返回结果中，编码状态以NN开头的，且不包含【-】字符的
        public void Test_BusinessDispose14_5()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // SS: 3U8900/A/29AUG/KWLSYX/2  NM 1张路1张一扬  TKTL1600/07AUG/SHA888  SSR FOID 3U HK/NI341281198703102834/P1  SSR FOID YY HK/NI 34128198703102834/P2  OSI 3U CTCT15900928386  \
            // 实际获得的预订命令：
            /*
            SS: 3U8900/A/29AUG/KWLSYX/2
NM 1张路1张一扬
TKTL1759/09JAN/SHA888
SSR FOID 3U HK/NI341281198703102834/P1
SSR FOID YY HK/NI34128198703102834/P2
OSI 3U CTCT 15900928386
RMK TJ AUTH AAA123
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "3   U   8    900";
            flight.Cabin = "A";
            flight.DepDate = Convert.ToDateTime("2014-08-28");
            flight.SCity = "K  W  L";
            flight.ECity = "S   Y  X  ";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = " 张   路  ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "341281198703102834";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "    张    一    扬     ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "34128198703102834";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   sha    888        ";

            request.Mobile = "15900928386";

            request.RMKOfficeNoList.Add("    AaA   1   2  3   ");

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);

            if (result == null || result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            if (!result.state)
            {
                if (result.error != null)
                {
                    // 预订命令返回结果：
                    // @"3U  8900 A  29AUG  KWLSYX NN2  UNABLE    UNABLE TO SELL.PLEASE CHECK THE AVAILABILITY WITH "AV" AGAIN  [SHA243]"
                    // 运行结果，如：
                    // ""
                    Console.WriteLine(string.Format("返回失败，失败原因：{0}", result.error.ErrorMessage));
                    //Console.ReadLine();
                    return;
                }

                Console.WriteLine("返回失败");
                //Console.ReadLine();
                return;
            }

            Console.WriteLine("返回结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
            //Console.ReadLine();
        }

        [TestMethod]
        // 15、（必须测）（未找到测试案例）返回err
        public void Test_BusinessDispose15()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // 
            // 即：
            /*
            
             */
            // 预订命令返回结果：
            // @""


            // 运行结果，如：
        }

        // 16_1、（没有测）返回结果中，编码状态以HK开头的，且包含UNABLE TO

        [TestMethod]
        // 16_2、（必须测）（未找到测试案例）返回结果中，编码状态以DK开头的，且包含UNABLE TO
        public void Test_BusinessDispose16_2()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // 
            // 即：
            /*
            
             */
            // 预订命令返回结果：
            // @""


            // 运行结果，如：
        }

        [TestMethod]
        // 17、（必须测）（未找到测试案例）返回结果中，编码状态以KK开头的，出发航司是CZ的，且包含UNABLE TO
        public void Test_BusinessDispose17()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // 
            // 即：
            /*
            
             */
            // 预订命令返回结果：
            // @""


            // 运行结果，如：
        }

        [TestMethod]
        // 18、（必须测）（未找到测试案例）返回结果中，编码状态以KK开头的，出发航司不是CZ的，且包含UNABLE TO 
        public void Test_BusinessDispose18()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // 
            // 即：
            /*
            
             */
            // 预订命令返回结果：
            // @""


            // 运行结果，如：
        }

        // 19_1、（没有测）命令返回结果中，编码状态以HN开头的，且包含UNABLE TO
        // 19_2、（没有测）命令返回结果中，编码状态以HL开头的，且包含UNABLE TO
        // 19_3、（没有测）命令返回结果中，编码状态以DW开头的，且包含UNABLE TO
        // 19_4、（没有测）命令返回结果中，编码状态以KL开头的，且包含UNABLE TO

        [TestMethod]
        // 19_5、（必须测）（最终运行案例）（此测试案例提供的预订命令在EtermServer中运行过）命令返回结果中，编码状态以NN开头的，且包含UNABLE TO
        public void Test_BusinessDispose19_5()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中运行过：
            /*
            SS: SC1170/Q/04JAN/CANTNA/3
NM 1滕秀丽1王春生1王冬生
TKTL1106/12JAN/SHA888
SSR FOID SC HK/NI23100519580903102X/P1
SSR FOID SC HK/NI231002195403202715/P2
SSR FOID SC HK/NI231005195601040015/P3
OSI SC CTCT 13326664826
RMK TJ AUTH BJS415
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "S   C   11    70";
            flight.Cabin = "Q";
            flight.DepDate = Convert.ToDateTime("2015-01-04");
            flight.SCity = "C  A  N";
            flight.ECity = "T   N  A";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "滕秀丽";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "23100519580903102X";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "    王春生     ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "231002195403202715";
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "王冬生";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "231005195601040015";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "   sha    888        ";

            request.Mobile = "133266     64826    ";

            request.RMKOfficeNoList.Add("    BJS415  ");

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);

            if (result == null || result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            if (!result.state)
            {
                if (result.error != null)
                {
                    // 预订命令返回结果：
                    /*
                    SC  1170 Q  04JAN  CANTNA NN3  ENROUTE                                         
        UNABLE TO SELL.PLEASE CHECK THE AVAILABILITY WITH "AV" AGAIN
                     */
                    // 运行结果，如：
                    // 
                    Console.WriteLine(string.Format("返回失败，失败原因：{0}", result.error.ErrorMessage));
                    //Console.ReadLine();
                    return;
                }

                Console.WriteLine("返回失败");
                //Console.ReadLine();
                return;
            }

            Console.WriteLine("返回结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
            //Console.ReadLine(); 
        }

        [TestMethod]
        // 20、（必须测）（未找到测试案例）返回结果中包含如下格式的：
        // “有【.】这个字符，且【.】之前必须是至少有1位的0到9之间的数字字符”
        // 相应正则表达式：\d{1,}\.   
        public void Test_BusinessDispose20()
        {
            // 预订命令：
            // 以下预订命令在EtermServer中没运行过：
            // 
            // 即：
            /*
            
             */
            // 预订命令返回结果：
            // @""


            // 运行结果，如：
        }

        [TestMethod]
        //1、（预订命令在EtermServer中运行过）单人预订     
        public void Test_BusinessDispose21()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "CZ3461";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2014-12-28");
            flight.SCity = "CSX";
            flight.ECity = "CTU";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "沈燕彬               ";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "123456";
            request.PassengerList.Add(passenger);

            request.OfficeNo = "SHA999";

            request.Mobile = "12484852484856";

            request.RMKOfficeNoList.Add("GBWEFE232");

            string s = "http://192.168.5.165:15252/format=json&language=CSharp&method=Booking";
            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            IntPtr ptr = IntPtr.Zero;
            //string str = proxy.InvokeEterm(ptr, ptr, "test", string.Empty, s, Newtonsoft.Json.JsonConvert.SerializeObject(request));
        }

        // 2015-02-28（星期六）为王桥调试
        [TestMethod]
        public void Test_BusinessDispose_80()
        {
            /*
            预订命令1：
            SS: CA4309/E/28JAN/CTUCAN/1
NM 1朱伟坚
TKTL1517/28FEB/KHN117
SSR FOID CA HK/NI440106196510042095/P1
OSI CA CTCT 13647125256
RMK TJ AUTH KHN117
RMK test
\
             对应：
          system("SS: CA1589/F/28FEB/PEKSHA/1[RN]NM 1朱伟坚[RN]TKTL1851/28FEB/KHN117[RN]SSR FOID CA HK/NI440106196510042095/P1[RN]OSI CA CTCT 13647125256[RN]RMK TJ AUTH KHN117[RN]RMK test[RN]\");
return DATAS;             
             
            预订命令2：
            SS: CA1519/Y/18MAR/PEKSHA/1
NM 1朱伟坚
TKTL1709/28FEB/KHN117
SSR FOID CA HK/NI440106196510042095/P1
OSI CA CTCT 13647125256
RMK TJ AUTH KHN117
RMK test
\
        
             对应：
system("SS: CA1519/Y/18MAR/PEKSHA/1[RN]NM 1朱伟坚[RN]TKTL1852/28FEB/KHN117[RN]SSR FOID CA HK/NI440106196510042095/P1[RN]OSI CA CTCT 13647125256[RN]RMK TJ AUTH KHN117[RN]RMK test[RN]\");
return DATAS;
             */
            /*
            返回1：
            CA  4309 E  28JAN  CTUCAN NN1  UNABLE                                          
UNABLE TO SELL.PLEASE CHECK THE AVAILABILITY WITH "AV" AGAIN   
            返回2：
             HQTVDP -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  
  CA1519  Y WE18MAR  PEKSHA DK1   0930 1135                                    
  航空公司使用自动出票时限, 请检查PNR                                                         
  *** 预订酒店指令HC, 详情  HC:HELP   ***  
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();

            //flight.FlightNo = "CA1519";
            //flight.Cabin = "Y";
            //flight.SDate = "18MAR";
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";

            //flight.FlightNo = "CA1858";
            //flight.Cabin = "B";
            //flight.DepDate = Convert.ToDateTime("2015-03-23");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";

            //flight.FlightNo = "MU5137";
            //flight.Cabin = "H";
            //flight.DepDate = Convert.ToDateTime("2015-03-18");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";

            //flight.FlightNo = "CA1858";
            //flight.Cabin = "L";
            //flight.DepDate = Convert.ToDateTime("2015-03-08");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";

            //flight.FlightNo = "MU5137";
            //flight.Cabin = "S";
            //flight.DepDate = Convert.ToDateTime("2015-04-06");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";

            //flight.FlightNo = "MU5183";
            //flight.Cabin = "Y";
            //flight.DepDate = Convert.ToDateTime("2015-03-10");
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";

            //flight.FlightNo = "MU5183";
            //flight.Cabin = "Y";
            //flight.DepDate = Convert.ToDateTime("2015-03-10");
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";

            //flight.FlightNo = "MU5183";
            //flight.Cabin = "Y";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-04-06");
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";

            //flight.FlightNo = "MU5109";
            //flight.Cabin = "Y";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-04-08");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";

            //flight.FlightNo = "MU5137";
            //flight.Cabin = "P";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-03-31");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";

            //flight.FlightNo = "MU5137";
            //flight.Cabin = "B";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-03-27");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";

            //request.FlightList.Add(flight);

            //flight.FlightNo = "MU5183";
            //flight.Cabin = "L";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-03-21");
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";
            //flight.FlightNo = "KN5988";
            //flight.Cabin = "V";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-03-30");
            //flight.SCity = "PVG";
            //flight.ECity = "NAY";
            flight.FlightNo = "MU5341";
            flight.Cabin = "V";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-03-30");
            flight.SCity = "PVG";
            flight.ECity = "NAY";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            //flight.FlightNo = "MU5127";
            //flight.Cabin = "M";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-03-23");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";
            flight.FlightNo = "KN5977";
            flight.Cabin = "R";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-04-02");
            flight.SCity = "NAY";
            flight.ECity = "PVG";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            //passenger.name = "朱伟坚";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "440106196510042095";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            passenger.name = "张路";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "341281198703102834";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            //JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            //passenger.name = "张业华";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "310107198812044435";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhangyehua";
            //request.PassengerList.Add(passenger);

            //passenger = new JetermEntity.Passenger();
            //passenger.name = "杜俊强";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "310107198312044435";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "dujunqiang";
            //request.PassengerList.Add(passenger);

            //request.OfficeNo = "KHN117";
            request.OfficeNo = "SHA888";

            //request.Mobile = "13647125256";
            //request.Mobile = "15021977488";
            request.Mobile = "15192781928";

            //request.RMKOfficeNoList.Add("KHN117");
            //request.RMKOfficeNoList.Add("SHA777");
            request.RMKOfficeNoList.Add("PEK513");
            request.RMKOfficeNoList.Add("PEK277");

            //request.RMKRemark = "test";

            //request.Pnr = "JNL22B";

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                Console.WriteLine(string.Format("返回有错误，错误信息为{0}", result.error.ErrorMessage));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            Console.WriteLine("运行结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
        }

        // 2015-05-06（星期三），为王桥查找获取不到大编码号的原因
        // 运行结果：可以获取到大编码号
        [TestMethod]
        public void Test_EtermProxy_1000()
        {            
            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"o72fd431\",  \"OfficeNo\" : \"SHA243\" }";
            //string ss = "{\"FlightList\":[{\"FlightNo\":\"MU5137\",\"Cabin\":\"H\",\"SCity\":\"SHA\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1430064000000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"MU5156\",\"Cabin\":\"B\",\"SCity\":\"PEK\",\"ECity\":\"SHA\",\"DepDate\":\"\\/Date(1430323200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"干园\",\"idtype\":0,\"cardno\":\"650121199412242866\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"},{\"name\":\"张杰\",\"idtype\":0,\"cardno\":\"140525198401186312\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"13472634765\",\"RMKOfficeNoList\":[],\"RMKRemark\":null,\"Pnr\":null}";
            string ss = "{\"FlightList\":[{\"FlightNo\":\"CZ6178\",\"Airline\":\"\",\"Cabin\":\"Y\",\"SCity\":\"CGQ\",\"ECity\":\"CSX\",\"DepTerminal\":null,\"ArrTerminal\":null,\"DepDate\":\"\\/Date(1435075200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"CZ3937\",\"Airline\":\"\",\"Cabin\":\"M\",\"SCity\":\"CSX\",\"ECity\":\"CGQ\",\"DepTerminal\":null,\"ArrTerminal\":null,\"DepDate\":\"\\/Date(1435161600000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"张龙\",\"idtype\":0,\"cardno\":\"610103197010032517\",\"PassType\":0,\"Ename\":\"\",\"BabyBirthday\":\"\\/Date(-62135596800000+0800)\\/\",\"ChildBirthday\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"18101810679\",\"RMKOfficeNoList\":[\"CGQ203\"],\"RMKRemark\":null,\"Pnr\":null}";

            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：{"state":true,"error":null,"config":"o72fd431","OfficeNo":"SHA243","result":{"Pnr":"JVL94L","OfficeNo":"SHA243","BookingState":0,"BigPNR":"NWZJTC","Command":"SS: CZ6178/Y/24JUN/CGQCSX/1\r\nSS: CZ3937/M/25JUN/CSXCGQ/1\r\nNM 1张龙\r\nTKTL1636/06MAY/SHA888\r\nSSR FOID CZ HK/NI610103197010032517/P1\r\nOSI CZ CTCT 18101810679\r\nRMK TJ AUTH CGQ203\r\n\\","ResultBag":"JVL94L -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  CZ6178  Y WE24JUN  CGQCSX DK1   1340 1830                                    \r  CZ3937  M TH25JUN  CSXCGQ DK1   1340 1840                                    \r 『航空公司使用自动出票时限, 请检查PNR』                                                        \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1430897792270+0800)\/","SaveTime":1800}
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, ss);
            Console.WriteLine("返回结果：" + sret);
        }

        // 2015-05-07（星期四），测试是否能返回正确的PNR
        [TestMethod]
        public void Test_ParseCmdResult_1000()
        {
            string cmdResult1 =
@"
JD5766 X SU28JUN CGQCSX DK1 1445 1810 
HNME4P 
 *** 预订酒店指令HC, 详情 ?HC:HELP *** 
";

            string cmdResult2 =
@"
JD5766 X SU28JUN CGQCSX DK1 1445 1810 &#xD;HNME4P &#xD; *** 预订酒店指令HC, 详情 ?HC:HELP *** &#xD;
";

            string cmdResult3 =
@"
JD5766 U FR26JUN CGQCSX DK1 1445 1810 JSRKW1 *** 预订酒店指令HC, 详情 ?HC:HELP *** []
";

            //string cmdResult4 =
            string cmdResult =
@"
  MU5183  C SA20JUN  PEKPVG NN1   0735 0950                                     HQV2PV                                                                            *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                               [SHA243]
";

            string cmdResult5 =
@"
  HO1252  Y WE15JUL  PEKSHA DK1   0650 0905  HYQKXZ -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS      *** 预订酒店指令HC, 详情  HC:HELP   ***    [SHA243]
";

            string cmdResult6 =
@"
  KX1M3Y -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                     CZ6412  M SA09MAY  PEKSHA DK1   0635 0840                                       CZ3908  P TH14MAY  SHAPEK DK1   1150 1415     P1                               『航空公司使用自动出票时限, 请检查PNR』                                                           *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                               [KHN117]
";

            JetermEntity.Parser.Booking parserTool = new JetermEntity.Parser.Booking();
            CommandResult<JetermEntity.Response.Booking> bookParseResult = parserTool.ParseCmdResult(cmdResult);
            if (bookParseResult != null)
            {
                Console.WriteLine("解析结果为：" + Newtonsoft.Json.JsonConvert.SerializeObject(bookParseResult));
            }

            return;
        }

        //===============added by Li Yang, April 11th, 2015================================
        // 测试验证请求参数，主要包括以下方面：
        // request.FlightList    
        [TestMethod]
        public void Test_BusinessDispose_Booking1_1()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            //flight.FlightNo = "KN5988";
            flight.FlightNo = "K";
            //flight.Cabin = "V";
            //flight.Cabin = "V";    
            flight.DepDate = Convert.ToDateTime("2015-03-30");
            flight.SCity = "PVG";
            flight.ECity = "NAY";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "KN5977";
            flight.Cabin = "R";
            //flight.DepDate = Convert.ToDateTime("2015-04-02");
            //flight.DepDate = Convert.ToDateTime("2015-04-02");
            flight.SCity = "NAY";
            flight.ECity = "PVG";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张路";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "341281198703102834";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "徐莹莹";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "612732198903210342";
            passenger.PassType = EtermCommand.PassengerType.Children;
            //passenger.ChildBirthDayDate = Convert.ToDateTime("2012-07-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "林秀 CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "20110201";
            passenger.cardno = "19900201";
            passenger.PassType = EtermCommand.PassengerType.Children;
            //passenger.ChildBirthDayDate = Convert.ToDateTime("2011-02-01");
            passenger.ChildBirthday = Convert.ToDateTime("1990-02-01");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "杨林 CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            //request.OfficeNo = "KHN117";
            request.OfficeNo = "SHA888";

            request.Mobile = "15192781928";

            //request.RMKOfficeNoList.Add("KHN117");
            //request.RMKOfficeNoList.Add("SHA777");
            request.RMKOfficeNoList.Add("PEK513");
            request.RMKOfficeNoList.Add("PEK277");

            //request.Pnr = "JNL22B";
            //request.Pnr = "HF4X80"; 

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult = result.error.CmdResultBag;
                //Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult) ? string.Empty : string.Format("，订位指令返回结果为：{0}{1}", Environment.NewLine, cmdResult)));
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            Console.WriteLine("解析结果为：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
        }

        // 测试验证请求参数，主要包括以下方面：     
        // request.PassengerList      
        [TestMethod]
        public void Test_BusinessDispose_Booking1_2()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "KN5988";
            //flight.FlightNo = "K";
            //flight.Cabin = "V";
            flight.Cabin = "V";    
            flight.DepDate = Convert.ToDateTime("2015-03-30");
            flight.SCity = "PVG";
            flight.ECity = "NAY";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "KN5977";
            flight.Cabin = "R";
            //flight.DepDate = Convert.ToDateTime("2015-04-02");
            flight.DepDate = Convert.ToDateTime("2015-04-02");
            flight.SCity = "NAY";
            flight.ECity = "PVG";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张路";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "341281198703102834";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "徐莹莹";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "612732198903210342";
            passenger.PassType = EtermCommand.PassengerType.Children;
            //passenger.ChildBirthDayDate = Convert.ToDateTime("2012-07-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            //passenger.name = "林秀 CHD";
            passenger.name = "林秀 a";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "20110201";
            passenger.cardno = "19900201";
            passenger.PassType = EtermCommand.PassengerType.Children;
            //passenger.ChildBirthDayDate = Convert.ToDateTime("2011-02-01");
            passenger.ChildBirthday = Convert.ToDateTime("1990-02-01");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "杨林 CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            //request.OfficeNo = "KHN117";
            request.OfficeNo = "SHA888";

            request.Mobile = "15192781928";

            //request.RMKOfficeNoList.Add("KHN117");
            //request.RMKOfficeNoList.Add("SHA777");
            request.RMKOfficeNoList.Add("PEK513");
            request.RMKOfficeNoList.Add("PEK277");

            //request.Pnr = "JNL22B";
            //request.Pnr = "HF4X80"; 

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult = result.error.CmdResultBag;
                //Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult) ? string.Empty : string.Format("，订位指令返回结果为：{0}{1}", Environment.NewLine, cmdResult)));
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            Console.WriteLine("解析结果为：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
        }

        // 测试验证请求参数，主要包括以下方面：
        // request.Pnr
        [TestMethod]
        public void Test_BusinessDispose_Booking1_3()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "KN5988";
            //flight.FlightNo = "K";
            //flight.Cabin = "V";
            flight.Cabin = "V";
            flight.DepDate = Convert.ToDateTime("2015-03-30");
            flight.SCity = "PVG";
            flight.ECity = "NAY";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "KN5977";
            flight.Cabin = "R";
            //flight.DepDate = Convert.ToDateTime("2015-04-02");
            flight.DepDate = Convert.ToDateTime("2015-04-02");
            flight.SCity = "NAY";
            flight.ECity = "PVG";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张路";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "341281198703102834";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "徐莹莹";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "612732198903210342";
            passenger.PassType = EtermCommand.PassengerType.Children;
            //passenger.ChildBirthDayDate = Convert.ToDateTime("2012-07-10");
            passenger.ChildBirthday = Convert.ToDateTime("2012-07-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            //passenger.name = "林秀 CHD";
            passenger.name = "林秀 a";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "20110201";
            passenger.cardno = "19900201";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2011-02-01");
            //passenger.ChildBirthDayDate = Convert.ToDateTime("1990-02-01");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "杨林 CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            //request.OfficeNo = "KHN117";
            request.OfficeNo = "SHA888";

            request.Mobile = "15192781928";

            //request.RMKOfficeNoList.Add("KHN117");
            //request.RMKOfficeNoList.Add("SHA777");
            request.RMKOfficeNoList.Add("PEK513");
            request.RMKOfficeNoList.Add("PEK277");

            //request.Pnr = "JNL22B";
            //request.Pnr = "HF4X80"; 

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(request);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult = result.error.CmdResultBag;
                //Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult) ? string.Empty : string.Format("，订位指令返回结果为：{0}{1}", Environment.NewLine, cmdResult)));
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            Console.WriteLine("解析结果为：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
        }

        // 测试返回结果的正确性（验证没通过的测试案例）
        [TestMethod]
        public void Test_BusinessDispose_Booking1_4()
        {
            string cmdResult = @"123";

            JetermEntity.Parser.Booking booking = new JetermEntity.Parser.Booking();

            CommandResult<JetermEntity.Response.Booking>  result = new CommandResult<JetermEntity.Response.Booking>();
            result.config = string.Empty;
            result.OfficeNo = string.Empty;

            result.result = new JetermEntity.Response.Booking();
            result.result.OfficeNo = string.Empty;

            result = booking.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
                //Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }

            Console.WriteLine("解析结果为：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
        }

        // 测试返回结果的正确性（验证通过的测试案例）
        /*
        解析结果：
        {"Pnr":"HRW13L","OfficeNo":"","PnrState":false,"BigPNR":null,"WaitState":true,"Command":null,"ResultBag":"CZ6269  E SA31JAN  HRBKMG NN1   0935 1605  HRW13L -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS      *** 预订酒店指令HC, 详情  \u001eHC:HELP   ***   \u001e [KHN117]"}
         */
        [TestMethod]
        public void Test_BusinessDispose_Booking1_5()
        {
            string cmdResult = @"CZ6269  E SA31JAN  HRBKMG NN1   0935 1605  HRW13L -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS      *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117]";

            JetermEntity.Parser.Booking booking = new JetermEntity.Parser.Booking();

            CommandResult<JetermEntity.Response.Booking> result = new CommandResult<JetermEntity.Response.Booking>();
            result.config = string.Empty;
            result.OfficeNo = string.Empty;

            result.result = new JetermEntity.Response.Booking();
            result.result.OfficeNo = string.Empty;

            result = booking.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            Console.WriteLine("解析结果为：" + parseResult);
        }

        /*
        针对第2种返回结果进行测试：
        SS指令：
SS: JD5169/X/06DEC/CANHGH/1  
NM 1张亮  
TKTL1657/04DEC/SHA888  
SSR FOID JD HK/NI612401198603061479/P1  
OSI JD CTCT13203799010  
RMK TJ AUTH SHA333  
RMK TJ AUTH SHA123  
\

返回结果：
JD5169  X SA06DEC  CANHGH DK1   0735 0925  JSKQSX     *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117]  
         */
        [TestMethod]
        public void Test_BusinessDispose_Booking2()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "JD5169";         
            flight.Cabin = "X";
            flight.DepDate = Convert.ToDateTime("2014-12-06");
            flight.SCity = "CAN";
            flight.ECity = "HGH";
            request.FlightList.Add(flight);           

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张亮";      
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "612401198603061479";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);
           
            request.OfficeNo = "SHA888";

            request.Mobile = "13203799010";
          
            request.RMKOfficeNoList.Add("SHA333");
            request.RMKOfficeNoList.Add("SHA123");

            //request.Pnr = "JNL22B";
            //request.Pnr = "HF4X80"; 

            JetermEntity.Parser.Booking booking = new JetermEntity.Parser.Booking();

            CommandResult<JetermEntity.Response.Booking> result = new CommandResult<JetermEntity.Response.Booking>();
            result.config = string.Empty;
            result.OfficeNo = string.Empty;

            result.result = new JetermEntity.Response.Booking();
            result.result.OfficeNo = string.Empty;

            string cmd = booking.ParseCmd(request);

            string cmdResult =
@"
JD5169  X SA06DEC  CANHGH DK1   0735 0925  JSKQSX     *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117]
";

            result = booking.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            Console.WriteLine("生成的订位指令为：" + Environment.NewLine + cmd + Environment.NewLine + "解析结果为：" + Environment.NewLine + parseResult);
        }

        /*
针对第1种返回结果进行测试：
SS: CZ3907/J/27MAR/PEKSHA/1  
NM 1张路  
TKTL1113/18MAR/KHN117  
SSR FOID CZ HK/NI341281198703102834/P1  
OSI CZ CTCT 15900928386  
RMK TJ AUTH CZZ444  
\

返回结果：
CZ3907  J FR27MAR  PEKSHA DK1   1750 2010  HTXPL9 -   航空公司使用自动出票时限, 请检查PNR     *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117] 
         */
        [TestMethod]
        public void Test_BusinessDispose_Booking3()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "CZ3907";
            flight.Cabin = "J";
            flight.DepDate = Convert.ToDateTime("2015-03-27");
            flight.SCity = "PEK";
            flight.ECity = "SHA";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张路";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "341281198703102834";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            request.OfficeNo = "KHN117";

            request.Mobile = "15900928386";

            request.RMKOfficeNoList.Add("CZZ444");            

            //request.Pnr = "JNL22B";
            //request.Pnr = "HF4X80"; 

            JetermEntity.Parser.Booking booking = new JetermEntity.Parser.Booking();

            CommandResult<JetermEntity.Response.Booking> result = new CommandResult<JetermEntity.Response.Booking>();
            result.config = string.Empty;
            result.OfficeNo = string.Empty;

            result.result = new JetermEntity.Response.Booking();
            result.result.OfficeNo = string.Empty;

            string cmd = booking.ParseCmd(request);

            string cmdResult =
@"CZ3907  J FR27MAR  PEKSHA DK1   1750 2010  HTXPL9 -   航空公司使用自动出票时限, 请检查PNR     *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117]";

            result = booking.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            Console.WriteLine("生成的订位指令为：" + Environment.NewLine + cmd + Environment.NewLine + "解析结果为：" + Environment.NewLine + parseResult);
        }

        /*
（没有测）
针对第1种返回结果进行测试：
SS: PN6259/X/25APR/CKGCSX/1  
NM 1张经时  
TKTL1722/18MAR/KHN117  
SSR FOID PN HK/NI342401196107140018/P1  
OSI PN CTCT15021977488  
OSI PN CABIN/X1  
RMK TJ AUTH SHA111  
\

返回结果：
PN6259  X SA25APR  CKGCSX DK1   0705 0815     X1   HVL06W -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS      *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117]
         */
        [TestMethod]
        public void Test_BusinessDispose_Booking4()
        {

        }

        // 测试等待订位标识返回的正确性
        /*
SS指令：
SS: CZ6269/E/31JAN/HRBKMG/1  
NM 1柴慧君  
TKTL1533/01DEC/SHA888  
SSR FOID YY HK/NI1234554321/P1  
OSI CZ CTCT 15900928386  
RMK TJ AUTH SHA666  
\
            
返回结果：
  CZ6269  E SA31JAN  HRBKMG NN1   0935 1605  HRW13L -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS      *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117]
 */
        [TestMethod]
        public void Test_BusinessDispose_Booking5()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "CZ6269";
            flight.Cabin = "E";
            flight.DepDate = Convert.ToDateTime("2015-01-31");
            flight.SCity = "HRB";
            flight.ECity = "KMG";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "柴慧君";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "1234554321";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            request.OfficeNo = "SHA888";

            request.Mobile = "15900928386";

            request.RMKOfficeNoList.Add("SHA666");

            //request.Pnr = "JNL22B";
            //request.Pnr = "HF4X80"; 

            JetermEntity.Parser.Booking booking = new JetermEntity.Parser.Booking();

            CommandResult<JetermEntity.Response.Booking> result = new CommandResult<JetermEntity.Response.Booking>();
            result.config = string.Empty;
            result.OfficeNo = string.Empty;

            result.result = new JetermEntity.Response.Booking();
            result.result.OfficeNo = string.Empty;

            string cmd = booking.ParseCmd(request);

            string cmdResult =
@"CZ6269  E SA31JAN  HRBKMG NN1   0935 1605  HRW13L -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS      *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117]";

            result = booking.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            Console.WriteLine("生成的订位指令为：" + Environment.NewLine + cmd + Environment.NewLine + "解析结果为：" + Environment.NewLine + parseResult);
        }

        /*
SQL语句：
SELECT  * 
FROM jinri3.dbo.tblEtermLog WITH(NOLOCK)
WHERE Cmd LIKE '%CHD%'
ORDER BY id DESC
         
找到的测试案例：
SS: MU5110/Y/25JUL/PEKSHA/1  
NM 1徐莹莹 CHD  
TKTL1137/03APR/BJS579  
SSR FOID MU HK/NI612732198903210342/P1  
SSR OTHS MU ADULT PNR IS HF4X80  
SSR CHLD MU HK1/10JUL12/P1  
OSI MU CTCT15021977488  
\

SS: GS6661/Y/01APR/TSNSHA/1  
NM 1张亮 CHD  
TKTL1545/31MAR/BJS579  
SSR FOID GS HK/NI20101010/P1  
SSR OTHS GS ADULT PNR IS KPKDSV  
SSR CHLD GS HK1/10OCT10/P1  
OSI GS CTCT15021977488  
\

SS: CZ6657/Y/01JUN/PEKHRB/1  
NM 1林秀 CHD  
TKTL1746/30JAN/BJS579  
SSR FOID CZ HK/NI20110201/P1  
SSR OTHS CZ ADULT PNR IS JQR8SS  
SSR CHLD CZ HK1/01FEB11/P1  
OSI CZ CTCT 13645201429  
\


SS: CZ3938/Y/08APR/CGQCSX/1  
NM 1杨林 CHD  
TKTL1845/13JAN/BJS579  
SSR FOID CZ HK/NI20101010/P1  
SSR OTHS CZ ADULT PNR IS HMNQWB  
SSR CHLD CZ HK1/10OCT10/P1  
OSI CZ CTCT 13764246613  
\

SS: CZ3938/Y/08APR/CGQCSX/1  
NM 1孙龙 CHD  
TKTL1842/13JAN/BJS579  
SSR FOID CZ HK/NI20101010/P1  
SSR OTHS CZ ADULT PNR IS HMNQWB  
SSR CHLD CZ HK1/10OCT10/P1  
OSI CZ CTCT 13764246613  
\
         */

        // （没测）测试单人儿童订位（单程）指令生成
        [TestMethod]
        public void Test_BusinessDispose_Booking6()
        {

        }

        // 测试多人儿童订位（往返）指令生成
        /*
        生成的结果：
SS: CZ6657/Y/01JUN/PEKHRB/13
SS: CZ7657/M/07JUN/HRBPEK/13
NM 1林秀 CHD1孙龙 CHD1徐莹莹 CHD1杨林 CHD1张亮 CHD1张亮1 CHD1张亮2 CHD
-1张亮3 CHD1张亮4 CHD1张亮5 CHD1张亮6 CHD1张亮7 CHD1张亮8 CHD
TKTL1840/11APR/BJS579
SSR FOID CZ HK/NI20110201/P1
SSR FOID CZ HK/NI20101010/P2
SSR FOID CZ HK/NI612732198903210342/P3
SSR FOID CZ HK/NI20101010/P4
SSR FOID CZ HK/NI20101010/P5
SSR FOID CZ HK/NI20101010/P6
SSR FOID CZ HK/NI20101010/P7
SSR FOID CZ HK/NI20101010/P8
SSR FOID CZ HK/NI20101010/P9
SSR FOID CZ HK/NI20101010/P10
SSR FOID CZ HK/NI20101010/P11
SSR FOID CZ HK/NI20101010/P12
SSR FOID CZ HK/NI20101010/P13
SSR OTHS CZ ADULT PNR IS HF4X80
SSR CHLD CZ HK1/01FEB11/P1
SSR CHLD CZ HK1/10OCT10/P2
SSR CHLD CZ HK1/10JUL12/P3
SSR CHLD CZ HK1/10OCT10/P4
SSR CHLD CZ HK1/10OCT10/P5
SSR CHLD CZ HK1/10OCT10/P6
SSR CHLD CZ HK1/10OCT10/P7
SSR CHLD CZ HK1/10OCT10/P8
SSR CHLD CZ HK1/10OCT10/P9
SSR CHLD CZ HK1/10OCT10/P10
SSR CHLD CZ HK1/10OCT10/P11
SSR CHLD CZ HK1/10OCT10/P12
SSR CHLD CZ HK1/10OCT10/P13
OSI CZ CTCT 15021977488
RMK TJ AUTH SHA666
\
         */
        [TestMethod]
        public void Test_BusinessDispose_Booking7()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "CZ6657";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2014-06-01");
            flight.SCity = "PEK";
            flight.ECity = "HRB";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "CZ7657";
            flight.Cabin = "M";
            flight.DepDate = Convert.ToDateTime("2014-06-07");
            flight.SCity = "HRB";
            flight.ECity = "PEK";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "林秀CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20110201";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2011-02-01");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "徐莹莹CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "612732198903210342";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2012-07-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "孙龙CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "杨林CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "张亮CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "张亮1CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "张亮2CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "张亮3CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "张亮4CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "张亮5CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "张亮6CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "张亮7CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "张亮8CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20101010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2010-10-10");
            request.PassengerList.Add(passenger);

            request.OfficeNo = "BJS579";

            request.Mobile = "15021977488";

            request.RMKOfficeNoList.Add("SHA666");

            //request.Pnr = "JNL22B";
            request.Pnr = "HF4X80"; 

            JetermEntity.Parser.Booking booking = new JetermEntity.Parser.Booking();

            CommandResult<JetermEntity.Response.Booking> result = new CommandResult<JetermEntity.Response.Booking>();
            result.config = string.Empty;
            result.OfficeNo = string.Empty;

            result.result = new JetermEntity.Response.Booking();
            result.result.OfficeNo = string.Empty;

            string cmd = booking.ParseCmd(request);

            Console.WriteLine("生成的订位指令为：" + Environment.NewLine + cmd);
        }

        // （没测）测试单人儿童订位（往返）

        // （没测）测试BusinessDispose

        // 测试具有3个或3个以上航段的联程票指令生成
        [TestMethod]
        public void Test_ParseCmd()
        {
            // 返回SS指令：
            /*
SS: PN6253/Y/25JUN/SHAPEK/3
SS: PN6205/Y/30JUN/PEKHRB/3
SS: PN6319/Y/05JUL/HRBTSN/3
NM 1陈晓庆1张亮1ZI/XING
TKTL2117/26MAY/SHA888
SSR FOID PN HK/NI320911198911224646/P1
SSR FOID PN HK/NI612401198603061479/P2
SSR FOID PN HK/NI320681198610238019/P3
OSI PN CTCT 13203799010
OSI PN CABIN/Y/Y/Y1
RMK TJ AUTH SHA333
RMK TJ AUTH SHA123
\
             */
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "PN6253";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-25");
            flight.SCity = "SHA";
            flight.ECity = "PEK";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "PN6205";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-30");
            flight.SCity = "PEK";
            flight.ECity = "HRB";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "PN6319";
            flight.Cabin = "Y1";
            flight.DepDate = Convert.ToDateTime("2015-07-05");
            flight.SCity = "HRB";
            flight.ECity = "TSN";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张亮";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "612401198603061479";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "ZI/XING";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "320681198610238019";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "陈晓庆";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "320911198911224646";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            request.OfficeNo = "SHA888";

            request.Mobile = "13203799010";

            request.RMKOfficeNoList.Add("SHA333");
            request.RMKOfficeNoList.Add("SHA123");

            //request.Pnr = "JNL22B";
            //request.Pnr = "HF4X80"; 

            JetermEntity.Parser.Booking booking = new JetermEntity.Parser.Booking();

            CommandResult<JetermEntity.Response.Booking> result = new CommandResult<JetermEntity.Response.Booking>();
            result.config = string.Empty;
            result.OfficeNo = string.Empty;

            result.result = new JetermEntity.Response.Booking();
            result.result.OfficeNo = string.Empty;

            string cmd = booking.ParseCmd(request);
            Console.WriteLine("生成的订位SS指令为：" + cmd);
            return;

            string cmdResult =
@"
JD5169  X SA06DEC  CANHGH DK1   0735 0925  JSKQSX     *** 预订酒店指令HC, 详情  HC:HELP   ***    [KHN117]
";

            result = booking.ParseCmdResult(cmdResult);

            if (result == null)
            {
                Console.WriteLine("没有返回结果");
                //Console.ReadLine();
                return;
            }
            if (!result.state)
            {
                string cmdResult2 = result.error.CmdResultBag;
                Console.WriteLine(string.Format("返回有错误，错误信息为：{0}{1}。{2}", Environment.NewLine, result.error.ErrorMessage, string.IsNullOrWhiteSpace(cmdResult2) ? string.Empty : string.Format("{0}订位指令返回结果为：{1}{2}", Environment.NewLine, Environment.NewLine, cmdResult2)));
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
            Console.WriteLine("生成的订位指令为：" + Environment.NewLine + cmd + Environment.NewLine + "解析结果为：" + Environment.NewLine + parseResult);
        }

        // 测试儿童单程票订位（PN航司）       

        // 测试儿童往返票订位（PN航司）

        // 测试儿童联程票订位（PN航司）

        // 测试儿童单程票订位（非PN航司）

        // 测试儿童往返票订位（非PN航司）
        [TestMethod]
        public void Test_Proxy103()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "CA1858";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-02");
            flight.SCity = "SHA";
            flight.ECity = "PEK";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "CA1603";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-05");
            flight.SCity = "PEK";
            flight.ECity = "HRB";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张越";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "20081010";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthday = Convert.ToDateTime("2008-10-10");
            request.PassengerList.Add(passenger);            

            request.OfficeNo = "SHA888";

            request.Mobile = "13203799010";

            request.RMKOfficeNoList.Add("SHA333");
            request.RMKOfficeNoList.Add("SHA123");

            request.Pnr = "KNEY6X";

            string strParams = JsonConvert.SerializeObject(request);
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"\",  \"OfficeNo\" : \"SHA243\" }";
            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：
#warning will check：为什么会返回PNR CANCELLED，2015-05-27订的位置--2015-05-28(4)，志勇已修复
            // {"state":false,"error":{"ErrorCode":19,"ErrorMessage":"很抱歉，订位失败，请重新预订","CmdResultBag":"PNR CANCELLED                                                                  \r"},"config":"","OfficeNo":"SHA243","result":{"Pnr":null,"OfficeNo":"SHA243","BookingState":1,"BigPNR":null,"Command":"SS: CA1858/Y/02JUN/SHAPEK/1\r\nSS: CA1603/Y/05JUN/PEKHRB/1\r\nNM 1张越 CHD\r\nTKTL1606/27MAY/SHA888\r\nSSR FOID CA HK/NI20081010/P1\r\nSSR OTHS CA ADULT PNR IS KNEY6X\r\nSSR CHLD CA HK1/10OCT08/P1\r\nOSI CA CTCT 13203799010\r\nRMK TJ AUTH SHA333\r\nRMK TJ AUTH SHA123\r\n\\","ResultBag":"PNR CANCELLED                                                                  \r"},"reqtime":"\/Date(1432710360291+0800)\/","SaveTime":1800}
            // {"state":false,"error":{"ErrorCode":19,"ErrorMessage":"很抱歉，订位失败，请重新预订","CmdResultBag":"SEATS                                                                          \r"},"config":"","OfficeNo":"SHA243","result":{"Pnr":null,"OfficeNo":"SHA243","BookingState":2,"BigPNR":null,"Command":"SS: CA1858/Y/02JUN/SHAPEK/1\r\nSS: CA1603/Y/05JUN/PEKHRB/1\r\nNM 1张越 CHD\r\nTKTL1614/27MAY/SHA888\r\nSSR FOID CA HK/NI20081010/P1\r\nSSR OTHS CA ADULT PNR IS KNEY6X\r\nSSR CHLD CA HK1/10OCT08/P1\r\nOSI CA CTCT 13203799010\r\nRMK TJ AUTH SHA333\r\nRMK TJ AUTH SHA123\r\n\\","ResultBag":"SEATS                                                                          \r"},"reqtime":"\/Date(1432710881955+0800)\/","SaveTime":1800}
            // {"state":true,"error":null,"config":"","OfficeNo":"SHA243","result":{"Pnr":"JM5LQM","OfficeNo":"SHA243","BookingState":0,"BigPNR":"","Command":"SS: CA1858/Y/02JUN/SHAPEK/1\r\nSS: CA1603/Y/05JUN/PEKHRB/1\r\nNM 1张越 CHD\r\nTKTL1616/27MAY/SHA888\r\nSSR FOID CA HK/NI20081010/P1\r\nSSR OTHS CA ADULT PNR IS KNEY6X\r\nSSR CHLD CA HK1/10OCT08/P1\r\nOSI CA CTCT 13203799010\r\nRMK TJ AUTH SHA333\r\nRMK TJ AUTH SHA123\r\n\\","ResultBag":"JM5LQM -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  CA1858  Y TU02JUN  SHAPEK DK1   0755 1015                                    \r  CA1603  Y FR05JUN  PEKHRB DK1   0655 0900                                    \r 『航空公司使用自动出票时限, 请检查PNR』                                                        \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1432711008382+0800)\/","SaveTime":1800}
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, strParams);
            Console.WriteLine("返回结果：" + sret);
        }

        // 测试儿童联程票订位（非PN航司）

        // 测试成人单程票订位（PN航司）

        // 测试成人往返票订位（PN航司）
        [TestMethod]
        public void Test_Proxy101()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "PN6253";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-02");
            flight.SCity = "CKG";
            flight.ECity = "CAN";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "PN6206";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-07");
            flight.SCity = "CAN";
            flight.ECity = "CKG";
            request.FlightList.Add(flight);            

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张亮";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "612401198603061479";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "ZI/XING";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "320681198610238019";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            request.OfficeNo = "SHA888";

            request.Mobile = "13203799010";

            request.RMKOfficeNoList.Add("SHA333");
            request.RMKOfficeNoList.Add("SHA123");

            string strParams = JsonConvert.SerializeObject(request);
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"\",  \"OfficeNo\" : \"KHN117\" }";
            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：
            // {"state":false,"error":{"ErrorCode":19,"ErrorMessage":"很抱歉，订位失败，请重新预订","CmdResultBag":"PN  6253 Y  02JUN  CKGCAN NN2  UNABLE                                          \rUNABLE TO SELL.PLEASE CHECK THE AVAILABILITY WITH \"AV\" AGAIN                   \r"},"config":"","OfficeNo":"KHN117","result":{"Pnr":null,"OfficeNo":"KHN117","BookingState":2,"BigPNR":null,"Command":"SS: PN6253/Y/02JUN/CKGCAN/2\r\nSS: PN6206/Y/07JUN/CANCKG/2\r\nNM 1张亮1ZI/XING\r\nTKTL1535/27MAY/SHA888\r\nSSR FOID PN HK/NI612401198603061479/P1\r\nSSR FOID PN HK/NI320681198610238019/P2\r\nOSI PN CTCT 13203799010\r\nOSI PN CABIN/Y/Y\r\nRMK TJ AUTH SHA333\r\nRMK TJ AUTH SHA123\r\n\\","ResultBag":"PN  6253 Y  02JUN  CKGCAN NN2  UNABLE                                          \rUNABLE TO SELL.PLEASE CHECK THE AVAILABILITY WITH \"AV\" AGAIN                   \r"},"reqtime":"\/Date(1432708533567+0800)\/","SaveTime":1800}            
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, strParams);
            Console.WriteLine("返回结果：" + sret);
        }

        // 测试成人联程票订位（PN航司）

        // 测试成人单程票订位（非PN航司）
        [TestMethod]
        public void Test_Proxy104()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "JD5577";
            flight.Cabin = "E";
            flight.DepDate = Convert.ToDateTime("2015-06-25");
            flight.SCity = "PEK";
            flight.ECity = "SYX";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "郭维敏";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "342225198209102847";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);           

            request.OfficeNo = "SHA888";

            request.Mobile = "13641601096";

            request.RMKOfficeNoList.Add("CKG162");          

            string strParams = JsonConvert.SerializeObject(request);
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"\",  \"OfficeNo\" : \"KHN117\" }";
            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O762F461\",  \"OfficeNo\" : \"KHN117\" }";
            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：
            // 返回：NAMES
            // {"state":true,"error":null,"config":"","OfficeNo":"KHN117","result":{"Pnr":"KDM6HH","OfficeNo":"KHN117","BookingState":2,"BigPNR":"PENFSQ","Command":"SS: JD5577/E/25JUN/PEKSYX/1\r\nNM 1郭维敏\r\nTKTL1642/27MAY/SHA888\r\nSSR FOID JD HK/NI342225198209102847/P1\r\nOSI JD CTCT 13641601096\r\nRMK TJ AUTH CKG162\r\n\\","ResultBag":"  JD5577  E TH25JUN  PEKSYX NN1   0750 1145                                    \rKDM6HH -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1432712564792+0800)\/","SaveTime":1800}
            // {"state":true,"error":null,"config":"","OfficeNo":"KHN117","result":{"Pnr":"HV8DTL","OfficeNo":"KHN117","BookingState":2,"BigPNR":"PZNDW2","Command":"SS: JD5577/E/25JUN/PEKSYX/1\r\nNM 1郭维敏\r\nTKTL1702/27MAY/SHA888\r\nSSR FOID JD HK/NI342225198209102847/P1\r\nOSI JD CTCT 13641601096\r\nRMK TJ AUTH CKG162\r\n\\","ResultBag":"  JD5577  E TH25JUN  PEKSYX NN1   0750 1145                                    \rHV8DTL -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1432713772806+0800)\/","SaveTime":1800}
            // {"state":true,"error":null,"config":"O762F461","OfficeNo":"KHN117","result":{"Pnr":"HFHKQ4","OfficeNo":"KHN117","BookingState":2,"BigPNR":"PLGG66","Command":"SS: JD5577/E/25JUN/PEKSYX/1\r\nNM 1郭维敏\r\nTKTL1914/27MAY/SHA888\r\nSSR FOID JD HK/NI342225198209102847/P1\r\nOSI JD CTCT 13641601096\r\nRMK TJ AUTH CKG162\r\n\\","ResultBag":"  JD5577  E TH25JUN  PEKSYX NN1   0750 1145                                    \rHFHKQ4 -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1432721669028+0800)\/","SaveTime":1800}
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, strParams);
            Console.WriteLine("返回结果：" + sret);
        }

        // 测试成人往返票订位（非PN航司）
        [TestMethod]
        public void Test_Proxy102()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "CA1858";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-02");
            flight.SCity = "SHA";
            flight.ECity = "PEK";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "CA1603";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-05");
            flight.SCity = "PEK";
            flight.ECity = "HRB";
            request.FlightList.Add(flight);         

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张亮";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "612401198603061479";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "ZI/XING";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "320681198610238019";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            request.OfficeNo = "SHA888";

            request.Mobile = "13203799010";

            request.RMKOfficeNoList.Add("SHA333");
            request.RMKOfficeNoList.Add("SHA123");

            string strParams = JsonConvert.SerializeObject(request);
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"\",  \"OfficeNo\" : \"SHA243\" }";
            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：
            // {"state":true,"error":null,"config":"","OfficeNo":"SHA243","result":{"Pnr":"KNEY6X","OfficeNo":"SHA243","BookingState":0,"BigPNR":"NYJ68L","Command":"SS: CA1858/Y/02JUN/SHAPEK/2\r\nSS: CA1603/Y/05JUN/PEKHRB/2\r\nNM 1张亮1ZI/XING\r\nTKTL1541/27MAY/SHA888\r\nSSR FOID CA HK/NI612401198603061479/P1\r\nSSR FOID CA HK/NI320681198610238019/P2\r\nOSI CA CTCT 13203799010\r\nRMK TJ AUTH SHA333\r\nRMK TJ AUTH SHA123\r\n\\","ResultBag":"KNEY6X -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  CA1858  Y TU02JUN  SHAPEK DK2   0755 1015                                    \r  CA1603  Y FR05JUN  PEKHRB DK2   0655 0900                                    \r 『航空公司使用自动出票时限, 请检查PNR』                                                        \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1432708860190+0800)\/","SaveTime":1800}
            // 
            // 
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, strParams);
            Console.WriteLine("返回结果：" + sret);
        }

        // 测试成人联程票订位（非PN航司）
        [TestMethod]
        public void Test_Proxy100()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "CA1858";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-02");
            flight.SCity = "SHA";
            flight.ECity = "PEK";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "CA1603";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-05");
            flight.SCity = "PEK";
            flight.ECity = "HRB";
            request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "CA1640";
            flight.Cabin = "Y";
            flight.DepDate = Convert.ToDateTime("2015-06-10");
            flight.SCity = "HRB";
            flight.ECity = "TSN";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "张亮";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "612401198603061479";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "ZI/XING";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "320681198610238019";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            request.PassengerList.Add(passenger);

            request.OfficeNo = "SHA888";

            request.Mobile = "13203799010";

            request.RMKOfficeNoList.Add("SHA333");
            request.RMKOfficeNoList.Add("SHA123");

            string strParams = JsonConvert.SerializeObject(request);
            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O7712481\",  \"OfficeNo\" : \"SHA243\" }";
            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：
            // {"state":false,"error":{"ErrorCode":19,"ErrorMessage":"很抱歉，订位失败，请重新预订","CmdResultBag":"CA  1640 M  10JUN  HRBTSN NN2  SEGMENT
#warning will check: 查下ResultBag返回为空的原因。我分析下来是：执行i指令后，返回为空，然后就没执行下去。2015-06-01（星期一），志勇已修复
            // {"state":false,"error":{"ErrorCode":19,"ErrorMessage":"很抱歉，订位失败，请重新预订","CmdResultBag":""},"config":"O77124B1","OfficeNo":"SHA243","result":{"Pnr":null,"OfficeNo":"SHA243","BookingState":1,"BigPNR":null,"Command":"SS: CA1858/Y/02JUN/SHAPEK/2\r\nSS: CA1603/Y/05JUN/PEKHRB/2\r\nSS: CA1640/Y/10JUN/HRBTSN/2\r\nNM 1张亮1ZI/XING\r\nTKTL1442/27MAY/SHA888\r\nSSR FOID CA HK/NI612401198603061479/P1\r\nSSR FOID CA HK/NI320681198610238019/P2\r\nOSI CA CTCT 13203799010\r\nRMK TJ AUTH SHA333\r\nRMK TJ AUTH SHA123\r\n\\","ResultBag":""},"reqtime":"\/Date(1432705368931+0800)\/","SaveTime":1800}
            // {"state":false,"error":{"ErrorCode":19,"ErrorMessage":"很抱歉，订位失败，请重新预订","CmdResultBag":"SEATS                                                                          \r"},"config":"O7712481","OfficeNo":"SHA243","result":{"Pnr":null,"OfficeNo":"SHA243","BookingState":2,"BigPNR":null,"Command":"SS: CA1858/Y/02JUN/SHAPEK/2\r\nSS: CA1603/Y/05JUN/PEKHRB/2\r\nSS: CA1640/Y/10JUN/HRBTSN/2\r\nNM 1张亮1ZI/XING\r\nTKTL1518/27MAY/SHA888\r\nSSR FOID CA HK/NI612401198603061479/P1\r\nSSR FOID CA HK/NI320681198610238019/P2\r\nOSI CA CTCT 13203799010\r\nRMK TJ AUTH SHA333\r\nRMK TJ AUTH SHA123\r\n\\","ResultBag":"SEATS                                                                          \r"},"reqtime":"\/Date(1432707505160+0800)\/","SaveTime":1800}
            // {"state":false,"error":{"ErrorCode":13,"ErrorMessage":"预订命令返回为空","CmdResultBag":""},"config":"O7712481","OfficeNo":"SHA243","result":{"Pnr":null,"OfficeNo":"SHA243","BookingState":1,"BigPNR":null,"Command":"SS: CA1858/Y/02JUN/SHAPEK/2\r\nSS: CA1603/Y/05JUN/PEKHRB/2\r\nSS: CA1640/Y/10JUN/HRBTSN/2\r\nNM 1张亮1ZI/XING\r\nTKTL1442/01JUN/SHA888\r\nSSR FOID CA HK/NI612401198603061479/P1\r\nSSR FOID CA HK/NI320681198610238019/P2\r\nOSI CA CTCT 13203799010\r\nRMK TJ AUTH SHA333\r\nRMK TJ AUTH SHA123\r\n\\","ResultBag":""},"reqtime":"\/Date(1433137319841+0800)\/","SaveTime":1800}
            // {"state":false,"error":{"ErrorCode":13,"ErrorMessage":"预订命令返回为空","CmdResultBag":""},"config":"O77124B1","OfficeNo":"SHA243","result":{"Pnr":null,"OfficeNo":"SHA243","BookingState":1,"BigPNR":null,"Command":"SS: CA1858/Y/02JUN/SHAPEK/2\r\nSS: CA1603/Y/05JUN/PEKHRB/2\r\nSS: CA1640/Y/10JUN/HRBTSN/2\r\nNM 1张亮1ZI/XING\r\nTKTL1448/01JUN/SHA888\r\nSSR FOID CA HK/NI612401198603061479/P1\r\nSSR FOID CA HK/NI320681198610238019/P2\r\nOSI CA CTCT 13203799010\r\nRMK TJ AUTH SHA333\r\nRMK TJ AUTH SHA123\r\n\\","ResultBag":""},"reqtime":"\/Date(1433137680918+0800)\/","SaveTime":1800}
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, strParams);
            Console.WriteLine("返回结果：" + sret);
        }

        //===================以下是为AppId为100201进行的测试========================
        [TestMethod]
        public void Test_AdultBookingInvoke101()
        {
            // 订位请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();
            //JetermEntity.Request.Booking cmd = new JetermEntity.Request.Booking();

            // 设置应用程序编号
            cmd.AppId = 100201;

            // 根据各自的业务需求，设置缓存返回结果时长
            //cmd.CacheTime = EtermCommand.CacheTime.min30;
            cmd.CacheTime = EtermCommand.CacheTime.none;

            //cmd.officeNo = "KHN117";

            cmd.request = new JetermEntity.Request.Booking();

            #region 订位请求参数

            JetermEntity.Flight flight = new JetermEntity.Flight();

            flight.FlightNo = "CZ6412";
            flight.Cabin = "E";
            flight.DepDate = Convert.ToDateTime("2015-05-21");
            flight.SCity = "PEK";
            flight.ECity = "SHA";
            cmd.request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            //passenger.name = "FDSFDSF";
            //passenger.name = "FDSF";
            //passenger.name = "David/Yang";
            passenger.name = "FD/SF";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "1111";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            //passenger.name = "SDFDSFSDF";
            //passenger.name = "SDFDSF";
            //passenger.name = "Cathy/Han";
            passenger.name = "SD/FD";
            //passenger.Ename = "XuXiaoYIng";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "2222";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            cmd.request.PassengerList.Add(passenger);

            cmd.request.OfficeNo = "KHN117";
            //cmd.request.OfficeNo = "CZZ444";
            //cmd.request.OfficeNo = "BJS579";

            cmd.request.Mobile = "18101810679";

            //cmd.request.RMKOfficeNoList.Add("KHN117");
            //cmd.request.RMKOfficeNoList.Add("SHA777");
            //cmd.request.RMKOfficeNoList.Add("PEK513");
            //cmd.request.RMKOfficeNoList.Add("PEK277");
            //cmd.request.RMKOfficeNoList.Add("CKG234");
            //cmd.request.RMKOfficeNoList.Add("CKG24");
            //cmd.request.RMKOfficeNoList.Add("PEK112");
            //cmd.request.RMKOfficeNoList.Add("SHA123");
            cmd.request.RMKOfficeNoList.Add("CZZ444");

            //cmd.request.RMKRemark = "test";         

            #endregion

            #region 调用Invoke以处理业务

            //EtermClient client = new EtermClient();

            //CommandResult<JetermEntity.Response.Booking> result = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(cmd);

            EtermProxy.BLL.Booking logic = new EtermProxy.BLL.Booking(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            logic.OfficeNo = "KHN117";
            CommandResult<JetermEntity.Response.Booking> result = logic.BusinessDispose(cmd.request);

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
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult + Environment.NewLine + "返回的OfficeNo：" + result.result.OfficeNo + "；返回的配置名：" + result.config);

            #endregion

            //return;
        }

        // 生成json格式的订位请求数据
        /*
SS: 3U8165/L/25JUN/HGHCKG/2[RN]
         * NM 1邓玉兰1王良奎[RN]
         * TKTL0637/25JUN/SHA888[RN]
         * SSR FOID 3U HK/NI51032119721218050X/P1[RN]
         * SSR FOID 3U HK/NI510321197010170493/P2[RN]
         * OSI 3U CTCT 13516802717[RN]
         * RMK TJ AUTH BJS140[RN]
         * \
         */
        [TestMethod]
        public void Test_Proxy105()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "3U8165";
            flight.Cabin = "Y";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-07-30");
            flight.SCity = "HGH";
            flight.ECity = "CKG";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "王善";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "51032119721218050X";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            request.PassengerList.Add(passenger);          

            //cmd.request.OfficeNo = "KHN117";
            request.OfficeNo = "SHA888";
            //cmd.request.OfficeNo = "BJS579";

            request.Mobile = "13917436789";

            request.RMKOfficeNoList.Add("BJS140");

            string strParams = JsonConvert.SerializeObject(request);
            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O77124B1\",  \"OfficeNo\" : \"SHA243\" }";
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"\",  \"OfficeNo\" : \"KHN117\" }";
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O762F461\",  \"OfficeNo\" : \"KHN117\" }";
            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：
            // 返回：NAMES
            // {"state":true,"error":null,"config":"","OfficeNo":"KHN117","result":{"Pnr":"KDM6HH","OfficeNo":"KHN117","BookingState":2,"BigPNR":"PENFSQ","Command":"SS: JD5577/E/25JUN/PEKSYX/1\r\nNM 1郭维敏\r\nTKTL1642/27MAY/SHA888\r\nSSR FOID JD HK/NI342225198209102847/P1\r\nOSI JD CTCT 13641601096\r\nRMK TJ AUTH CKG162\r\n\\","ResultBag":"  JD5577  E TH25JUN  PEKSYX NN1   0750 1145                                    \rKDM6HH -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1432712564792+0800)\/","SaveTime":1800}
            // {"state":true,"error":null,"config":"","OfficeNo":"KHN117","result":{"Pnr":"HV8DTL","OfficeNo":"KHN117","BookingState":2,"BigPNR":"PZNDW2","Command":"SS: JD5577/E/25JUN/PEKSYX/1\r\nNM 1郭维敏\r\nTKTL1702/27MAY/SHA888\r\nSSR FOID JD HK/NI342225198209102847/P1\r\nOSI JD CTCT 13641601096\r\nRMK TJ AUTH CKG162\r\n\\","ResultBag":"  JD5577  E TH25JUN  PEKSYX NN1   0750 1145                                    \rHV8DTL -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1432713772806+0800)\/","SaveTime":1800}
            // {"state":true,"error":null,"config":"O762F461","OfficeNo":"KHN117","result":{"Pnr":"HFHKQ4","OfficeNo":"KHN117","BookingState":2,"BigPNR":"PLGG66","Command":"SS: JD5577/E/25JUN/PEKSYX/1\r\nNM 1郭维敏\r\nTKTL1914/27MAY/SHA888\r\nSSR FOID JD HK/NI342225198209102847/P1\r\nOSI JD CTCT 13641601096\r\nRMK TJ AUTH CKG162\r\n\\","ResultBag":"  JD5577  E TH25JUN  PEKSYX NN1   0750 1145                                    \rHFHKQ4 -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  \r  *** 预订酒店指令HC, 详情  ?HC:HELP   ***                                             \r"},"reqtime":"\/Date(1432721669028+0800)\/","SaveTime":1800}
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, strParams);
            Console.WriteLine("返回结果：" + sret);
        }

        //2015-09-22（星期二）测试：测试ZH航司+填PhoneNo的情况
        [TestMethod]
        public void Test_Proxy106()
        {
            JetermEntity.Request.Booking request = new JetermEntity.Request.Booking();

            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "ZH9557";
            flight.Cabin = "Y";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-09-30");
            flight.SCity = "CGQ";
            flight.ECity = "SZX";
            request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();
            passenger.name = "王善";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            passenger.cardno = "51032119721218050X";
            passenger.PassType = EtermCommand.PassengerType.Adult;
            //passenger.Ename = "zhuweijian";
            request.PassengerList.Add(passenger);

            //cmd.request.OfficeNo = "KHN117";
            request.OfficeNo = "SHA888";
            //cmd.request.OfficeNo = "BJS579";

            request.Mobile = "13917736789";
            request.PhoneNo = "02160727777";

            request.RMKOfficeNoList.Add("BJS140");

            string strParams = JsonConvert.SerializeObject(request);
            // O7712481,O7712491,O77124A1,
            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O7712481\",  \"OfficeNo\" : \"SHA243\" }";
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"\",  \"OfficeNo\" : \"KHN117\" }";
            //string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"O762F461\",  \"OfficeNo\" : \"KHN117\" }";
            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            // 返回结果：
            // 返回：
            /*
             KPCX4W -EOT SUCCESSFUL, BUT ASR UNUSED FOR 1 OR MORE SEGMENTS                  
  ZH9557  Y WE30SEP  CGQSZX DK1   0755 1345                                    
 『航空公司使用自动出票时限, 请检查PNR』                                                        
  *** 预订酒店指令HC, 详情  ?HC:HELP   ***   
             */
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, strParams);
            Console.WriteLine("返回结果：" + sret);
        }

        //（没测）测试ZH航司+没有填PhoneNo的情况
        [TestMethod]
        public void Test_Proxy107()
        {

        }

        //（没测）测试非ZH航司的情况
    }
}
