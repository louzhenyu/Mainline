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
    public class BookingDemoTest
    {
        // （未测）测试成人单程票订位

        // 测试成人往返票订位
        [TestMethod]
        public void Test_AdultBookingInvoke2()
        {
            // 订位请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();

            // 设置应用程序编号
            cmd.AppId = 100630;

            // 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.min30;

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

            flight.FlightNo = "MU5137";
            flight.Cabin = "H";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-04-27");
            flight.SCity = "SHA";
            flight.ECity = "PEK";
            cmd.request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "MU5156";
            flight.Cabin = "B";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-04-30");
            flight.SCity = "PEK";
            flight.ECity = "SHA";
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
            //passenger.Ename = "dujunqiang";
            //cmd.request.PassengerList.Add(passenger);

            //cmd.request.OfficeNo = "KHN117";
            cmd.request.OfficeNo = "SHA888";
            //cmd.request.OfficeNo = "BJS579";

            cmd.request.Mobile = "13472634765";

            //cmd.request.RMKOfficeNoList.Add("KHN117");
            //cmd.request.RMKOfficeNoList.Add("SHA777");
            //cmd.request.RMKOfficeNoList.Add("PEK513");
            //cmd.request.RMKOfficeNoList.Add("PEK277");
            //cmd.request.RMKOfficeNoList.Add("CKG234");
            //cmd.request.RMKOfficeNoList.Add("CKG24");
            //cmd.request.RMKOfficeNoList.Add("PEK112");
            //cmd.request.RMKOfficeNoList.Add("SHA123");
            //cmd.request.RMKOfficeNoList.Add("PEK530");

            //cmd.request.RMKRemark = "test";         

            #endregion

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();

            CommandResult<JetermEntity.Response.Booking> result = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(cmd);

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
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }

#warning test here
        // （未测）测试儿童单程票订位
        [TestMethod]
        public void Test_ChildrenBookingInvoke1()
        {

        }

        // 测试儿童往返票订位
        [TestMethod]
        public void Test_ChildrenBookingInvoke2()
        {
            // 订位请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();

            // 设置应用程序编号
            cmd.AppId = 100630;

            // 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.min30;
          
            cmd.request = new JetermEntity.Request.Booking();

            #region 订位请求参数

            JetermEntity.Flight flight = new JetermEntity.Flight();
            //flight.FlightNo = "CA1893";
            //flight.Cabin = "G";          
            //flight.DepDate = Convert.ToDateTime("2015-04-24");
            //flight.SCity = "PVG";
            //flight.ECity = "SZX";
            //cmd.request.FlightList.Add(flight);

            //flight = new JetermEntity.Flight();
            //flight.FlightNo = "CA919";
            //flight.Cabin = "Q";       
            //flight.DepDate = Convert.ToDateTime("2015-04-29");
            //flight.SCity = "SZX";
            //flight.ECity = "PVG";
            //cmd.request.FlightList.Add(flight);

            flight.FlightNo = "MU5137";
            flight.Cabin = "H";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-04-27");
            flight.SCity = "SHA";
            flight.ECity = "PEK";
            cmd.request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "MU5156";
            flight.Cabin = "B";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-04-30");
            flight.SCity = "PEK";
            flight.ECity = "SHA";
            cmd.request.FlightList.Add(flight);

            JetermEntity.Passenger passenger = new JetermEntity.Passenger();            
            passenger.name = "林秀CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "20110201";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthDayDate = Convert.ToDateTime("2011-02-01");
            cmd.request.PassengerList.Add(passenger);

            passenger = new JetermEntity.Passenger();
            passenger.name = "徐莹莹CHD";
            passenger.idtype = JetermEntity.EtermCommand.IDtype.Other;
            passenger.cardno = "612732198903210342";
            passenger.PassType = EtermCommand.PassengerType.Children;
            passenger.ChildBirthDayDate = Convert.ToDateTime("2012-07-10");
            cmd.request.PassengerList.Add(passenger);            

            //cmd.request.OfficeNo = "KHN117";
            cmd.request.OfficeNo = "SHA888";
            //cmd.request.OfficeNo = "BJS579";
          
            cmd.request.Mobile = "13472634765";

            //cmd.request.RMKOfficeNoList.Add("KHN117");
            //cmd.request.RMKOfficeNoList.Add("SHA777");
            //cmd.request.RMKOfficeNoList.Add("PEK513");
            //cmd.request.RMKOfficeNoList.Add("PEK277");
            //cmd.request.RMKOfficeNoList.Add("CKG234");
            //cmd.request.RMKOfficeNoList.Add("CKG24");
            //cmd.request.RMKOfficeNoList.Add("PEK112");
            //cmd.request.RMKOfficeNoList.Add("SHA123");
            //cmd.request.RMKOfficeNoList.Add("PEK530");

            //cmd.request.RMKRemark = "test";

            //cmd.request.Pnr = "JNL22B";
            //cmd.request.Pnr = "HF4X80";
            cmd.request.Pnr = "JMME8S";

            #endregion

            #region 调用Invoke以处理业务

            EtermClient client = new EtermClient();

            CommandResult<JetermEntity.Response.Booking> result = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(cmd);

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
            Console.WriteLine("解析结果：" + Environment.NewLine + parseResult);

            //Console.ReadLine();

            #endregion
        }

        // （未测）测试有多名儿童往返票
    }
}
