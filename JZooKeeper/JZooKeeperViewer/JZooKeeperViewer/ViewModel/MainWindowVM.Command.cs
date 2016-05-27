using Microsoft.Practices.Prism.Commands;
using Org.Apache.Zookeeper.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;
using JZooKeeperViewer.Component;
using JZooKeeperViewer.Enum;
using JZooKeeperViewer.Model;
using JZooKeeperViewer.View;

namespace JZooKeeperViewer.ViewModel
{
    public partial class MainWindowVM
    {
        public DelegateCommand Connect { get; set; }
        public DelegateCommand Disconnect { get; set; }
        public DelegateCommand Refresh { get; set; }
        public DelegateCommand Clear { get; set; }
        public DelegateCommand SaveModify { get; set; }

        public DelegateCommand AddNode { get; set; }
        public DelegateCommand DeleteNode { get; set; }

        //added by Yang Li
        public DelegateCommand Search { get; set; }

        //added by Yang Li
        public DelegateCommand ClearSearch { get; set; }

        private void DoConnect()
        {
            ConnectSettingWindow win = new ConnectSettingWindow();
            bool? result = win.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var vm = win.DataContext as ConnectSettingWindowVM;
                try
                {
                    this.Watcher = new ZookeeperWatcher(this);
                    _zk = new ZooKeeper(vm.ConnectionString, TimeSpan.FromMilliseconds(double.Parse(vm.Timeout)), this.Watcher);
                    ZooKeeper.WaitUntilConnected(_zk);
                    //added by Yang Li
                    this.ConnectionString = vm.ConnectionString;
                    this.GetZookeeperNodes();
                }
                catch (Exception ex)
                {
                    this.AddLog(LogType.Fatal, ex.Message);
                    this.DoDisconnect();
                }
                finally
                {
                    this.RaiseToolBarCanExecuteChanged();
                }
            }
        }

        private bool DoCanConnect()
        {
            return _zk == null;
        }

        private void DoDisconnect()
        {
            try
            {
                //if (_zk != null) _zk.Dispose();
                _zk = null;

                //added by Yang Li
                this.MyMainWindow.Title = "JZooKeeperViewer";
                //added by Yang Li
                this.ConnectionString = string.Empty;

                ListViewDataContext.Clear();
                TreeViewDataContext.Clear();
                Data = null;

                this.AddLog(LogType.Info, "Disconnect successfully.");
            }
            catch (Exception ex)
            {
                this.AddLog(LogType.Fatal, ex.Message);
            }
            finally
            {
                this.RaiseToolBarCanExecuteChanged();
            }
        }

        private bool DoCanDisconnect()
        {
            return _zk != null;
        }

        //added by Yang Li
        private void DoRefresh1()
        {
            ListViewDataContext.Clear();
            this.CmbEncoding.SelectedIndex = 2;
            this.SelectedEncoding = "UTF8";
            this.DataText = string.Empty;

            this.GetZookeeperNodes();
        }

        private void DoRefresh()
        {
            try
            {
                //modified by Yang Li
                //this.GetZookeeperNodes();
                this.ClearSearchCondition();
                this.DoRefresh1();
                this.AddLog(LogType.Info, "Refresh successfully.", false);
            }
            catch (Exception ex)
            {
                this.AddLog(LogType.Fatal, ex.Message);
            }
            finally
            {
                this.RaiseToolBarCanExecuteChanged();
            }
        }

        private bool DoCanRefresh()
        {
            return _zk != null;
        }

        private void DoClear()
        {
            this.Logs.Clear();
        }

        private bool DoCanClear()
        {
            return true;
        }

