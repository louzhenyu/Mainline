using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using JZooKeeperViewer.Component;
using JZooKeeperViewer.Model;
using JZooKeeperViewer.View;
using ZooKeeperNet;
using Org.Apache.Zookeeper.Data;
using JZooKeeperViewer.Enum;
using System.IO;
using System.Windows.Data;

using log4net;
using System.Net;
using System.Windows.Controls;
using System.Windows;

namespace JZooKeeperViewer.ViewModel
{
    public partial class MainWindowVM : NotifyObject
    {
        private static ILog log4Net = LogManager.GetLogger(typeof(MainWindowVM));

        public ObservableCollection<ZookeeperTreeNodeModel> TreeViewDataContext { get; set; }
        private object _treeViewDataContextLock = new object();
        public ObservableCollection<ZookeeperStatModel> ListViewDataContext { get; set; }
        private object _listViewDataContextLock = new object();
        public ObservableCollection<string> Logs { get; set; }
        private object _logsLock = new object();

        private ZookeeperTreeNodeModel _selectedZookeeperTreeNodeModel = null;
        public ZookeeperTreeNodeModel SelectedZookeeperTreeNodeModel
        {
            get { return _selectedZookeeperTreeNodeModel; }
            set
            {
                _selectedZookeeperTreeNodeModel = value;
                this.RaisePropertyChanged("SelectedZookeeperTreeNodeModel");
                this.GetZookeeperNodeStatAndData();
            }
        }
        public ZookeeperStatModel SelectedZookeeperStatModel { get; set; }

        private string _selectedEncoding = "UTF8";
        public string SelectedEncoding
        {
            get
            {
                return _selectedEncoding;
            }
            set
            {
                _selectedEncoding = value;
                this.SyncDataText();
            }
        }

        private byte[] _data = null;
        public byte[] Data
        {
            get { return _data; }
            set
            {
                _data = value;
                this.SyncDataText();
            }
        }

        private string _dataText = null;
        public string DataText
        {
            get { return _dataText; }
            set
            {
                _dataText = value;
                SaveModify.RaiseCanExecuteChanged();
                this.RaisePropertyChanged("DataText");
            }
        }

        //added by Yang Li
        public string SearchKey
        {
            get;
            set;
        }

        //added by Yang Li
        private TextBox _txtSearchKey = null;
        public TextBox TxtSearchKey
        {
            get
            {
                return _txtSearchKey;
            }
            set
            {
                _txtSearchKey = value;
            }
        }

        //added by Yang Li
        private ComboBox _cmbEncoding = null;
        public ComboBox CmbEncoding
        {
            get
            {
                return _cmbEncoding;
            }
            set
            {
                _cmbEncoding = value;
            }
        }

        //added by Yang Li
        public string ConnectionString
        {
            get;
            set;
        }

        //added by Yang Li
        public Window MyMainWindow
        {
            get;
            set;
        }

        public IWatcher Watcher { get; private set; }

        public MainWindowVM()
        {
            TreeViewDataContext = new ObservableCollection<ZookeeperTreeNodeModel>();
            ListViewDataContext = new ObservableCollection<ZookeeperStatModel>();
            Logs = new ObservableCollection<string>();

            BindingOperations.EnableCollectionSynchronization(TreeViewDataContext, _treeViewDataContextLock);
            BindingOperations.EnableCollectionSynchronization(ListViewDataContext, _listViewDataContextLock);
            BindingOperations.EnableCollectionSynchronization(Logs, _logsLock);

            Connect = new DelegateCommand(DoConnect, DoCanConnect);
            Disconnect = new DelegateCommand(DoDisconnect, DoCanDisconnect);
            Refresh = new DelegateCommand(DoRefresh, DoCanRefresh);
            Clear = new DelegateCommand(DoClear, DoCanClear);

            SaveModify = new DelegateCommand(DoSaveModify, DoCanSaveModify);

            AddNode = new DelegateCommand(DoAddNode, DoCanAddNode);
            DeleteNode = new DelegateCommand(DoDeleteNode, DoCanDeleteNode);

            //added by Yang Li
            Search = new DelegateCommand(DoSearch, DoCanSearch);
            //added by Yang Li
            ClearSearch = new DelegateCommand(DoClearSearch, DoCanClearSearch);
        }

