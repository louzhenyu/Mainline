using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JLog4netWinFormDemo
{
    public partial class Form1 : Form
    {
        ILog log = LogManager.GetLogger(typeof(Form1));
        ILog fileLog = LogManager.GetLogger("FileLogger");
        public Form1()
        {
            InitializeComponent();
        }


        private void btnSystemLog_Click(object sender, EventArgs e)
        {
            MongoLogAdd();
            
        }

        /// <summary>
        /// 系统日志，记录到mongodb
        /// </summary>
        private void MongoLogAdd()
        {
            log.Info("mongodblog   Test winform");
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 业务日志，记录到SQLSERVER
        /// </summary>
        private void BusLogAdd()
        {

            ILog busLogger = LogManager.GetLogger("BusOrderLogger");
            ThreadContext.Properties["userid"] = "9527";
            ThreadContext.Properties["username"] = "xupearl";
            busLogger.Info("business erro  winform");
        }

        private void btnBussLog_Click(object sender, EventArgs e)
        {
            BusLogAdd();
        }

        /// <summary>
        /// 文本日志
        /// </summary>
        private void FileLogAdd()
        {
            fileLog.Info("file test winform");
        }

        private void btnFileLog_Click(object sender, EventArgs e)
        {
            FileLogAdd();
        }
    }
}
