using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FX.CTI.Business;

namespace Test_FX.CTI.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var sender = new EmailSender();
            sender.Start();
            while (true)
            {
                Console.ReadKey();
                sender.Stop();
            }
        }
    }
}
