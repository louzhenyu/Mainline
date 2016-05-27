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

namespace JinRi.Fx.WebUI.Controllers.Menu
{
    public class MenuController : ControllerBaseAdmin
    {
        SysMenuLogic logic = new SysMenuLogic();
        //
        // GET: /Menu/

        public ActionResult Index(string menuName = "", int moduleId = -1, int status = -1, int pageIndex = 1)
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };

            SysModuleLogic moduleLogic = new SysModuleLogic();
            List<SysModule> moduleList = moduleLogic.GetModuleList().ToList<SysModule>();

            Dictionary<int, SysModule> dictionaryModule = new Dictionary<int, SysModule>();
            for (int index = 0; index < moduleList.Count; index++)
            {
                dictionaryModule.Add(moduleList[index].ModuleId, moduleList[index]);
            }
            moduleList.Insert(0, new SysModule { ModuleId = -1, ModuleName = "全部" });
            ViewBag.ModuleId = new SelectList(moduleList, "ModuleId", "ModuleName");
            ViewBag.DictionaryModule = dictionaryModule;

            List<SysMenu> menuList = logic.GetMenuList(status, menuName, moduleId, pageItem).ToList<SysMenu>();
            ViewBag.MenuList = new PagedList<SysMenu>(menuList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View();
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
                SysMenu model = new SysMenu();
                model.MenuName = collection["MenuName"];
                model.MenuUrl = collection["MenuUrl"];
                model.ImageUrl = collection["ImageUrl"];
                model.ModuleId = Convert.ToInt32(collection["ModuleList"]);
                model.Sort = Convert.ToInt32(collection["Sort"]);
                model.Status = Convert.ToInt32(collection["ModelState"]);
                model.Remark = collection["Remark"];
                logic.AddMenu(model);
                return this.RefreshParent();
            }
            catch(Exception ex)
            {
                return this.Back("新增菜单发生异常。" + ex.Message);
            }
        }

        //
        // GET: /Menu/Edit/5

        public ActionResult Edit(int id)
        {
            var model = logic.GetMenuInfo(id);

            SysModuleLogic moduleLogic =new SysModuleLogic();
            List<SysModule> moduleList = moduleLogic.GetModuleList(0).ToList<SysModule>();
            this.ViewBag.ModuleList = new SelectList(moduleList, "ModuleId",  "ModuleName",model.ModuleId);

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
                SysMenu model = new SysMenu();
                model.MenuId = id;
                model.MenuName = collection["MenuName"];
                model.MenuUrl = collection["MenuUrl"];
                model.ImageUrl = collection["ImageUrl"];
                model.ModuleId = Convert.ToInt32(collection["ModuleList"]);
                model.Sort = Convert.ToInt32(collection["Sort"]);
                model.Status = Convert.ToInt32(collection["ModelState"]);
                model.Remark = collection["Remark"];
                logic.UpdateMenu(model);
                return this.RefreshParent();
            }
            catch(Exception ex)
            {
                return this.Back("修改菜单发生异常。" + ex.Message);
            }
        }
        //
        // POST: /Menu/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteMenuList(ids);
            return RedirectToAction("Index");
        }
    }
}
