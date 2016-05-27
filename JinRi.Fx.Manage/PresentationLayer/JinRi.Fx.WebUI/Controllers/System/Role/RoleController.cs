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

namespace JinRi.Fx.WebUI.Controllers.Role
{
    [UserAuthentication]
    public class RoleController : ControllerBaseAdmin
    {
        SysRoleLogic logic = new SysRoleLogic();
        //
        // GET: /Role/
        public ActionResult Index(string roleName = "",int systemId=-1, int status = -1, int pageIndex = 1)
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };
            List<SysRole> roleList = logic.GetRoleList(status, roleName, systemId, pageItem).ToList<SysRole>();
            var model = new PagedList<SysRole>(roleList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View(model);
        }

        //
        // GET: /Role/Create
        public ActionResult Create()
        {
            SetViewBagData();
            return View();
        }
        void SetViewBagData()
        {
            SysRoleLogic roleLogic = new SysRoleLogic();
            List<SysRole> roleList = roleLogic.GetRoleList().ToList<SysRole>();
            var modelState = EnumHelper.GetItemValueList<EntityStatus>();
            this.ViewBag.ModelState = new SelectList(modelState, "Key", "Value"); 
        }
        //
        // POST: /Role/Create
        [HttpPost]
        public ActionResult Create(SysRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    role.Status = Convert.ToInt32(Request.Form["ModelState"]);
                    role.Remark = Request.Form["Remark"];
                    logic.AddRole(role);
                    return this.RefreshParent();
                }
                SetViewBagData();
                return View();
            }
            catch (Exception ex)
            {
                return this.Back("新增角色改模块发生异常。" + ex.Message);
            }
        }

        //
        // GET: /Role/Edit/5
        public ActionResult Edit(int id)
        {
            var model = logic.GetRoleInfo(id);
            SetViewBagData();
            return View(model);
        }

        //
        // POST: /Role/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SysRole model = new SysRole();
                    model.RoleId = id;
                    model.RoleName = collection["RoleName"];
                    model.SystemId = Convert.ToInt32(collection["SystemId"]);
                    model.Status = Convert.ToInt32(collection["ModelState"]);
                    model.Remark = collection["Remark"];
                    logic.UpdateRole(model);
                    return this.RefreshParent();
                }
                SetViewBagData();
                return View();
            }
            catch (Exception ex)
            {
                return this.Back("修改角色改模块发生异常。" + ex.Message);
            }
        }
        //
        // POST: /Role/Delete/5
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteRoleList(ids);
            return RedirectToAction("Index");
        }
    }
}
