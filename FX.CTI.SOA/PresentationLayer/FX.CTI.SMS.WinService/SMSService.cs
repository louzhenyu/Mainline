using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using FX.CTI.Business;

namespace FX.CTI.SMS.WinService
{
    public partial class SMSService : ServiceBase
    {
        SMSSender _smsSender;
        public SMSService()
        {
            InitializeComponent();
            _smsSender = new SMSSender();
        }

        protected override void OnStart(string[] args)
        {
            _smsSender.Start();
        }

        protected override void OnStop()
        {
            _smsSender.Stop();
        }
    }
}
