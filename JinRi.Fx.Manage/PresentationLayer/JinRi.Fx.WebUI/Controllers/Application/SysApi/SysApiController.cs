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

namespace JinRi.Fx.WebUI.Controllers.Application
{
    public class SysApiController : ControllerBaseAdmin
    {
        SysApiLogic logic = new SysApiLogic();
        //
        // GET: /SysApi/
        public ActionResult Index(SysApiSearchArgs arg, int pageIndex = 1)
        {
            int appId = arg.AppId.ConvertTo<int>(-1);
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };
            SysApplicationLogic appLogic = new SysApplicationLogic();
            List<SysApplicationEntity> listApp = appLogic.GetSysApplicationList().ToList<SysApplicationEntity>();
            ViewBag.DictionaryApplication = listApp.ToDictionary(k => k.AppId, v => v);
            List<SysApiEntity> sysApiList = logic.GetSysApiList(appId, arg.Status, arg.ApiName, pageItem).ToList<SysApiEntity>();
            ViewBag.SysApiList = new PagedList<SysApiEntity>(sysApiList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View(arg);
        }

        //
        // GET: /SysApi/Create

        public ActionResult Create()
        {
            SetDropDownList();
            return View();
        }

        void SetDropDownList()
        {
            List<dynamic> list = new List<dynamic> { new { Name = "内部接口", Value = 0 }, new { Name = "OpenApi", Value = 1 }, new { Name = "JSOA 2.0", Value = 2 } };
            this.ViewBag.ApiType = new SelectList(list, "Value", "Name");

            var status = EnumHelper.GetItemValueList<EntityStatus>();
            this.ViewBag.ApiStatus = new SelectList(status, "Key", "Value");
        }
        //
        // POST: /SysApi/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Fx.Entity.SysApiEntity model = new Fx.Entity.SysApiEntity();
                model.AppId = collection["AppId"].ConvertTo<int>();
                model.ApiName = collection["ApiName"];
                model.ApiType = collection["ApiType"].ConvertTo<int>();
                model.ApiDescription = collection["ApiDescription"];
                model.ApiOwner = collection["ApiOwner"];
                model.ApiAddress = collection["ApiAddress"];
                model.ApiStatus = Convert.ToInt32(collection["ApiStatus"]);
                model.Remark = collection["Remark"];
                logic.AddSysApi(model);
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                return this.Back("新增接口注册发生异常。" + ex.Message);
            }
        }

        //
        // GET: /SysApi/Edit/5

        public ActionResult Edit(int id)
        {
            var model = logic.GetSysApiInfo(id);
            SetDropDownList();
            return View(model);
        }

        //
        // POST: /SysApi/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Fx.Entity.SysApiEntity model = new Fx.Entity.SysApiEntity();
                model.SysApiId = id;
                model.AppId = collection["AppId"].ConvertTo<int>();
                model.ApiName = collection["ApiName"];
                model.ApiType = collection["ApiType"].ConvertTo<int>();
                model.ApiDescription = collection["ApiDescription"];
                model.ApiOwner = collection["ApiOwner"];
                model.ApiAddress = collection["ApiAddress"];
                model.ApiStatus = Convert.ToInt32(collection["ApiStatus"]);
                model.Remark = collection["Remark"];
                logic.UpdateSysApi(model);
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                return this.Back("修改接口注册发生异常。" + ex.Message);
            }
        }
        //
        // POST: /SysApi/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteSysApiList(ids);
            return RedirectToAction("Index");
        }
    }
}
