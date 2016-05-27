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

namespace JinRi.Fx.WebUI.Controllers.Application
{
    public class SysSubController : ControllerBaseAdmin
    {
        SysSubLogic logic = new SysSubLogic();
        //
        // GET: /SysSub/

        public ActionResult Index(string productName = "", int productId = -1, int pageIndex = 1)
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };

            List<SysProductEntity> productList = new SysProductLogic().GetProductList().ToList<SysProductEntity>();
            ViewBag.DictionaryProduct = productList.ToDictionary(k => k.ProductId, v => v);

            productList.Insert(0, new SysProductEntity { ProductId = -1, ProductName = "全部" });
            ViewBag.ProductId = new SelectList(productList, "ProductId", "ProductName");

            List<SysSubEntity> subSystemList = logic.GetSubSystemList(productId, productName, pageItem).ToList<SysSubEntity>();
            ViewBag.SubSystemList = new PagedList<SysSubEntity>(subSystemList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View();
        }

        //
        // GET: /SysSub/Create

        public ActionResult Create()
        {
            SysProductLogic productLogic = new SysProductLogic();
            var productList = productLogic.GetProductList().ToList<SysProductEntity>();
            this.ViewBag.ProductId = new SelectList(productList, "ProductId", "ProductName");

            return View();
        }

        //
        // POST: /SysSub/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Fx.Entity.SysSubEntity model = new Fx.Entity.SysSubEntity();
                model.SubSystemId = Convert.ToInt32(collection["SubSystemId"]);
                if (logic.GetSubSystemInfo(model.SubSystemId) == null)
                {
                    model.SystemName = collection["SystemName"];
                    model.SystemEName = collection["SystemEName"];
                    model.Remark = collection["Remark"];
                    model.ProductId = Convert.ToInt32(collection["ProductId"]);
                    logic.AddSubSystem(model);
                }
                else
                {
                    return this.Back("重复的子系统编号：" + model.SubSystemId);
                }
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                return this.Back("新增子系统发生异常。" + ex.Message);
            }
        }

        //
        // GET: /SysSub/Edit/5

        public ActionResult Edit(int id)
        {
            var model = logic.GetSubSystemInfo(id);
            SysProductLogic productLogic = new SysProductLogic();
            var productList = productLogic.GetProductList().ToList<SysProductEntity>();
            this.ViewBag.ProductId = new SelectList(productList, "ProductId", "ProductName", model.ProductId);
            return View(model);
        }

        //
        // POST: /SysSub/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                Fx.Entity.SysSubEntity model = new Fx.Entity.SysSubEntity();
                model.SubSystemId = id;
                model.SystemName = collection["SystemName"];
                model.SystemEName = collection["SystemEName"];
                model.Remark = collection["Remark"];
                model.ProductId = Convert.ToInt32(collection["ProductId"]);
                logic.UpdateSubSystem(model);
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                return this.Back("修改子系统发生异常。" + ex.Message);
            }
        }
        //
        // POST: /SysSub/Delete/5

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteSubSystemList(ids);
            return RedirectToAction("Index");
        }
    }
}
