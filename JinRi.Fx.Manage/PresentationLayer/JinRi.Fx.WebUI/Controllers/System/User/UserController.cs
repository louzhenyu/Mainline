using JinRi.Fx.Entity;
using JinRi.Fx.Logic;
using JinRi.Fx.Utility;
using JinRi.Fx.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace JinRi.Fx.WebUI.Controllers.User
{
    [UserAuthentication]
    public class UserController : ControllerBaseAdmin
    {
        SysUserLogic logic = new SysUserLogic();
        SysRoleLogic roleLogic = new SysRoleLogic();
        //
        // GET: /User/
        public ActionResult Index(string userName = "", int roleId = -1, int status = -1, int pageIndex = 1)
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };
            List<SysUser> userList = logic.GetUserList(userName, roleId, status, pageItem).ToList<SysUser>();
            var model = new PagedList<SysUser>(userList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);

            Dictionary<int, SysRole> dictionaryRole = new Dictionary<int, SysRole>();

            List<SysRole> roleList = roleLogic.GetRoleList(0).ToList<SysRole>();
            if (roleList != null)
            {
                for (int index = 0; index < roleList.Count; index++)
                {
                    dictionaryRole.Add(roleList[index].RoleId, roleList[index]);
                }
            }
            ViewBag.DictionaryRole = dictionaryRole;

            return View(model);
        }

        //
        // GET: /User/Create
        public ActionResult Create()
        {
            List<SysRole> roleList = roleLogic.GetRoleList(0).ToList<SysRole>();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");

            var modelState = EnumHelper.GetItemValueList<EntityStatus>();
            this.ViewBag.ModelState = new SelectList(modelState, "Key", "Value");
            return View();
        }

        //
        // POST: /User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                SysUser model = new SysUser();
                model.UserName = collection["UserName"];
                model.RoleId = Convert.ToInt32(collection["RoleList"]);
                model.Status = Convert.ToInt32(collection["ModelState"]);
                model.RealName = collection["RealName"];
                model.Tel = collection["Tel"];
                model.Email = collection["Email"];
                if (logic.GetUserInfo(model.UserName) == null)
                {
                    logic.AddUser(model);
                    return this.RefreshParent();
                }
                else
                {
                    return this.Back("用户名重复");
                }
            }
            catch(Exception ex)
            {
                return this.Back("新增用户发生异常。" + ex.Message);
            }
        }

        //
        // GET: /User/Edit/5
        public ActionResult Edit(int id)
        {
            var model = logic.GetUserInfo(id);

            List<SysRole> roleList = roleLogic.GetRoleList(0).ToList<SysRole>();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName", model.RoleId);

            var modelState = EnumHelper.GetItemValueList<EntityStatus>();
            this.ViewBag.ModelState = new SelectList(modelState, "Key", "Value", model.Status);
            return View(model);
        }

        //
        // POST: /User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                SysUser model = new SysUser();
                model.UserId = id;
                model.RoleId = Convert.ToInt32(collection["RoleList"]);
                model.Status = Convert.ToInt32(collection["ModelState"]);
                model.RealName = collection["RealName"];
                model.Tel = collection["Tel"];
                model.UserName = collection["UserName"];
                model.Email = collection["Email"];
                logic.UpdateUser(model);
                return this.RefreshParent();
            }
            catch(Exception ex)
            {
                return this.Back("修改用户发生异常。" + ex.Message);
            }
        }
        //
        // POST: /User/Delete/5
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteUserList(ids);
            return RedirectToAction("Index");
        }
    }
}
