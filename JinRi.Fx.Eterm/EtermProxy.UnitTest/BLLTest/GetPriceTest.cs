using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EtermProxy.UnitTest.BLLTest
{
    [TestClass]
    class GetPriceTest
    {
        //===============added by Li Yang, April 13th, 2015================================
        [TestMethod]
        public void Test_Deserializer1()
        {
            JetermEntity.Parser.GetPrice getPrice = new JetermEntity.Parser.GetPrice();

            JetermEntity.Request.GetPrice request = new JetermEntity.Request.GetPrice();
            request.PassengerType = JetermEntity.EtermCommand.PassengerType.Adult;
            request.FlightList = new List<JetermEntity.Flight>();
            JetermEntity.Flight flight = new JetermEntity.Flight();
            flight.FlightNo = "CZ123";

            string cmd = getPrice.ParseCmd(request);
            getPrice.ParseCmdResult(cmd);
        }

#warning code here：补测--1、儿童往返票价格列表。2、成人往返票价格列表
    }
}
