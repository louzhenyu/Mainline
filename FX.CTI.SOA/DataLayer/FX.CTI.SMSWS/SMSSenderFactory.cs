using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FX.CTI.SMSWS
{
    public class SMSSenderFactory
    {
        public static ISMSSender CreateSender(int smsChannel)
        {
            ISMSSender sender;
            switch (smsChannel)
            {
                case 1:
                    sender = new SMSSenderWXT();
                    break;
                case 2:
                    sender = new SMSSenderSxun();
                    break;
                default:
                    sender = new SMSSenderWXT();
                    break;
            }
            return sender;
        }
    }
}
