using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JZooKeeperViewer.Component;
using JZooKeeperViewer.Model;

using System.Text.RegularExpressions;
using JZooKeeperViewer.Common;

namespace JZooKeeperViewer.ViewModel
{
    public class ConnectSettingWindowVM : NotifyObject
    {
        //modified by Yang Li
        //private string _connectionString = "192.168.8.100:2181";
        private string _connectionString = Common.Common.ZKServer;
        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                _connectionString = value;
                OK.RaiseCanExecuteChanged();
                this.RaisePropertyChanged("ConnectionString");
            }
        }

        //modified by Yang Li
        //private string _timeout = "15000";
        private string _timeout = Common.Common.ZKSessionTimeOut; // 默认1小时
        public string Timeout
        {
            get { return _timeout; }
            set
            {
                _timeout = value;
                OK.RaiseCanExecuteChanged();
                this.RaisePropertyChanged("Timeout");
            }
        }

        public DelegateCommand OK { get; set; }

        public ConnectSettingWindowVM()
        {
            OK = new DelegateCommand(DoOK, DoCanOK);
        }

        private void DoOK() { }

        //added by Yang Li
        private bool DoCanOK()
        {
            //added by Yang Li

            if (string.IsNullOrWhiteSpace(_connectionString) || string.IsNullOrWhiteSpace(_timeout))
            {
                return false;
            }

            string connectionString = _connectionString.Replace("．", ".").Replace("：", ":").Replace("，", ",").Trim();
            if (connectionString.Contains(","))
            {
                return false;
            }
            connectionString = Regex.Replace(connectionString, @"\s", string.Empty).Trim();
            string timeoutString = Regex.Replace(_timeout, @"\s", string.Empty).Trim();

            //modified by Yang Li
            //string[] hostandport = ConnectionString.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            //int timeout = 0;
            //int port = 0;            
            //if (hostandport.Length == 2 && int.TryParse(Timeout, out timeout) && timeout > 0 && int.TryParse(hostandport[1], out port) && port > 0)
            //{
            //    return true;
            //}
            //else return false;  

            string[] hostandport = connectionString.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            int port = 0;
            double timeout = 0;

            if (hostandport.Length != 2 || !int.TryParse(hostandport[1], out port) || port < 1)
            {
                return false;
            }

            if (!double.TryParse(timeoutString, out timeout) || timeout < 1)
            {
                return false;
            }

            string[] ip = hostandport[0].Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (ip.Length != 4)
            {
                return false;
            }
            foreach (string ipPart in ip)
            {
                char[] ipPartCharArray = ipPart.ToCharArray();
                foreach (char ipPartChar in ipPartCharArray)
                {
                    if (ipPartChar >= '0' && ipPartChar <= '9')
                    {
                        continue;
                    }

                    return false;
                }
            }

            //added by Yang Li
            _connectionString = connectionString;
            _timeout = timeoutString;

            return true;
        }
    }
}
