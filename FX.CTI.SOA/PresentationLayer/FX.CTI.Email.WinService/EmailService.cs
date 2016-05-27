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

namespace FX.CTI.Email.WinService
{
    public partial class EmailService : ServiceBase
    {
        EmailSender _emailSender;
        public EmailService()
        {
            InitializeComponent();
            _emailSender = new EmailSender();
        }

        protected override void OnStart(string[] args)
        {
            _emailSender.Start();
        }

        protected override void OnStop()
        {
            _emailSender.Stop();
        }
    }
}
