using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetermClient.DAL;
using JetermClient.BLL;
using Newtonsoft.Json;
using JetermEntity.Request;
using JetermEntity;
using JetermEntity.Response;
using System.Net;
using System.Net.Sockets;
using JetermClient.Common;


namespace JetermClient.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ParserTest.Main1();
            return;

            EtermClient client = new EtermClient();
            string str = client.Invoke(123, "http://114.80.69.227:5555/RT123456", "", EtermCommand.ServerSource.EtermServer,TimeSpan.FromSeconds(5));
            string.Format(JMetricsHelper.JetermCount, "TEST").MeterMark("次");//计数
            //MetricsManager.MeterMark(string.Format(JMetricsHelper.JetermCount, "Common"), "次");
            
           

            
            Command<JetermEntity.Request.Booking> book = new Command<JetermEntity.Request.Booking>();
            book.AppId = 1232423;
            book.request = new JetermEntity.Request.Booking();
            book.request.FlightList = new List<Flight>();
            book.request.FlightList.Add(new Flight() { Cabin = "C", ECity = "PEK", SCity = "SHA", FlightNo = "CA2232" });
            book.request.PassengerList = new List<Passenger>();
            book.request.PassengerList.Add(new Passenger() { cardno = "2343543", BirthDayString = "23423543" });
            CommandResult<JetermEntity.Response.Booking> r1 = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(book);

            string.Format(JMetricsHelper.JetermCount, "Common").MeterMark("次");//计数
            Console.ReadLine(); 
            /*
           // string tes="{\"state\":false,\"error\":{\"ErrorCode\":4,\"ErrorMessage\":\"没有传必须传的请求参数值\"},\"config\":\"CS002\",\"OfficeNo\":\"KHN117\",\"result\":{\"Pnr\":null,\"OfficeNo\":\"KHN117\",\"PnrState\":false,\"Command\":null,\"ResultBag\":null},\"reqtime\":\"\\/Date(1427093175480+0800)\\/\",\"SaveTime\":1800}";
           // CommandResult<JetermEntity.Response.Booking> r=JsonConvert.DeserializeObject<CommandResult<JetermEntity.Response.Booking>>(tes);
            Console.WriteLine(string.Format("开始 {0}", DateTime.Now.ToString("HH:mm:ss.fff")));
            string s = "{\"FlightNo\":\"MU5137\",\"Cabin\":\"B\",\"SCity\":\"\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1427731200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}";

            string sss =  JetermClient.Utility.HttpService.HttpPost("192.168.5.165:15252", "Booking", s, 500, 500);
            //dome();
            Console.WriteLine(sss);

            Console.WriteLine(string.Format("结束 {0}", DateTime.Now.ToString("HH:mm:ss.fff")));

            Console.ReadLine();

            

            Flight f= JsonConvert.DeserializeObject<Flight>(s);
             */

            /*
            string commandBag = string.Format("{0}{1}{2}", "rtCmd", Environment.NewLine, "rtCmdResult");
            commandBag += string.Format("{0}{1}{2}{3}{4}", Environment.NewLine, Environment.NewLine, "priceCmd", Environment.NewLine, "priceCmdResult");
            return;
            */

            /*
            string error = "[ERROR]abc";
            //string error = "a[ERROR]bc";
            //error = "abc";
            error = error.Substring(7);
            int i = error.IndexOf("[ERROR]");
            return;
            */

            /*
            try
            {
                //ERROR errorValue = (ERROR)Enum.Parse(typeof(ERROR), "NO_FIND_CONFIG");
                ERROR errorValue = (ERROR)Enum.Parse(typeof(ERROR), "NO_FIND_CONFIG1,");
                if (Enum.IsDefined(typeof(ERROR), errorValue) | errorValue.ToString().Contains(","))
                {
                    Console.WriteLine("Converted '{0}' to {1}.", "NO_FIND_CONFIG", errorValue.ToString());
                }
                else
                {
                    Console.WriteLine("{0} is not an underlying value of the ERROR enumeration.", "NO_FIND_CONFIG1,");
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("'{0}' is not a member of the ERROR enumeration.", "NO_FIND_CONFIG1,");
            }            
            return;
             */

            //CommandResult<JetermEntity.Response.TicketInfo> re=new CommandResult<JetermEntity.Response.TicketInfo>();
            //JetermEntity.EtermCommand.TicketStatus ts= EtermCommand.TicketStatus.NotSet;
            //EtermProxy.Common.CommonBll<JetermEntity.Response.TicketInfo>.GetTicketStatusAndSaveTime("USED", re, out ts);

            //#region 邮件测试
            //JetermUntility.Communication com = new JetermUntility.Communication();
            //com.SendMail("345861586@qq.com", "bkbjwhsh","smtp.qq.com","测试","test",new string[]{"345861586@qq.com"});
            //#endregion

            //#region 配置管理测试
            //JetermEntity.EtermConfig config = new JetermEntity.EtermConfig();
            //config.UserName = "test";
            //EtermClient client = new EtermClient();
            //bool bret = client.ConfigManage(EtermCommand.OperatMethod.add, config);
            //#endregion

            //#region Eterm代理测试
            ////string s = "http://localhost:15252/format=json&method=SeekPNR&param0=SFE234&param1=Adult&param2=True";
            ////string s = "http://192.168.5.165:15252/format=json&language=CSharp&method=TicketInfo&param0=675-8911180438";
            //string s = "http://192.168.2.224:15252/format=json&language=CSharp&method=Booking";
            //string post = "{\"FlightList\":[{\"FlightNo\":\"MU5109\",\"Cabin\":\"Y\",\"SCity\":\"SHA\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1428422400000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"朱伟坚\",\"idtype\":0,\"cardno\":\"440106196510042095\",\"PassType\":0,\"Ename\":\"zhuweijian\",\"BirthDayString\":\"\",\"TicketNo\":\"\"}],\"OfficeNo\":\"KHN117\",\"Mobile\":\"13647125256\",\"RMKOfficeNoList\":[\"KHN117\"],\"RMKRemark\":\"test\",\"Pnr\":\"JNL22B\"}";
            //EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            //IntPtr ptr = IntPtr.Zero;
            //string str = proxy.InvokeEterm(ptr, ptr, "CS003", "", s, post);
            ////object o = JsonConvert.DeserializeObject(str);
            //return;
            //#endregion

            #region EtermClient测试
            //EtermClient clinet = new EtermClient();

            //Command<JetermEntity.Request.Booking> book = new Command<JetermEntity.Request.Booking>();
            //book.AppId = "1232423";
            //book.request = new JetermEntity.Request.Booking();
            //book.request.FlightList = new List<Flight>();
            //book.request.FlightList.Add(new Flight() { Cabin = "C", ECity = "PEK", SCity = "SHA", FlightNo = "CA2232" });
            //book.request.PassengerList = new List<Passenger>();
            //book.request.PassengerList.Add(new Passenger() { name="李小波", cardno = "2343543", BirthDayString = "23423543" });
            //CommandResult<JetermEntity.Response.Booking> r1 = clinet.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(book);

            //string sret = JsonConvert.SerializeObject(r1);

            //Console.Write(sret);
            //Console.ReadKey();


            // 大编码获取票号请求对象
            //Command<JetermEntity.Request.TicketByBigPnr> cmd = new Command<JetermEntity.Request.TicketByBigPnr>();
            // 订位请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();

            // 设置应用程序编号
            cmd.AppId = 100630;

            // 根据各自的业务需求，设置缓存时长
            cmd.CacheTime = EtermCommand.CacheTime.min30;            

            //cmd.request = new JetermEntity.Request.TicketByBigPnr();
            cmd.request = new JetermEntity.Request.Booking();            

            #region 大编码号请求参数

            //cmd.request.BigPnr = "PCZ0SX";
            //cmd.request.FlightNo = "8L9801";
            //string flightCode = "KMGLJG";
            //if (!string.IsNullOrEmpty(flightCode))
            //{
            //    if (flightCode.Length > 3)
            //    {
            //        cmd.request.SCity = flightCode.Substring(0, 3);
            //    }

            //    if (flightCode.Length > 5)
            //    {
            //        cmd.request.ECity = flightCode.Substring(3, 3);
            //    }
            //}

            #endregion

            #region 订位请求参数
            
            JetermEntity.Flight flight = new JetermEntity.Flight();
            //flight.FlightNo = "CA4309";
            //flight.Cabin = "E";
            //flight.SDate = "28JAN";
            //flight.SCity = "CTU";
            //flight.ECity = "CAN";

            //flight.FlightNo = "MU5138";
            //flight.Cabin = "V";
            ////flight.SDate = "18MAR";
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

            //flight.FlightNo = "MU5137";
            //flight.Cabin = "B";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-03-31");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";

            //flight.FlightNo = "MU5183";
            //flight.Cabin = "L";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-03-21");
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";
            //cmd.request.FlightList.Add(flight);

            //flight = new JetermEntity.Flight();
            //flight.FlightNo = "MU5127";
            //flight.Cabin = "M";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-03-23");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";            
            //cmd.request.FlightList.Add(flight);

            //flight.FlightNo = "CZ6412";
            //flight.Cabin = "Y";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-05-01");
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";
            //cmd.request.FlightList.Add(flight);

            //flight = new JetermEntity.Flight();
            //flight.FlightNo = "CZ3908";
            //flight.Cabin = "Y";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-05-04");
            //flight.SCity = "SHA";
            //flight.ECity = "PEK";
            //cmd.request.FlightList.Add(flight);

            //flight.FlightNo = "CA1606";
            //flight.Cabin = "B";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-06-17");
            //flight.SCity = "DLC";
            //flight.ECity = "PEK";
            //cmd.request.FlightList.Add(flight);

            //flight.FlightNo = "CA1831";
            //flight.Cabin = "K";
            ////flight.SDate = "18MAR";
            //flight.DepDate = Convert.ToDateTime("2015-05-29");
            //flight.SCity = "PEK";
            //flight.ECity = "SHA";
            //cmd.request.FlightList.Add(flight);

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

            flight.FlightNo = "MU5102";
            flight.Cabin = "M";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-04-16");
            flight.SCity = "PEK";
            flight.ECity = "SHA";
            cmd.request.FlightList.Add(flight);

            flight = new JetermEntity.Flight();
            flight.FlightNo = "MU5153";
            flight.Cabin = "L";
            //flight.SDate = "18MAR";
            flight.DepDate = Convert.ToDateTime("2015-04-18");
            flight.SCity = "SHA";
            flight.ECity = "PEK";
            cmd.request.FlightList.Add(flight);

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
            //passenger.Ename = "zhuweijian";

            //passenger.name = "干园";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "650121199412242866";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            ////passenger.Ename = "zhuweijian";

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

            //passenger.name = "张杰";
            ////passenger.Ename = "XuXiaoYIng";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "140525198401186312";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            ////passenger.Ename = "zhuweijian";

            //passenger.name = "沈璐";
            ////passenger.Ename = "XuXiaoYIng";
            //passenger.idtype = JetermEntity.EtermCommand.IDtype.IDcard;
            //passenger.cardno = "12312";
            //passenger.PassType = EtermCommand.PassengerType.Adult;
            ////passenger.Ename = "zhuweijian";

            cmd.request.PassengerList.Add(passenger);

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

            //cmd.request.Mobile = "13647125256";
            //cmd.request.Mobile = "15021977488";
            //cmd.request.Mobile = "15192781928";
            //cmd.request.Mobile = "15692182199"; 
            //cmd.request.Mobile = "15821988525";
            cmd.request.Mobile = "13472634765";           

            //cmd.request.RMKOfficeNoList.Add("KHN117");
            //cmd.request.RMKOfficeNoList.Add("SHA777");
            //cmd.request.RMKOfficeNoList.Add("PEK513");
            //cmd.request.RMKOfficeNoList.Add("PEK277");
            //cmd.request.RMKOfficeNoList.Add("CKG234");
            //cmd.request.RMKOfficeNoList.Add("CKG24");
            //cmd.request.RMKOfficeNoList.Add("PEK112");
            //cmd.request.RMKOfficeNoList.Add("SHA123");
            cmd.request.RMKOfficeNoList.Add("PEK530");

            //cmd.request.RMKRemark = "test";

            //cmd.request.Pnr = "JNL22B";
            //cmd.request.Pnr = "HF4X80";
            
            #endregion

            #endregion

            // EtermClient对象
            //EtermClient client = new EtermClient();

            //Command<JetermEntity.Request.Booking> book = new Command<JetermEntity.Request.Booking>();
            //book.AppId = "1232423";
            //book.request = new JetermEntity.Request.Booking();
            //book.request.FlightList = new List<Flight>();
            //book.request.FlightList.Add(new Flight() { Cabin="C", ECity="PEK",SCity="SHA", FlightNo="CA2232" });
            //book.request.PassengerList = new List<Passenger>();
            //book.request.PassengerList.Add(new Passenger() { cardno="2343543", BirthDayString="23423543" });
            //CommandResult<JetermEntity.Response.Booking> r1 = clinet.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(book);

            #region 调用Invoke以处理业务

            //CommandResult<JetermEntity.Response.TicketByBigPnr> result = client.Invoke<JetermEntity.Request.TicketByBigPnr, JetermEntity.Response.TicketByBigPnr>(cmd);
            CommandResult<JetermEntity.Response.Booking> result = client.Invoke<JetermEntity.Request.Booking, JetermEntity.Response.Booking>(cmd);

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
                Console.WriteLine(string.Format("返回有错误，错误信息为{0}", result.error.ErrorMessage));
                Console.ReadLine();
                return;
            }
            if (result.result == null)
            {
                Console.WriteLine("没有返回结果");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("运行结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));

            Console.ReadLine();

            #endregion
        }

        /// <summary>
        /// JetermClient调用示例
        /// </summary>
        private static void dome()
        {
            //EtermClient对象
            EtermClient client = new EtermClient();
            //订单请求对象
            Command<JetermEntity.Request.Booking> cmd = new Command<JetermEntity.Request.Booking>();
            //应用程序标识
            cmd.AppId = 124345345;            
            //应用缓存
            cmd.CacheTime = EtermCommand.CacheTime.min5;
            //要请求的订单对象及属性赋值
            cmd.request = new JetermEntity.Request.Booking();
            cmd.request.FlightList = new List<Flight>();
            cmd.request.OfficeNo = "SHA327";
            cmd.request.PassengerList = new List<Passenger>();
            cmd.request.RMKOfficeNoList = new List<string>();
            cmd.TimeOut = new TimeSpan(0, 0, 0, 50);
            //开始调用Invoke处理业务
            CommandResult<JetermEntity.Response.Booking> result =           //对应的请求结果
                client.Invoke<JetermEntity.Request.Booking,                 //泛型请求类定义
                JetermEntity.Response.Booking>                              //泛型应答类定义
                (cmd);                                                      //请求参数    
            //---------------------------------------------
            //业务处理
        }

    }

    /*
   [Flags]
   enum Colors { None = 0, Red = 1, Green = 2, Blue = 4 };
   [Flags]
   public enum ERROR
   {
       /// <summary>
       /// 未知错误
       /// </summary>
       [Description("未知错误")]
       NONE,
       /// <summary>
       /// 没有发现可用配置
       /// </summary>
       [Description("没有发现可用配置")]
       NO_FIND_CONFIG,
       /// <summary>
       /// 无效的业务名称
       /// </summary>
       [Description("无效的业务名称")]
       INVALID_BUSINESS
   }
   */
}
