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

namespace JinRi.Fx.WebUI.Controllers.Module
{
    [UserAuthentication]
    public class ModuleController : ControllerBaseAdmin
    {
        SysModuleLogic logic = new SysModuleLogic();
        //
        // GET: /Menu/

        public ActionResult Index(string moduleName = "", int status = -1, int pageIndex = 1)
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };
            List<SysModule> menuList = logic.GetModuleList(status, moduleName, -1, pageItem).ToList<SysModule>();
            var model = new PagedList<SysModule>(menuList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View(model);
        }

        //
        // GET: /Menu/Create

        public ActionResult Create()
        {
            SysModuleLogic moduleLogic = new SysModuleLogic();
            List<SysModule> moduleList = moduleLogic.GetModuleList(0).ToList<SysModule>();
            this.ViewBag.ModuleList = new SelectList(moduleList, "ModuleId", "ModuleName");

            var modelState = EnumHelper.GetItemValueList<EntityStatus>();
            this.ViewBag.ModelState = new SelectList(modelState, "Key", "Value");
            return View();
        }

        //
        // POST: /Menu/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                SysModule model = new SysModule();
                model.ModuleName = collection["ModuleName"];
                model.ModuleUrl = collection["ModuleUrl"];
                model.ImageUrl = collection["ImageUrl"];
                model.Sort = Convert.ToInt32(collection["Sort"]);
                model.Status = Convert.ToInt32(collection["ModelState"]);
                model.Remark = collection["Remark"];
                logic.AddModule(model);
                return this.RefreshParent();
            }
            catch(Exception ex)
            {
                return this.Back("新增模块发生异常。" + ex.Message);
            }
        }

        //
        // GET: /Menu/Edit/5

        public ActionResult Edit(int id)
        {
            var model = logic.GetModuleInfo(id);
            var modelState = EnumHelper.GetItemValueList<EntityStatus>();
            this.ViewBag.ModelState = new SelectList(modelState, "Key", "Value", model.Status);
            return View(model);
        }

        //
        // POST: /Menu/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                SysModule model = new SysModule();
                model.ModuleId = id;
                model.ModuleName = collection["ModuleName"];
                model.ModuleUrl = collection["ModuleUrl"];
                model.ImageUrl = collection["ImageUrl"];
                model.Sort = Convert.ToInt32(collection["Sort"]);
                model.Status = Convert.ToInt32(collection["ModelState"]);
                model.Remark = collection["Remark"];
                logic.UpdateModule(model);
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                return this.Back("修改模块发生异常。" + ex.Message);
            }
        }
        //
        // POST: /Menu/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteModuleList(ids);
            return RedirectToAction("Index");
        }
    }
}
