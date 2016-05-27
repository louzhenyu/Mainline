using JetermEntity.Request;
using JetermEntity.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EtermProxy.UnitTest
{
    [TestClass]
    public class TicketInfoByFTest
    {
        [TestMethod]
        public void Test_BusinessDispose()
        {        
            JetermEntity.Request.TicketInfoByF request = new JetermEntity.Request.TicketInfoByF();          
            request.TicketNo = "784-2158602564";
            //request.TicketNo = "7842133192747";
            //request.TicketNo = "7842130024027";

            EtermProxy.BLL.TicketInfoByF logic = new EtermProxy.BLL.TicketInfoByF(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            CommandResult<JetermEntity.Response.TicketInfoByF> result = logic.BusinessDispose(request);

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

        [TestMethod]
        public void Test_BusinessDispose2()
        {
            JetermEntity.Request.TicketInfoByF request = new JetermEntity.Request.TicketInfoByF();
            //request.TicketNo = "784-2158602564";
            //request.TicketNo = "7842133192747";
            //request.TicketNo = "7842130024027";
            //request.TicketNo = "784-7589111741";
            request.TicketNo = "999-8906177682 ";

            EtermProxy.BLL.TicketInfoByF logic = new EtermProxy.BLL.TicketInfoByF(IntPtr.Zero, IntPtr.Zero, string.Empty, string.Empty);
            logic.OfficeNo = "SHA243";
            CommandResult<JetermEntity.Response.TicketInfoByF> result = logic.BusinessDispose(request);

            if (result != null && result.state)
            {
                // 运行结果，如：
                // {"TicketNo":"7847589111741","PassengerName":"LIU/JOANNE","PassengerCardNo":null,"IsSchedule":false}
                Console.WriteLine("运行结果：" + Newtonsoft.Json.JsonConvert.SerializeObject(result.result));
                return;
            }
            if (result.error != null)
            {
                Console.WriteLine("运行错误，错误信息：" + result.error.ErrorMessage);
            }
        }
    }
}
