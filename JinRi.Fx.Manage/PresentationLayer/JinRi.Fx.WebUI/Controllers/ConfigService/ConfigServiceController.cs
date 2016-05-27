using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using JinRi.Fx.Web;
using JinRi.Fx.Logic;
using JinRi.Fx.ResponseDTO.ConfigService;
using JinRi.Fx.Entity;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using JinRi.Fx.Utility;

namespace JinRi.Fx.WebUI.Controllers.ConfigService
{
    public class ConfigServiceController : ControllerBaseAdmin
    {
        private static JZooKeeperClient me = null;

        [UserAuthentication]
        public ActionResult Index()
        {
            return View();
        }

        [UserAuthentication]
        public string LoadTree(int? id)
        {
            LoadTreeResponseDTO result = new LoadTreeResponseDTO();
            JavaScriptSerializer serialize = new JavaScriptSerializer();

            result.ZNodeList = new List<TreeZNode>();

            TreeZNode root = new TreeZNode(1, true, "/", "/", 0);
            result.ZNodeList.Add(root);
            me = new JZooKeeperClient();
            result.IsSuccess = me.GetNodeList(root, result.ZNodeList, ref result.Message);
            if (!result.IsSuccess)
            {
                return serialize.Serialize(result);
            }

            /*
            List<TreeZNode> treeNodeList = result.ZNodeList.Where(i => i.NodePath.IndexOf("/ConfigService") > -1 && i.pId == (id ?? 0)).OrderBy(x => x.name).ToList<TreeZNode>();
            */
            ///*
            List<TreeZNode> treeNodeList = result.ZNodeList.Where(i => i.NodePath.IndexOf("/ConfigService") > -1).OrderBy(x => x.name).ToList<TreeZNode>();
            //*/
            if (treeNodeList == null || treeNodeList.Count < 1)
            {
                result.ZNodeList = treeNodeList;
                result.IsSuccess = false;
                result.Message = "目前没有配置项数据；请联系运维或系统管理员加【ConfigService】配置项。";

                return serialize.Serialize(result);
            }

            //根据配置中心权限的设置显示节点：        
            LoginUserInfo loginUserInfo = this.WorkContext.CurrentUser;
            int loginRoleId = loginUserInfo.RoleId;
            int loginUserId = loginUserInfo.UserId;
            switch (loginRoleId)
            {
                case 2://2表示系统管理员角色
                case 4://4表示运维角色              
                    break;
                case 7://7表示国内开发主管角色
                case 8://8表示国际开发主管角色
                    SysRoleRightLogic logic = new SysRoleRightLogic();

                    SysRoleRight sysRoleRight = logic.GetRoleRight(true, loginRoleId, 0, 0, AppSettingsHelper.ConfigCenterMenuName, 0);
                    if (sysRoleRight == null)
                    {
                        result.IsSuccess = false;
                        result.Message = "很抱歉，您没有权限进行配置；请联系运维或系统管理员开通下设置页面的访问权限！";
                        break;
                    }

                    List<SysRoleRight> roleRightList = logic.GetRoleRightList(loginRoleId, true, loginUserId).ToList<SysRoleRight>();
                    if (roleRightList == null || roleRightList.Count < 1)
                    {
                        treeNodeList = treeNodeList.Where(node => node.NodePath.Equals("/ConfigService")).ToList<TreeZNode>();
                        break;
                    }

                    List<string> appIdStringList = roleRightList.Select(r => r.AppIdString).ToList<string>();
                    List<TreeZNode> tempList = treeNodeList.Where(n => n.NodePath.Equals("/ConfigService")).ToList<TreeZNode>();
                    foreach (string appId in appIdStringList)
                    {
                        List<TreeZNode> list = treeNodeList.Where(n => n.NodePath.IndexOf("/ConfigService/" + appId) > -1).ToList();
                        if (list != null && list.Count > 0)
                        {
                            tempList.InsertRange(tempList.Count - 1, list);
                        }
                    }
                    treeNodeList = tempList;
                    break;
                default://只有具有系统管理员、运维、国内/国际开发主管角色的登入用户才有权限进行配置                  
                    result.IsSuccess = false;
                    result.Message = "很抱歉，您没有权限进行配置！";
                    break;
            }

            result.ZNodeList = treeNodeList.OrderBy(x => x.name).ToList<TreeZNode>();

            return serialize.Serialize(result);
        }