        private void DoSaveModify()
        {
            byte[] data = null;
            //added by Yang Li
            string dataText = DataText.Trim();

            if (_selectedEncoding == "HEX")
            {
                MemoryStream ms = new MemoryStream();
                //modified by Yang Li
                //foreach (string hex in DataText.Split('-'))
                foreach (string hex in dataText.Split('-'))
                {
                    byte b = 0;
                    if (byte.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out b))
                    {
                        ms.WriteByte(b);
                    }
                    else
                    {
                        this.AddLog(LogType.Error, "hex data format error");
                        return;
                    }
                }
                data = ms.ToArray();
            }
            else
            {
                //modified by Yang Li
                //data = ConvertEncoding(_selectedEncoding).GetBytes(DataText);
                data = ConvertEncoding(_selectedEncoding).GetBytes(dataText);
            }

            //added by Yang Li: 修复了个Bug--每次修改后，都要把最新值赋给_data
            _data = data;

            //fixed by Yang Li: 1、当第>1次修改保存节点值时，解决抛异常的问题。2、必须根据实际将要被更新值的节点Version，进行更新值：
            //Stat stat = this.SetZookeeperNodeData(SelectedZookeeperTreeNodeModel.QueryPath, data, SelectedZookeeperTreeNodeModel.Stat.Cversion);
            Stat stat = this.SetZookeeperNodeData(SelectedZookeeperTreeNodeModel.QueryPath, data);
            if (stat != null)
            {
                this.SelectedZookeeperTreeNodeModel.Stat = stat;
                this.ChangeListView(stat);
            }
            else
            {
                this.SyncDataText();
                this.AddLog(LogType.Error, "version unmatch,set data failed");
            }
        }

        private bool DoCanSaveModify()
        {
            //modified by Yang Li: 只有编辑后的值和编辑前的值不一样时，才使保存按钮可用
            if (this.SelectedZookeeperTreeNodeModel == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.DataText))
            {
                if (_data == null || _data.Length == 0)
                {
                    return false;
                }
            }

            if (_data == null || _data.Length == 0)
            {
                return true;
            }

            string originalDataText = ConvertEncoding(_selectedEncoding).GetString(_data);
            string newDataText = this.DataText.Trim();
            if (newDataText.Equals(originalDataText))
            {
                return false;
            }

            return true;

            //return SelectedZookeeperTreeNodeModel != null && !string.IsNullOrWhiteSpace(this.DataText);
        }

        private void DoAddNode()
        {
            AddNodeWindow win = new AddNodeWindow(this.SelectedZookeeperTreeNodeModel.QueryPath);
            bool? result = win.ShowDialog();
            if (result.HasValue && result.Value)
            {
                AddNodeWindowVM vm = win.DataContext as AddNodeWindowVM;
                //added by Yang Li
                vm.NodeName = vm.NodeName.Trim();
                //modified by Yang Li
                //if (this.CreateZookeeperNode(string.Concat(this.SelectedZookeeperTreeNodeModel.JoinPath, "/", vm.NodeName), vm.SelectedACLMode, vm.SelectedCreateMode))
                if (this.CreateZookeeperNode(string.Concat(this.SelectedZookeeperTreeNodeModel.JoinPath, "/", vm.NodeName), vm.NodeValue.Trim(), vm.SelectedACLMode, vm.SelectedCreateMode))
                {
                    this.SelectedZookeeperTreeNodeModel.Childs.Add(new ZookeeperTreeNodeModel(vm.NodeName, string.Concat(this.SelectedZookeeperTreeNodeModel.JoinPath, "/", vm.NodeName), this.SelectedZookeeperTreeNodeModel));

                    //added by Yang Li
                    ClearSearchCondition();
                }
                else
                {
                    //modified by Yang Li
                    //this.AddLog(LogType.Error, "add node failed");
                    this.AddLog(LogType.Error, "Add node failed.");
                }
            }
        }

        private bool DoCanAddNode()
        {
            //modified by Yang Li: 不允许新增ZooKeeper默认【/zookeeper】节点的子节点
            //return this.SelectedZookeeperTreeNodeModel != null;
            return this.SelectedZookeeperTreeNodeModel != null && !this.SelectedZookeeperTreeNodeModel.QueryPath.Equals("/zookeeper") && !this.SelectedZookeeperTreeNodeModel.QueryPath.Equals("/zookeeper/quota");
        }

        private void DoDeleteNode()
        {
            try
            {
                //fixed by Yang Li: 需要根据选中节点的实际Version进行删除，否则会删除该节点时会抛BadVersion异常：
                if (_zk != null)
                {
                    Stat stat = _zk.Exists(this.SelectedZookeeperTreeNodeModel.QueryPath, false);
                    if (stat != null)
                    {
                        _zk.Delete(this.SelectedZookeeperTreeNodeModel.QueryPath, stat.Version);
                    }
                }                
                //this.DeleteZookeeperNode(this.SelectedZookeeperTreeNodeModel.QueryPath, this.SelectedZookeeperTreeNodeModel.Stat.Version);
            }
            //modified by Yang Li
            catch (Exception ex)
            {
                this.AddLog(LogType.Error, string.Format("Delete node failed. The error message is 【{0}】.", ex.Message));
            }
            //catch
            //{
            //    this.AddLog(LogType.Error, "delete node failed");
            //}
        }

        private bool DoCanDeleteNode()
        {
            //modified by Yang Li: 不允许删除ZooKeeper默认【/zookeeper】节点以及其子节点
            //return SelectedZookeeperTreeNodeModel != null && SelectedZookeeperTreeNodeModel.QueryPath != "/";
            return SelectedZookeeperTreeNodeModel != null && SelectedZookeeperTreeNodeModel.QueryPath != "/" && !this.SelectedZookeeperTreeNodeModel.QueryPath.Equals("/zookeeper") && !this.SelectedZookeeperTreeNodeModel.QueryPath.Equals("/zookeeper/quota");
        }

        //added by Yang Li
        private void DoSearch()
        {
            try
            {
                if (_zk == null)
                {
                    this.AddLog(LogType.Info, "Search Operation: Please connect to Server firstly.", false);
                    return;
                }

                this.DoRefresh1();
                if (TreeViewDataContext == null || !TreeViewDataContext.Any())
                {
                    this.AddLog(LogType.Info, "Search Operation: No content can be searched.", false);
                    return;
                }

                if (string.IsNullOrWhiteSpace(SearchKey))
                {
                    this.AddLog(LogType.Error, "Search Operation: Please enter Search Condition before clicking Search button.", false);
                    return;
                }

                SearchKey = SearchKey.Trim().Replace("／", "/");
                if (SearchKey.Equals("/"))
                {
                    this.AddLog(LogType.Info, "Search Operation: Search successfully.", false);
                    return;
                }

                SearchByQueryCondition(TreeViewDataContext[0].Childs);
                this.RaiseToolBarCanExecuteChanged();
                this.AddLog(LogType.Info, "Search Operation: Search successfully.", false);
            }
            catch (Exception ex)
            {
                //this.AddLog(LogType.Fatal, ex.Message);
                this.AddLog(LogType.Fatal, string.Format("Search Operation failed. The error message is 【{0}】.", ex.Message));
            }
        }

        //added by Yang Li
        private bool DoCanSearch()
        {
            return true;
        }

        //added by Yang Li
        private void DoClearSearch()
        {
            try
            {
                if (_zk == null)
                {
                    this.AddLog(LogType.Info, "Clear Search Condition Operation: Please connect to Server firstly.", false);
                    return;
                }

                this.ClearSearchCondition();
                this.DoRefresh1();

                this.TxtSearchKey.Focus();

                this.AddLog(LogType.Info, "Clear Search Condition Operation: Clear Search Condition successfully.", false);
            }
            catch (Exception ex)
            {
                //this.AddLog(LogType.Fatal, ex.Message);           
                this.AddLog(LogType.Fatal, string.Format("Clear Search Condition Operation failed. The error message is 【{0}】.", ex.Message));
            }
        }

        //added by Yang Li
        private bool DoCanClearSearch()
        {
            return true;
        }

        //added by Yang Li
        private void ClearSearchCondition()
        {
            SearchKey = string.Empty;
            this.TxtSearchKey.Text = string.Empty;
        }
    }
}