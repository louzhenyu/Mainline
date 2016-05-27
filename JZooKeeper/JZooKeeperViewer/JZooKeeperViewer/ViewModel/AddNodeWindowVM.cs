using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JZooKeeperViewer.Component;

namespace JZooKeeperViewer.ViewModel
{
    public class AddNodeWindowVM : NotifyObject
    {
        private string _path = null;
        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                this.RaisePropertyChanged("Path");
            }
        }

        private string _nodeName = null;
        public string NodeName
        {
            get { return _nodeName; }
            set
            {
                _nodeName = value;
                OK.RaiseCanExecuteChanged();
                this.RaisePropertyChanged("NodeName");
            }
        }

        //added by Yang Li
        private string _nodeValue = string.Empty;
        public string NodeValue
        {
            get { return _nodeValue; }
            set
            {
                _nodeValue = value;
                OK.RaiseCanExecuteChanged();
                this.RaisePropertyChanged("NodeValue");
            }
        }

        private string _selectedACLMode = "OPEN_ACL_UNSAFE";
        public string SelectedACLMode
        {
            get { return _selectedACLMode; }
            set
            {
                _selectedACLMode = value;
                this.RaisePropertyChanged("SelectedACLMode");
            }
        }

        private string _selectedCreateMode = "Persistent";
        public string SelectedCreateMode
        {
            get { return _selectedCreateMode; }
            set
            {
                _selectedCreateMode = value;
                this.RaisePropertyChanged("SelectedCreateMode");
            }
        }

        public DelegateCommand OK { get; set; }

        public AddNodeWindowVM(string path)
        {
            this.Path = path;
            OK = new DelegateCommand(DoOK, DoCanOK);
        }

        private void DoOK() { }

        private bool DoCanOK()
        {
            if (!string.IsNullOrWhiteSpace(NodeName))
            {
                return true;
            }
            else return false;
        }
    }
}
