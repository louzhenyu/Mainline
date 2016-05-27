using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JetermClient.Test
{
    [TestClass]
    public class EtermProxyTest
    {
        [TestMethod]
        public void Test()
        {
            //string s = "http://192.168.2.224:15252/format=json&language=CSharp&method=Booking";
            //string post = "{\"FlightList\":[{\"FlightNo\":\"CA1893\",\"Cabin\":\"G\",\"SCity\":\"PVG\",\"ECity\":\"SZX\",\"DepDate\":\"\\/Date(1429804800000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"CA919\",\"Cabin\":\"Q\",\"SCity\":\"SZX\",\"ECity\":\"PVG\",\"DepDate\":\"\\/Date(1430236800000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"干园\",\"idtype\":0,\"cardno\":\"650121199412242866\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"},{\"name\":\"张杰\",\"idtype\":0,\"cardno\":\"140525198401186312\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"13472634765\",\"RMKOfficeNoList\":[],\"RMKRemark\":null,\"Pnr\":null}";
            //EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            //IntPtr ptr = IntPtr.Zero;
            //string str = proxy.InvokeEterm(ptr, ptr, s, post);
            
            string strPost = "{\"ClassName\" : \"Booking\", \"Config\" : \"CS002\",  \"OfficeNo\" : \"test\" }";            
            string ss = "{\"FlightList\":[{\"FlightNo\":\"MU5137\",\"Cabin\":\"H\",\"SCity\":\"SHA\",\"ECity\":\"PEK\",\"DepDate\":\"\\/Date(1430064000000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"},{\"FlightNo\":\"MU5156\",\"Cabin\":\"B\",\"SCity\":\"PEK\",\"ECity\":\"SHA\",\"DepDate\":\"\\/Date(1430323200000+0800)\\/\",\"ArrDate\":\"\\/Date(-62135596800000+0800)\\/\"}],\"PassengerList\":[{\"name\":\"干园\",\"idtype\":0,\"cardno\":\"650121199412242866\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"},{\"name\":\"张杰\",\"idtype\":0,\"cardno\":\"140525198401186312\",\"PassType\":0,\"Ename\":\"\",\"BirthDayString\":\"\",\"ChildBirthDayDate\":\"\\/Date(-62135596800000+0800)\\/\",\"TicketNo\":\"\"}],\"OfficeNo\":\"SHA888\",\"Mobile\":\"13472634765\",\"RMKOfficeNoList\":[],\"RMKRemark\":null,\"Pnr\":null}";

            EtermProxy.Proxy proxy = new EtermProxy.Proxy();
            string sret = proxy.InvokeEterm(IntPtr.Zero, IntPtr.Zero, strPost, ss);
            Console.WriteLine("返回结果：" + sret);
        }
    }
}
