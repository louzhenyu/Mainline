using Org.Apache.Zookeeper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;
using JZooKeeperViewer.Component;
using JZooKeeperViewer.Enum;
using JZooKeeperViewer.Model;

namespace JZooKeeperViewer.ViewModel
{
    public partial class MainWindowVM
    {
        private ZooKeeper _zk = null;

        public bool ConnectZookeeper(string connectionString, TimeSpan timeout)
        {
            try
            {
                if (_zk == null)
                {
                    _zk = new ZooKeeper(connectionString, timeout, new ZookeeperWatcher(this));
                    ZooKeeper.WaitUntilConnected(_zk);
                }
                return true;
            }
            catch (Exception ex)
            {
                this.AddLog(LogType.Fatal, ex.Message);
                if (_zk != null) _zk.Dispose();
                _zk = null;
            }
            return false;
        }

        public Stat GetZookeeperNodeStat(string path)
        {
            if (_zk != null)
            {
                return _zk.Exists(path, true);
            }
            else return null;
        }

        public void GetZookeeperNodes()
        {
            if (_zk != null)
            {
                TreeViewDataContext.Clear();
                var node = new ZookeeperTreeNodeModel("/", "/", null);
                TreeViewDataContext.Add(node);
                GetZookeeperNodesLoop(node);
            }
        }

        public void GetZookeeperNodes(string queryPath)
        {
            var node = this.SearchNodeViaPath(this.TreeViewDataContext[0], queryPath);
            if (node != null)
            {
                node.Childs.Clear();
                GetZookeeperNodesLoop(node);
            }
        }

        private void GetZookeeperNodesLoop(ZookeeperTreeNodeModel node)
        {
            foreach (var child in _zk.GetChildren(node.QueryPath, true))
            {
                var childnode = new ZookeeperTreeNodeModel(child, string.Concat(node.JoinPath, "/", child), node);
                node.Childs.Add(childnode);
                GetZookeeperNodesLoop(childnode);
            }
        }

        public void GetZookeeperNodeStatAndData()
        {
            if (this.SelectedZookeeperTreeNodeModel != null)
            {
                ListViewDataContext.Clear();
                Data = null;

                try
                {
                    Stat stat = GetZookeeperNodeStat(this.SelectedZookeeperTreeNodeModel.QueryPath);
                    if (stat != null)
                    {
                        this.ChangeListView(stat);
                        Data = _zk.GetData(this.SelectedZookeeperTreeNodeModel.QueryPath, false, stat);
                        this.SelectedZookeeperTreeNodeModel.Stat = stat;
                    }
                    else
                    {
                        this.AddLog(LogType.Error, string.Concat("this node had removed:", this.SelectedZookeeperTreeNodeModel.QueryPath));
                    }
                }
                catch (Exception ex)
                {
                    this.AddLog(LogType.Fatal, ex.Message);
                }
            }
        }

        //fixed by Yang Li: 必须根据实际将要被更新值的节点Version，进行更新值：
        public Stat SetZookeeperNodeData(string path, byte[] data)
        //public Stat SetZookeeperNodeData(string path, byte[] data, int version)
        {
            if (_zk != null)
            {
                //fixed by Yang Li: 必须根据实际将要被更新值的节点Version，进行更新值：
                Stat currentSat = _zk.Exists(path, false);
                if (currentSat != null)
                {
                    return _zk.SetData(path, data, currentSat.Version);
                }
                return null;
                //return _zk.SetData(path, data, version);
            }
            else return null;
        }

