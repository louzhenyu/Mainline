using JinRi.Fx.Entity;
using JinRi.Fx.Logic;
using JinRi.Fx.Utility;
using JinRi.Fx.Web;
using JinRi.Fx.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace JinRi.Fx.WebUI.Controllers.RoleRight
{
    [UserAuthentication]
    public class RoleRightController : ControllerBaseAdmin
    {
        SysRoleRightLogic logic = new SysRoleRightLogic();
        //
        // GET: /RoleRight/

        [UserAuthentication]
        public ActionResult Index(int roleId = -1)
        {
            ViewBag.CurrentRoleId = roleId;

            SysRoleLogic roleLogic = new SysRoleLogic();
            List<SysRole> roleList = roleLogic.GetRoleList().ToList<SysRole>();
            ViewBag.RoleId = new SelectList(roleList, "RoleId", "RoleName");

            List<SysRoleRight> roleRightList = new List<SysRoleRight>();

            if (roleId > 0)
            {
                SysModuleLogic moduleLogic = new SysModuleLogic();
                List<SysModule> moduleList = moduleLogic.GetModuleList().ToList<SysModule>();
                ViewBag.ModuleList = moduleList;

                SysMenuLogic menuLogic = new SysMenuLogic();
                List<SysMenu> menuList = menuLogic.GetMenuList(0).ToList<SysMenu>();
                ViewBag.MenuList = menuList;

                roleRightList = logic.GetRoleRightList(roleId, false).ToList<SysRoleRight>();
            }

            return View(roleRightList);
        }

        //
        // POST: /RoleRight/Save/5

        [HttpPost]
        public ActionResult Save(FormCollection collection, [Bind(Prefix = "rList")]List<RoleRightModel> list)
        {
            List<SysRoleRight> roleRightList = new List<SysRoleRight>();
            int roleId = Convert.ToInt32(collection["RoleId"]);
            int userId = Convert.ToInt32(collection["UserId"]);
            List<RoleRightModel> modelList = list.Where(m => m.Checked).ToList<RoleRightModel>();
            for (int index = 0; index < modelList.Count; index++)
            {
                roleRightList.Add(new SysRoleRight { RoleId = roleId, UserId = userId, MenuId = modelList[index].MenuId, AppId = modelList[index].AppId });
            }

            int roleRightType = Convert.ToInt32(collection["RoleRightType"]);
            if (roleRightType == 2) //进入配置中心权限管理页
            {
                logic.SaveRoleRight(roleId, roleRightList, true, userId);
                return RedirectToAction("ConfigServiceRight", new { RoleId = roleId, UserId = userId });
            }
            logic.SaveRoleRight(roleId, roleRightList);
            return RedirectToAction("Index", new { RoleId = roleId });
        }

        [UserAuthentication]
        public ActionResult ConfigServiceRight(int roleId = -1, int userId = 0)
        {            
            //具有国内/国际开发主管角色的登入用户进入该页后，角色下拉列表只列出国内/国际开发主管，
            //否则，列出国内开发主管和国际开发主管两角色：
            SysRoleLogic roleLogic = new SysRoleLogic();
            List<SysRole> roleList = roleLogic.GetRoleList().ToList<SysRole>();            
            LoginUserInfo currentLogin = this.WorkContext.CurrentUser;
            //7表示国内开发主管角色，8表示国际开发主管角色
            if (currentLogin.RoleId == 7 || currentLogin.RoleId == 8)
            {
                roleList = roleList.Where(x => x.RoleId == currentLogin.RoleId).ToList<SysRole>();
            }
            else
            {
                roleList = roleList.Where(x => (x.RoleId == 7 || x.RoleId == 8)).ToList<SysRole>();
            }
            ViewBag.RoleId = new SelectList(roleList, "RoleId", "RoleName");
            if (roleId < 1)
            {
                if (currentLogin.RoleId == 7 || currentLogin.RoleId == 8)
                {
                    ViewBag.CurrentRoleId = currentLogin.RoleId;
                }
                else
                {
                    ViewBag.CurrentRoleId = 7;
                }
            }
            else
            {
                ViewBag.CurrentRoleId = roleId;
            }
            
            List<SysUser> userList = GetUserList(roleId);
            ViewBag.UserId = new SelectList(userList, "UserId", "RealName");
            ViewBag.CurrentUserId = userId;

            List<SysRoleRight> roleRightList = new List<SysRoleRight>();
            if (roleId > 0)
            {
                SysApplicationLogic appIdMenuLogic = new SysApplicationLogic();

                //求配置中心权限管理模块的各菜单项：
                List<SysApplicationEntity> appIdMenuList = null;
                SysUserLogic sysUserLogic = new SysUserLogic();
                SysUser sysUser = null;
                int? selectedUserId = -1;
                string selectedUserName = string.Empty;
                switch (roleId)
                {
                    case 7://国内开发主管角色，目前只求针对国内机票产品线的配置中心权限管理模块的各菜单项：                        
                        sysUser = sysUserLogic.GetUserInfo(userId);
                        if (sysUser != null)
                        {
                            selectedUserName = sysUser.RealName;
                            ViewBag.CurrentUserName = selectedUserName;
                        }
                        appIdMenuList = appIdMenuLogic.GetSysApplicationList(-1, -1, "", -1, -1, null, new List<int> { 1 }, selectedUserName).ToList<SysApplicationEntity>();                        
                        break;
                    case 8: //国际开发主管角色，目前只求针对国际机票产品线的配置中心权限管理模块的各菜单项：                       
                        sysUser = sysUserLogic.GetUserInfo(userId);
                        if (sysUser != null)
                        {
                            selectedUserName = sysUser.RealName;
                            ViewBag.CurrentUserName = selectedUserName;
                        }
                        appIdMenuList = appIdMenuLogic.GetSysApplicationList(-1, -1, "", -1, -1, null, new List<int> { 2 }, selectedUserName).ToList<SysApplicationEntity>();                        
                        break;                 
                    default:
                        break;
                }
                ViewBag.AppIdMenuList = appIdMenuList;

                selectedUserId = ViewBag.CurrentUserId as int?;
                roleRightList = logic.GetRoleRightList(roleId, true, (selectedUserId.HasValue ? Convert.ToInt32(selectedUserId) : -1)).ToList<SysRoleRight>();                
            }

            return View(roleRightList);
        }
   
        public JsonResult BindUserDropDownList(int roleId)
        {
            List<SysUser> userList = GetUserList(roleId);

            SelectList result = new SelectList(userList, "UserId", "RealName");
            ViewBag.UserId = result;           
            return Json(result.Items, JsonRequestBehavior.AllowGet);
        }

        private List<SysUser> GetUserList(int roleId)
        {
            LoginUserInfo currentLogin = this.WorkContext.CurrentUser;

            if (roleId < 1)
            {                
                if (currentLogin.RoleId == 7 || currentLogin.RoleId == 8)
                {
                    roleId = currentLogin.RoleId;
                }
                else
                {
                    roleId = 7;
                }
            }            

            //具有国内/国际开发主管角色的登入用户进入该页后，用户名下拉列表只列出自己的名字；
            //具有系统管理员角色或运维角色的登入用户进入该页后，用户名下拉列表列出符合角色下拉列表框中所选中角色的所有用户；
            //具有其他角色的登入用户进入该页后，用户名下拉列表框没有用户可选：
            List<SysUser> userList = new List<SysUser>();
            
            SysUserLogic sysUserLogic = new SysUserLogic();
            if ((currentLogin.RoleId == 7 || currentLogin.RoleId == 8) && currentLogin.RoleId == roleId)//7表示国内开发主管角色，8表示国际开发主管角色
            {
                userList = sysUserLogic.GetUserList(currentLogin.UserName, currentLogin.RoleId, 0).ToList<SysUser>();
            }
            else if (currentLogin.RoleId == 2 || currentLogin.RoleId == 4)//2表示系统管理员角色，4表示运维角色
            {
                userList = sysUserLogic.GetUserList("", -1, 0).ToList<SysUser>();
                userList = userList.Where(x => x.RoleId == roleId).ToList<SysUser>();
            }

            return userList;
        }
    }
}
