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

namespace JinRi.Fx.WebUI.Controllers.SysProduct
{
    [UserAuthentication]
    public class SysProductController : ControllerBaseAdmin
    {
        SysProductLogic logic = new SysProductLogic();
        //
        // GET: /SysProduct/
        public ActionResult Index(string productName = "", int pageIndex = 1)
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };
            List<SysProductEntity> roleList = logic.GetProductList(productName, pageItem).ToList<SysProductEntity>();
            var model = new PagedList<SysProductEntity>(roleList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View(model);
        }

        //
        // GET: /SysProduct/Create
        public ActionResult Create()
        {
            var modelState = EnumHelper.GetItemValueList<EntityStatus>();
            ViewBag.ModelState = new SelectList(modelState, "Key", "Value");
            return View();
        }
        //
        // POST: /SysProduct/Create
        [HttpPost]
        public ActionResult Create(SysProductEntity model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.ProductId = Convert.ToInt32(Request.Form["ProductId"]);
                    if (logic.GetProductInfo(model.ProductId) == null)
                    {
                        model.ProductName = Request.Form["ProductName"];
                        model.ProductEName = Request.Form["ProductEName"];
                        model.Remark = Request.Form["Remark"];
                        logic.AddProduct(model);
                        return this.RefreshParent();
                    }
                    else
                    {
                        return this.Back("重复的产品线编号：" + model.ProductId);
                    }
                }
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                return this.Back("新增产品线该模块发生异常。" + ex.Message);
            }
        }

        //
        // GET: /SysProduct/Edit/5
        public ActionResult Edit(int id)
        {
            var modelState = EnumHelper.GetItemValueList<EntityStatus>();
            ViewBag.ModelState = new SelectList(modelState, "Key", "Value");
            return View(logic.GetProductInfo(id));
        }

        //
        // POST: /SysProduct/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    JinRi.Fx.Entity.SysProductEntity model = new JinRi.Fx.Entity.SysProductEntity();
                    model.ProductId = id;
                    model.ProductName = collection["ProductName"];
                    model.ProductEName = collection["ProductEName"];
                    model.Remark = collection["Remark"];
                    logic.UpdateProduct(model);
                    return this.RefreshParent();
                }
                return RedirectToAction("Edit", new { id = id });
            }
            catch (Exception ex)
            {
                return this.Back("修改产品线该模块发生异常。" + ex.Message);
            }
        }
        //
        // POST: /SysProduct/Delete/5
        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
            logic.DeleteProductList(ids);
            return RedirectToAction("Index");
        }
    }
}