        public void RaiseToolBarCanExecuteChanged()
        {
            this.Connect.RaiseCanExecuteChanged();
            this.Disconnect.RaiseCanExecuteChanged();
            this.Refresh.RaiseCanExecuteChanged();
        }

        public void RaiseTreeViewContextMenuCanExecuteChanged()
        {
            this.AddNode.RaiseCanExecuteChanged();
            this.DeleteNode.RaiseCanExecuteChanged();
        }

        private ZookeeperTreeNodeModel SearchNodeViaPath(ZookeeperTreeNodeModel root, string queryPath)
        {
            if (root.QueryPath == queryPath) return root;
            foreach (var node in root.Childs)
            {
                if (node.QueryPath == queryPath) return node;
                else if (queryPath.Contains(node.QueryPath))
                {
                    return SearchNodeViaPath(node, queryPath);
                }
            }
            return null;
        }

        private void SyncDataText()
        {
            if (_data != null && !string.IsNullOrWhiteSpace(_selectedEncoding))
            {
                if (_selectedEncoding == "HEX")
                {
                    DataText = BitConverter.ToString(_data).ToUpper();
                }
                else
                {
                    DataText = ConvertEncoding(_selectedEncoding).GetString(_data);
                }
            }
            else DataText = null;
        }

        public void AddLog(LogType type, string log, bool saveLog = true)
        {
            //modified by Yang Li
            //this.Logs.Add(string.Format("[{0}]{1}:{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), type.ToString(), log));
            string message = string.Format("[{0}]{1}: {2} Operated By: [Client Server IP: {3}][Client Server Name: {4}].", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), type.ToString(), log, Common.Common.GetHostIP().ToString(), Dns.GetHostName());
            this.Logs.Add(message);

            //added by Yang Li
            if (saveLog)
            {
                message = string.Format("JZooKeeperViewer: [{0}]{1}{2}: {3} {1}Operated By: {1}[Client Server IP: {4}]{1}[Client Server Name: {5}].", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Environment.NewLine, type.ToString(), log, Common.Common.GetHostIP().ToString(), Dns.GetHostName());
                switch (type)
                {
                    case LogType.Info:
                        log4Net.Info(message);
                        break;
                    case LogType.Fatal:
                    case LogType.Error:
                        log4Net.Error(message);
                        break;
                }
            }            
        }

        public void ChangeListView(Stat stat)
        {
            ListViewDataContext.Clear();
            ListViewDataContext.Add(new ZookeeperStatModel("Aversion", stat.Aversion.ToString()));
            ListViewDataContext.Add(new ZookeeperStatModel("Ctime", stat.Ctime == 0 ? null : ConvertTimeTickToLocalDateTime(stat.Ctime).ToString("yyyy-MM-dd HH:mm:ss")));
            ListViewDataContext.Add(new ZookeeperStatModel("Cversion", stat.Cversion.ToString()));
            ListViewDataContext.Add(new ZookeeperStatModel("Czxid", stat.Czxid.ToString()));
            ListViewDataContext.Add(new ZookeeperStatModel("DataLength", stat.DataLength.ToString()));
            ListViewDataContext.Add(new ZookeeperStatModel("EphemeralOwner", stat.EphemeralOwner.ToString()));
            ListViewDataContext.Add(new ZookeeperStatModel("Mtime", stat.Mtime == 0 ? null : ConvertTimeTickToLocalDateTime(stat.Mtime).ToString("yyyy-MM-dd HH:mm:ss")));
            ListViewDataContext.Add(new ZookeeperStatModel("Mzxid", stat.Mzxid.ToString()));
            ListViewDataContext.Add(new ZookeeperStatModel("NumChildren", stat.NumChildren.ToString()));
            ListViewDataContext.Add(new ZookeeperStatModel("Pzxid", stat.Pzxid.ToString()));
            ListViewDataContext.Add(new ZookeeperStatModel("Version", stat.Version.ToString()));
        }

