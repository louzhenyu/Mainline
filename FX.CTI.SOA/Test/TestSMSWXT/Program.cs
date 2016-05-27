using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FX.CTI.SMSWS;

namespace TestSMSWXT
{
    class Program
    {
        static void Main(string[] args)
        {
            var wxt = new SMSSenderWXT();
            wxt.SendSMS("13482767468", "今日测试");
        }
    }
}