        //modified by Yang Li
        //public bool CreateZookeeperNode(string path, string aclMode, string createMode)
        public bool CreateZookeeperNode(string path, string nodeValue, string aclMode, string createMode)
        {
            if (_zk != null)
            {
                List<ACL> acl = null;
                if (aclMode == "CREATOR_ALL_ACL")
                {
                    acl = Ids.CREATOR_ALL_ACL;
                }
                else if (aclMode == "OPEN_ACL_UNSAFE")
                {
                    acl = Ids.OPEN_ACL_UNSAFE;
                }
                else if (aclMode == "READ_ACL_UNSAFE")
                {
                    acl = Ids.READ_ACL_UNSAFE;
                }
                CreateMode mode = null;
                if (createMode == "Ephemeral")
                {
                    mode = CreateMode.Ephemeral;
                }
                else if (createMode == "EphemeralSequential")
                {
                    mode = CreateMode.EphemeralSequential;
                }
                else if (createMode == "Persistent")
                {
                    mode = CreateMode.Persistent;
                }
                else if (createMode == "PersistentSequential")
                {
                    mode = CreateMode.PersistentSequential;
                }

                //modified by Yang Li
                bool result = false;
                if (_zk.Exists(path, false) == null)
                {
                    result = _zk.Create(path, ConvertEncoding("UTF8").GetBytes(nodeValue), acl, mode) == path;
                }
                if (result)
                {
                    _zk.Exists(path, true);
                }
                //bool result = _zk.Create(path, null, acl, mode) == path;
                //if (result)
                //{
                //    _zk.Exists(path, true);
                //}
                return result;
            }
            return false;
        }

        public void DeleteZookeeperNode(string path, int version)
        {
            if (_zk != null)
            {
                _zk.Delete(path, version);
            }
        }

        public void AddZookeeperNode(string path, string name)
        {
            var node = SearchNodeViaPath(TreeViewDataContext[0], path);
            if (node != null)
            {
                node.Childs.Add(new ZookeeperTreeNodeModel(name, path, node));
                _zk.Exists(path, true);
            }
        }

        public void RemoveZookeeperNode(string path)
        {
            var node = SearchNodeViaPath(TreeViewDataContext[0], path);
            if (node != null)
            {
                node.Root.Childs.Remove(node);
            }
        }

        public void ProcessWatchedEvent(WatchedEvent @event)
        {
            //modified by Yang Li
            //this.AddLog(LogType.Info, string.Format("[{0}]Event:[{1}][{2}]{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), @event.Path, @event.Type.ToString(), @event.State.ToString()));
            this.AddLog(LogType.Info, string.Format("Event: [{0}][{1}]{2}.", @event.Path, @event.Type.ToString(), @event.State.ToString()));
            //added by Yang Li
            if (string.IsNullOrWhiteSpace(@event.Path))
            {
                switch (@event.Type)
                {
                    case EventType.None:
                        if (@event.State == KeeperState.SyncConnected)
                        {
                            this.MyMainWindow.Title = string.Format("JZooKeeperViewer(Connected to ZooKeeper Server【{0}】)", this.ConnectionString);
                        }
                        break;
                    default:
                        break;
                }
                return;
            }    
        
            if (!string.IsNullOrWhiteSpace(@event.Path))
            {
                try
                {
                    string path = @event.Path.Substring(0, @event.Path.LastIndexOf('/'));
                    string name = @event.Path.Substring(@event.Path.LastIndexOf('/') + 1);
                    switch (@event.Type)
                    {
                        case EventType.NodeCreated:
                            this.AddZookeeperNode(string.IsNullOrWhiteSpace(path) ? "/" : path, name);
                            break;
                        case EventType.NodeDeleted:
                            this.RemoveZookeeperNode(@event.Path);
                            break;
                        case EventType.NodeChildrenChanged:
                            this.GetZookeeperNodes(@event.Path);
                            break;
                        //added by Yang Li
                        case EventType.NodeDataChanged:
                            {
                                if (_zk == null)
                                {
                                    break;
                                }

                                if (this._selectedZookeeperTreeNodeModel == null)
                                {
                                    break;
                                }

                                Stat stat = GetZookeeperNodeStat(this._selectedZookeeperTreeNodeModel.QueryPath);
                                if (stat == null)
                                {
                                    break;
                                }

                                byte[] data = _zk.GetData(this._selectedZookeeperTreeNodeModel.QueryPath, false, stat);
                                if (data == null || data.Length == 0)
                                {
                                    break;
                                }
                                DataText = ConvertEncoding(_selectedEncoding).GetString(data);
                            }
                            break;                        
                    }
                }
                catch { }
            }
        }
    }
}