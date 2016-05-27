using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FX.CTI.SMSWS
{
    public interface ISMSSender
    {
        bool SendSMS(string mobile, string content);
    }
}
