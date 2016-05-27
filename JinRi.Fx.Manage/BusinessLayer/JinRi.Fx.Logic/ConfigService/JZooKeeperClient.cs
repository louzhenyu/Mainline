using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System.Threading;
using ZooKeeperNet;
using Org.Apache.Zookeeper.Data;
using log4net;

namespace JinRi.Fx.Logic
{
    public class JZooKeeperClient
    {
        private static ILog log = LogManager.GetLogger(typeof(JZooKeeperClient));

        public bool GetNodeList(TreeZNode root, List<TreeZNode> nodeList, ref string message)
        {
            try
            {
                using (ZooKeeper zk = new ZooKeeper(AppSettingsHelper.ZKServer, TimeSpan.FromSeconds(AppSettingsHelper.ZKSessionTimeOut), null))
                {
                    ZooKeeper.WaitUntilConnected(zk);
                   
                    GetNodeListLoop(zk, root, nodeList);                    
                }              
            }
            catch (Exception ex)
            {
                message = string.Format("JinRi.Fx.Manage:{0}GetNodeList()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                log.Error(message);
                message = string.Format("GetNodeList()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                return false;
            }
           
            return true;
        }

        public bool GetNodeValue(string nodePath, ref string nodeValue, ref string message)
        {
            nodeValue = string.Empty;

            try
            {
                using (ZooKeeper zk = new ZooKeeper(AppSettingsHelper.ZKServer, TimeSpan.FromSeconds(AppSettingsHelper.ZKSessionTimeOut), null))
                {
                    ZooKeeper.WaitUntilConnected(zk);

                    Stat stat = zk.Exists(nodePath, true);
                    if (stat == null)
                    {
                        message = string.Format("该节点【{0}】已被别人删除。", nodePath);
                        return false;
                    }

                    byte[] _nodeValue = zk.GetData(nodePath, false, stat);
                    if (_nodeValue != null && _nodeValue.Length > 0)
                    {
                        nodeValue = Encoding.UTF8.GetString(_nodeValue);
                    }
                }
            }
            catch (Exception ex)
            {
                message = string.Format("JinRi.Fx.Manage:{0}GetNodeValue()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                log.Error(message);
                message = string.Format("GetNodeValue()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                return false;
            }

            return true;
        }

        public bool AddNode(string nodePath, string nodeValue, ref string message)
        {
            try
            {
                using (ZooKeeper zk = new ZooKeeper(AppSettingsHelper.ZKServer, TimeSpan.FromSeconds(AppSettingsHelper.ZKSessionTimeOut), null))
                {
                    ZooKeeper.WaitUntilConnected(zk);

                    if (zk.Exists(nodePath, false) == null)
                    {
                        zk.Create(nodePath, Encoding.UTF8.GetBytes(nodeValue), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                        return true;
                    }
                    else
                    {
                        zk.Exists(nodePath, true);
                        message = string.Format("该节点【{0}】已被别人新增。", nodePath);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                message = string.Format("JinRi.Fx.Manage:{0}AddNode()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                log.Error(message);
                message = string.Format("AddNode()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                return false;
            }
        }

        public bool DeleteNode(string nodePath, ref string message)
        {
            try
            {
                using (ZooKeeper zk = new ZooKeeper(AppSettingsHelper.ZKServer, TimeSpan.FromSeconds(AppSettingsHelper.ZKSessionTimeOut), null))
                {
                    ZooKeeper.WaitUntilConnected(zk);

                    Stat stat = zk.Exists(nodePath, false);
                    if (stat != null)
                    {
                        zk.Delete(nodePath, stat.Version);
                    }
                }
            }
            catch (Exception ex)
            {
                message = string.Format("JinRi.Fx.Manage:{0}DeleteNode()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                log.Error(message);
                message = string.Format("DeleteNode()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                return false;
            }

            return true;
        }

        public bool UpdateNodeValue(string nodePath, string newNodeValue, ref string message)
        {
            try
            {
                byte[] newData = Encoding.UTF8.GetBytes(newNodeValue);

                using (ZooKeeper zk = new ZooKeeper(AppSettingsHelper.ZKServer, TimeSpan.FromSeconds(AppSettingsHelper.ZKSessionTimeOut), null))
                {
                    ZooKeeper.WaitUntilConnected(zk);

                    Stat stat = zk.Exists(nodePath, false);
                    if (stat == null)
                    {
                        message = string.Format("该节点【{0}】已被别人删除。", nodePath);
                        return false;
                    }

                    byte[] oldValue = zk.GetData(nodePath, false, stat);
                    if (oldValue == null || oldValue.Length < 1)
                    {
                        if (string.IsNullOrWhiteSpace(newNodeValue))
                        {
                            message = "和原来的值一样，所以无需更新。";
                            return false;
                        }
                    }
                    string oldValueStr = string.Empty;
                    if (oldValue != null && oldValue.Length > 0)
                    {
                        oldValueStr = Encoding.UTF8.GetString(oldValue);
                    }
                    if (newNodeValue.Equals(oldValueStr))
                    {
                        message = "和原来的值一样，所以无需更新。";
                        return false;
                    }

                    Stat newStat = zk.SetData(nodePath, newData, stat.Version);
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = string.Format("JinRi.Fx.Manage:{0}UpdateNodeValue()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                log.Error(message);
                message = string.Format("UpdateNodeValue()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                return false;
            }
        }

        void GetNodeListLoop(ZooKeeper zk, TreeZNode root, List<TreeZNode> nodeList)
        {
            int i = 0;
            try
            {
                foreach (string child in zk.GetChildren(root.NodePath, true))
                {
                    ++i;
                    int id = int.Parse(string.Format("{0}{1}", root.id, i));
                    TreeZNode childnode = new TreeZNode(id, true, string.Concat(root.joinNodePath, "/", child), child, root.id);
                    root.ChildNodeList.Add(childnode);
                    nodeList.Add(childnode);
                    GetNodeListLoop(zk, childnode, nodeList);
                }
            }
            catch(Exception ex)
            {
                string message = string.Format("JinRi.Fx.Manage:{0}GetNodeListLoop()方法抛异常：{0}[{1}]{2}。", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ex.ToString());
                log.Error(message);               
            }            
        }
    }
}
