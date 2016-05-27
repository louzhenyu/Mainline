using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace EtermProxy
{    
    /// <summary>
    /// Eterm代理接口
    /// </summary>
    [ComVisible(true)]
    public interface IProxy
    {
        [DispId(1)]
        string InvokeEterm(IntPtr hwnd, IntPtr handle, string strPost,string strParam);
        [DispId(2)]
        void SetEtermData(string config, string guid, string cmd, string data);
    }
}