        [UserAuthentication]
        public string GetNodeValue(string nodePath)
        {
            LoadTreeResponseDTO result = new LoadTreeResponseDTO();
            JavaScriptSerializer serialize = new JavaScriptSerializer();

            if (me == null)
            {
                return serialize.Serialize(result);
            }

            result.IsSuccess = me.GetNodeValue(nodePath, ref result.NodeValue, ref result.Message);
            return serialize.Serialize(result);
        }

        [UserAuthentication]
        public string AddNode(string nodePath, string nodeValue)
        {
            LoadTreeResponseDTO result = new LoadTreeResponseDTO();
            JavaScriptSerializer serialize = new JavaScriptSerializer();

            if (me == null)
            {
                return serialize.Serialize(result);
            }

            if (!ValidateRight(nodePath, result, true))
            {
                return serialize.Serialize(result);
            }

            result.IsSuccess = me.AddNode(nodePath, nodeValue, ref result.Message);
            //记Redis：
            if (result.IsSuccess && !string.IsNullOrWhiteSpace(nodeValue))
            {
                CacheManager.SetCache<string>(nodePath, nodeValue, new TimeSpan(183, 0, 0, 0, 0));
            }
            return serialize.Serialize(result);
        }

        [UserAuthentication]
        public string DeleteNode(string nodePath)
        {
            LoadTreeResponseDTO result = new LoadTreeResponseDTO();
            JavaScriptSerializer serialize = new JavaScriptSerializer();

            if (me == null)
            {
                return serialize.Serialize(result);
            }

            if (!ValidateRight(nodePath, result, false))
            {
                return serialize.Serialize(result);
            }

            result.IsSuccess = me.DeleteNode(nodePath, ref result.Message);
            //从Redis中删除：
            if(result.IsSuccess && CacheManager.ContainsKey(nodePath))
            {
                CacheManager.ClearCache(nodePath);
            }
            return serialize.Serialize(result);
        }

        [UserAuthentication]
        public string UpdateNodeValue(string nodePath, string newNodeValue)
        {
            LoadTreeResponseDTO result = new LoadTreeResponseDTO();
            JavaScriptSerializer serialize = new JavaScriptSerializer();

            if (me == null)
            {
                return serialize.Serialize(result);
            }

            if (!ValidateRight(nodePath, result, false))
            {
                return serialize.Serialize(result);
            }

            if (!string.IsNullOrWhiteSpace(newNodeValue))
            {
                newNodeValue = Regex.Replace(newNodeValue, @"\r|\n", " ");
            }
            result.IsSuccess = me.UpdateNodeValue(nodePath, newNodeValue, ref result.Message);
            //记Redis：
            if (result.IsSuccess)
            {
                CacheManager.SetCache<string>(nodePath, newNodeValue, new TimeSpan(183, 0, 0, 0, 0));
            }
            return serialize.Serialize(result);
        }

        /// <summary>
        /// 验证该登入用户是否有权限设置配置项
        /// </summary>
        /// <param name="nodePath"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool ValidateRight(string nodePath, LoadTreeResponseDTO result, bool isAddNode)
        {
            LoginUserInfo loginUserInfo = this.WorkContext.CurrentUser;

            string[] s = nodePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            int appId = 0;
            if (s.Length < 2)
            {
                result.IsSuccess = false;
                result.Message = string.Format("很抱歉，参数nodePath【{0}】传错，请联系系统管理员处理！", nodePath);
                return false;
            }
            int.TryParse(s[1], out appId);
            if (appId < 1)
            {
                result.IsSuccess = false;                
                result.Message = string.Format("根节点【ConfigService】下的第一层节点应是应用程序ID号，请联系系统管理员处理！", nodePath);
                if (isAddNode)
                {
                    result.Message = string.Format("根节点【ConfigService】下的第一层节点应是应用程序ID号！", nodePath);
                }
                return false;
            }

            SysRoleRightLogic logic = new SysRoleRightLogic();
            SysRoleRight sysRoleRight = logic.GetRoleRight(false, loginUserInfo.RoleId, loginUserInfo.UserId, 0, AppSettingsHelper.ConfigCenterMenuName, appId);
            if (sysRoleRight == null)
            {
                result.IsSuccess = false;
                result.Message = string.Format("很抱歉，您没有权限操作该应用【{0}】下的各配置项；请先为该应用开通下设置权限！", appId);
                return false;
            }

            return true;
        }
    }
}