        private DateTime ConvertTimeTickToLocalDateTime(long tick)
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1).AddMilliseconds(tick));
        }

        private Encoding ConvertEncoding(string name)
        {
            switch (name)
            {
                case "ASCII":
                    return Encoding.ASCII;
                case "UTF8":
                    return Encoding.UTF8;
                case "Unicode":
                    return Encoding.Unicode;
                case "GB2312":
                    return Encoding.GetEncoding("gb2312");
                case "GBK":
                    return Encoding.GetEncoding("gbk");
                case "BigEndianUnicode":
                    return Encoding.BigEndianUnicode;
                case "UTF32":
                    return Encoding.UTF32;
                case "UTF7":
                    return Encoding.UTF7;
            }
            return Encoding.Default;
        }

        //added by Yang Li
        /// <summary>
        /// 搜索符合查询条件的节点
        /// </summary>
        /// <param name="treeViewDataContextDS"></param>
        private void SearchByQueryCondition(ObservableCollection<ZookeeperTreeNodeModel> treeViewDataContextDS)
        {
            string searchKey = string.Empty;
            if (!string.IsNullOrWhiteSpace(SearchKey))
            {
                searchKey = SearchKey.Trim().ToLower();
            }

            // 删除不符合查询条件的所有节点（【/zookeeper】节点以及它的后代节点除外）         
            int treeCount = treeViewDataContextDS.Count;
            for (int j = (treeCount - 1); j > -1; --j)
            {
                ZookeeperTreeNodeModel rootNode = treeViewDataContextDS[j];

                if (rootNode.DisplayName.Equals("zookeeper"))
                {
                    continue;
                }

                // 判断没有后代的鼻祖节点是否符合查询条件。如果不符合，则删除该鼻祖节点；如果符合，则不删除该鼻祖节点，而是退出本次循环，进入下棵树的遍历。
                if (rootNode.Childs.Count == 0)
                {
                    if (!IsMatchedAncestorNode(rootNode, searchKey))
                    {
                        TreeViewDataContext[0].Childs.Remove(rootNode);
                    }

                    continue;
                }

                SearchByQueryCondition(rootNode, searchKey);
            }
        }

        //added by Yang Li
        /// <summary>
        /// 删除某树中不符合查询条件的所有节点
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="searchKey"></param>   
        private void SearchByQueryCondition(ZookeeperTreeNodeModel rootNode, string searchKey)
        {
            if (rootNode == null)
            {
                return;
            }

            int front = -1, rear = -1; // 定义队头和队尾指针
            // 存放树中所有节点         
            List<ZookeeperTreeNodeModel> nodeList = new List<ZookeeperTreeNodeModel>();
            // 先序遍历树，实现了由树结构转为List结构：
            ConvertTreeToList(rootNode, front, rear, nodeList);

            // 从叶子节点开始一个一个删除节点，直到遇到这类节点--其孩子节点中有符合查询条件的孩子节点，则不删除该节点，而是退出本次循环，进入下条路径的遍历。         
            List<ZookeeperTreeNodeModel> pathList = nodeList.Where<ZookeeperTreeNodeModel>(node => node.Childs.Count == 0).ToList();
            if (pathList == null || pathList.Count < 1)
            {
                return;
            }

            int pathCount = pathList.Count;
            for (int k = 0; k < pathCount; ++k)
            {
                ZookeeperTreeNodeModel currentNode = pathList[k];

                if (IsNeededPath(currentNode, searchKey))
                {
                    continue;
                }

                // 从叶子节点开始一个一个删除节点，直到遇到这类节点--其后代节点中有符合查询条件的后代节点，则不删除该节点，而是退出本次循环，进入下条路径的遍历。                            
                ZookeeperTreeNodeModel node = currentNode;
                int count = 0; // 记录循环次数
                // 删位于IsNeededPath为false的某路径上的各节点的逻辑：                           
                while (node.Root != null)
                {
                    count++;

                    if (count == 1 || node.Childs.Count == 0) // 第1次循环或该节点没有孩子时，则删除该节点
                    {
                        node.Root.Childs.Remove(node);
                        node = node.Root;
                        continue;
                    }

                    // 在决定是否删有后代的节点前，先需要查它的后代中是否有符合查询条件的后代。
                    // 若有，则该节点不将被删除，而是结束本次循环，转遍历下条IsNeededPath为false的某路径；
                    // 若没有，则删除该节点，然后转处理该路径上的下个节点。
                    bool isMatchedGeneration = IsMatchedGeneration(node, searchKey);
                    if (isMatchedGeneration)
                    {
                        break;
                    }

                    node.Root.Childs.Remove(node);
                    node = node.Root;
                }
            }
        }

        //added by Yang Li
        /// <summary>
        /// 先序遍历树，实现了由树结构转为List结构
        /// </summary>
        /// <param name="rootNode"></param>
        /// <param name="front"></param>
        /// <param name="rear"></param>
        /// <param name="nodeList"></param>
        private void ConvertTreeToList(ZookeeperTreeNodeModel rootNode, int front, int rear, List<ZookeeperTreeNodeModel> nodeList)
        {
            if (rootNode == null)
            {
                return;
            }

            rear++;
            nodeList.Add(rootNode); // 根节点指针进入列表

            ZookeeperTreeNodeModel q; // 当前循环被访问的节点

            // 先序遍历树，实现了由树结构转为List结构：
            while (front != rear) // 列表不为空
            {
                front++; // front是当前节点q在list中的位置
                q = nodeList[front];

                // q节点有孩子时，将其入list：
                foreach (ZookeeperTreeNodeModel childNode in q.Childs)
                {
                    rear++;
                    nodeList.Add(childNode);
                }
            }
        }

        //added by Yang Li
        /// <summary>
        /// 判断此路径是否符合查询条件。如果不符合查询条件，则返回false；如果符合查询条件，则返回true。
        /// </summary>
        /// <param name="node">该路径上的某个节点。从该路径的叶子节点开始访问。</param>
        /// <param name="searchKey"></param>    
        /// <returns></returns>
        private bool IsNeededPath(ZookeeperTreeNodeModel node, string searchKey)
        {
            bool isNeededPath = false;

            while (node.Root != null && !node.Root.DisplayName.Equals("/"))
            {
                bool isMatch = IsMatchedNode(node, searchKey);
                if (!isMatch)
                {
                    node = node.Root; // 下轮循环中将被访问的节点
                    continue;
                }

                //node = node.Root;
                isNeededPath = true;
                return true;
            }

            if (!isNeededPath && (node.Root != null && node.Root.DisplayName.Equals("/")))
            {
                // 判断其所在路径没有符合查询条件的后代的鼻祖节点是否符合查询条件
                return IsMatchedAncestorNode(node, searchKey);
            }

            return isNeededPath;
        }

        //added by Yang Li
        /// <summary>
        /// 判断该节点（鼻祖节点除外）是否符合查询条件
        /// </summary>
        /// <param name="node"></param>
        /// <param name="searchKey"></param>     
        /// <returns></returns>
        private bool IsMatchedNode(ZookeeperTreeNodeModel node, string searchKey)
        {
            bool isMatch = node.DisplayName.ToLower().Contains(searchKey);
            return isMatch;
        }

        //added by Yang Li
        /// <summary>
        /// 判断某没有后代的鼻祖节点或其所在路径没有符合查询条件的后代的鼻祖节点是否符合查询条件
        /// </summary>
        /// <param name="node"></param>
        /// <param name="searchKey"></param>        
        /// <returns></returns>
        private bool IsMatchedAncestorNode(ZookeeperTreeNodeModel node, string searchKey)
        {
            // 如果所输的App ID查询条件，并不包含在该鼻祖节点的DisplayName内容，则该鼻祖节点不符合查询条件。
            bool isMatch = node.DisplayName.ToLower().Contains(searchKey);
            return isMatch;
        }

        //added by Yang Li      
        /// <summary>
        /// 查找该node的所有后代节点中是否有后代符合查询条件
        /// </summary>
        /// <param name="node"></param>
        /// <param name="searchKey"></param>     
        /// <returns></returns>
        private bool IsMatchedGeneration(ZookeeperTreeNodeModel node, string searchKey)
        {
            // 决定node是否能被删前，先查找该节点所有孩子中是否有孩子符合查询条件
            foreach (ZookeeperTreeNodeModel childNode in node.Childs)
            {
                if (IsMatchedNode(childNode, searchKey))
                {
                    return true;
                }

                if (childNode.Childs.Count == 0)
                {
                    continue;
                }

                // 如果该节点的某个孩子不符合查询条件，但这个孩子还有后代，则检查后代中是否有符合查询条件的后代：
                List<ZookeeperTreeNodeModel> nodeList = new List<ZookeeperTreeNodeModel>();
                ConvertTreeToList(childNode, -1, -1, nodeList);
                if (nodeList != null && nodeList.Count > 0)
                {
                    List<ZookeeperTreeNodeModel> pathList = nodeList.Where<ZookeeperTreeNodeModel>(g => g.Childs.Count == 0).ToList();
                    if (pathList != null && pathList.Count > 0)
                    {
                        foreach (ZookeeperTreeNodeModel currentNode in pathList)
                        {
                            if (IsNeededPath(currentNode, searchKey))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
