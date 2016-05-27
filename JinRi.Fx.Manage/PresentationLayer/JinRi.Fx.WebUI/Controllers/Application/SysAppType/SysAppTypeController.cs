using JinRi.Fx.Data;
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

namespace JinRi.Fx.WebUI.Controllers.SysAppType
{
    [UserAuthentication]
    public class SysAppTypeController : ControllerBaseAdmin
    {
        SysAppTypeLogic logic = new SysAppTypeLogic();
        //
        // GET: /SysAppType/
        public ActionResult Index(string typeName = "", int pageIndex = 1)
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };
            List<SysAppTypeEntity> roleList = logic.GetAppTypeList(typeName, pageItem).ToList<SysAppTypeEntity>();
            var model = new PagedList<SysAppTypeEntity>(roleList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View(model);
        }

        //
        // GET: /SysAppType/Create
        public ActionResult Create()
        {
            return View();
        }
        //
        // POST: /SysAppType/Create
        [HttpPost]
        public ActionResult Create(SysAppTypeEntity model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.AppTypeId = Convert.ToInt32(Request.Form["AppTypeId"]);
                    if (logic.GetAppTypeInfo(model.AppTypeId) == null)
                    {
                        model.TypeName = Request.Form["TypeName"];
                        logic.AddAppType(model);
                        return this.RefreshParent();
                    }
                    else
                    {
                        return this.Back("重复的应用类型编号：" + model.AppTypeId);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                return this.Back("新增应用类型该模块发生异常。" + ex.Message);
            }
        }

        //
        // GET: /SysAppType/Edit/5
        public ActionResult Edit(int id)
        {
            var model = logic.GetAppTypeInfo(id);
            return View(model);
        }

        //
        // POST: /SysAppType/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SysAppTypeEntity model = new SysAppTypeEntity();
                    model.AppTypeId = id;
                    model.TypeName = collection["TypeName"];
                    logic.UpdateAppType(model);
                    return this.RefreshParent();
                }
                return View();
            }
            catch (Exception ex)
            {
                return this.Back("修改应用类型该模块发生异常。" + ex.Message);
            }
        }
        //
        // POST: /SysAppType/Delete/5
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteAppTypeList(ids);
            return RedirectToAction("Index");
        }
    }
}
