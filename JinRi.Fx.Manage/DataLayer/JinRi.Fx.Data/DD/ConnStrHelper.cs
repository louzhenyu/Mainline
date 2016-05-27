using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Data.DD
{
    public class ConnStrHelper
    {
        public static string GetConnStr(string serverName)
        {
            string result = null;
            switch (serverName)
            {
                case "1":
                    result = ConnectionStr.DomesticDD;
                    break;
                case "2":
                    result = ConnectionStr.InternationalDD;
                    break;
            }
            return result;
        }
    }
}
