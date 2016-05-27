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
using JFx;
using JinRi.Fx.RequestDTO;
using JinRi.Fx.WebUI.Models;

namespace JinRi.Fx.WebUI.Controllers.Application
{
    public class SysApplicationController : ControllerBaseAdmin
    {
        SysApplicationLogic logic = new SysApplicationLogic();
        SysSubLogic subSystemLogic = new SysSubLogic();
        SysAppTypeLogic appTypeLogic = new SysAppTypeLogic();
        //
        // GET: /SysApplication/

        public ActionResult Index(SysApplicationSearchArgs arg, int pageIndex = 1)
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };

            int appId = arg.AppId.ConvertTo<int>(-1);
            List<SysSubEntity> subSystemList = subSystemLogic.GetSubSystemList().ToList<SysSubEntity>();
            List<SysAppTypeEntity> appTypeList = appTypeLogic.GetAppTypeList().ToList<SysAppTypeEntity>();

            ViewBag.DictionaryAppType = appTypeList.ToDictionary(k => k.AppTypeId, v => v);
            ViewBag.DictionarySubSystem = subSystemList.ToDictionary(k => k.SubSystemId, v => v);

            appTypeList.Insert(0, new SysAppTypeEntity() { AppTypeId = -1, TypeName = "全部" });
            subSystemList.Insert(0, new SysSubEntity() { SubSystemId = -1, SystemName = "全部" });
            ViewBag.AppTypeId = new SelectList(appTypeList, "AppTypeId", "TypeName");
            ViewBag.SubSystemId = new SelectList(subSystemList, "SubSystemId", "SystemName");

            List<SysApplicationEntity> sysApplicationList = logic.GetSysApplicationList(appId, arg.SubSystemId, arg.AppName, arg.AppTypeId, arg.Status, pageItem).ToList<SysApplicationEntity>();
            ViewBag.SysApplicationList = new PagedList<SysApplicationEntity>(sysApplicationList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View(arg);
        }

        //
        // GET: /SysApplication/Create

        public ActionResult Create()
        {
            var subSystemList = subSystemLogic.GetSubSystemList().ToList<Fx.Entity.SysSubEntity>();
            this.ViewBag.SubSystemId = new SelectList(subSystemList, "SubSystemId", "SystemName");

            var AppTypeList = appTypeLogic.GetAppTypeList().ToList<Fx.Entity.SysAppTypeEntity>();
            this.ViewBag.AppTypeId = new SelectList(AppTypeList, "AppTypeId", "TypeName");

            var appState = EnumHelper.GetItemValueList<EntityStatus>();
            this.ViewBag.AppState = new SelectList(appState, "Key", "Value");

            return View();
        }
        //
        // POST: /SysApplication/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                SysApplicationEntity model = new SysApplicationEntity();
                model.AppId = Convert.ToInt32(collection["AppId"]);
                if (logic.GetSysApplicationInfo(model.AppId) == null)
                {
                    model.SubSystemId = Convert.ToInt32(collection["SubSystemId"]);
                    model.AppName = collection["AppName"];
                    model.AppEName = collection["AppEName"];
                    model.AppTypeId = Convert.ToInt32(collection["AppTypeId"]);
                    model.Owner = collection["Owner"];
                    model.Status = Convert.ToInt32(collection["AppState"]);
                    model.Remark = collection["Remark"];
                    logic.AddSysApplication(model);
                    return this.RefreshParent();
                }
                else
                {
                    return this.Back("重复的应用编号：" + model.AppId);
                }
            }
            catch (Exception ex)
            {
                return this.Back("新增应用发生异常。" + ex.Message);
            }
        }

        //
        // GET: /SysApplication/Edit/5

        public ActionResult Edit(int id)
        {
            var subSystemList = subSystemLogic.GetSubSystemList().ToList<Fx.Entity.SysSubEntity>();
            this.ViewBag.SubSystemId = new SelectList(subSystemList, "SubSystemId", "SystemName");

            var AppTypeList = appTypeLogic.GetAppTypeList().ToList<Fx.Entity.SysAppTypeEntity>();
            this.ViewBag.AppTypeId = new SelectList(AppTypeList, "AppTypeId", "TypeName");

            var appState = EnumHelper.GetItemValueList<EntityStatus>();
            this.ViewBag.AppState = new SelectList(appState, "Key", "Value");

            var model = logic.GetSysApplicationInfo(id);
            return View(model);
        }

        //
        // POST: /SysApplication/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Fx.Entity.SysApplicationEntity model = new Fx.Entity.SysApplicationEntity();
                model.AppId = id;
                model.SubSystemId = Convert.ToInt32(collection["SubSystemId"]);
                model.AppName = collection["AppName"];
                model.AppEName = collection["AppEName"];
                model.AppTypeId = Convert.ToInt32(collection["AppTypeId"]);
                model.Owner = collection["Owner"];
                model.Status = Convert.ToInt32(collection["AppState"]);
                model.Remark = collection["Remark"];
                logic.UpdateSysApplication(model);
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                return this.Back("修改应用发生异常。" + ex.Message);
            }
        }
        //
        // POST: /SysApplication/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteApplicationList(ids);
            return RedirectToAction("Index");
        }

        public ActionResult Dependent(int id, AppDependentSearchArgs arg, int pageIndex = 1)
        {
            ViewBag.AppId = id;
            #region 查询参数
            DependentSearchRequest request = new DependentSearchRequest()
            {
                SubSystemId = arg.SubSystemId,
                Status = arg.Status,
                AppTypeId = arg.AppTypeId,
                AppName = arg.AppName,
                Bind = arg.Bind,
                AppId = id
            };
            #endregion

            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };

            List<Entity.SysSubEntity> subSystemList = subSystemLogic.GetSubSystemList().ToList<Entity.SysSubEntity>();
            List<Entity.SysAppTypeEntity> appTypeList = appTypeLogic.GetAppTypeList().ToList<Entity.SysAppTypeEntity>();

            appTypeList.Insert(0, new SysAppTypeEntity() { AppTypeId = -1, TypeName = "全部" });
            subSystemList.Insert(0, new SysSubEntity() { SubSystemId = -1, SystemName = "全部" });

            ViewBag.AppTypeId = new SelectList(appTypeList, "AppTypeId", "TypeName");
            ViewBag.SubSystemId = new SelectList(subSystemList, "SubSystemId", "SystemName");
            ViewBag.AppState = new SelectList(EnumHelper.GetItemValueList<EntityStatus>(), "Key", "Value");
            ViewBag.DictionaryAppType = appTypeList.ToDictionary(k => k.AppTypeId, v => v);

            List<SysApplicationEntity> sysApplicationList = logic.GetCanBindApplication(request, pageItem).ToList<SysApplicationEntity>();
            ViewBag.SysApplicationList = new PagedList<SysApplicationEntity>(sysApplicationList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);

            return View(arg);
        }
        public ActionResult BindDependent(int appId, int dependentId, int option)
        {
            try
            {
                SysAppDependentLogic dependentLogic = new SysAppDependentLogic();
                switch (option)
                {
                    case 0:
                        dependentLogic.BindingDependent(appId, dependentId);
                        break;
                    case 1:
                        dependentLogic.DeleteDependent(appId, dependentId);
                        break;
                }
                return RedirectToAction("Dependent", new { id = appId });
            }
            catch (Exception ex)
            {
                return this.Back("绑定依赖关系发生异常。" + ex.Message);
            }
        }
    }
}
