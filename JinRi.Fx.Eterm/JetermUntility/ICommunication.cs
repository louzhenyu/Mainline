using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace JetermUntility
{
    [ComVisible(true)]
    public interface ICommunication
    {
        [DispId(8)]
        void SendMail(string sEmail, string Pwd, string FormServer, string subject, string body,string[] toAddressList);        
    }
}
