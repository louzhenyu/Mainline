using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JinRi.Fx.Data.DD;
using JinRi.Fx.Logic.DD;
using JinRi.Fx.Web;
using JinRi.Fx.WebUI.Models.DD;
using Newtonsoft.Json;

namespace JinRi.Fx.WebUI.Controllers.DD.Database
{
    public class DatabaseController : ControllerBaseAdmin
    {
        readonly DatabaseLogic _databaseLogic = new DatabaseLogic();
        [UserAuthentication]
        public ActionResult Database()
        {
            string serverName = Request["ServerName"];
            ViewBag.ServerName = serverName;
            return View();
        }
        [UserAuthentication]
        public ActionResult DatabaseJson(string serverName)
        {
            IList<DatabaseModel> list = new List<DatabaseModel>();
            var databaseModels = _databaseLogic.GetAllDatabase(serverName).ToDatabaseModels();
            if (databaseModels != null)
            {
                list = databaseModels.ToList();
            }
            var page = new Pagination<DatabaseModel> { Total = list.Count, Rows = list };
            return Content(JsonConvert.SerializeObject(page));
        }
        [HttpPost]
        [UserAuthentication]
        public ActionResult Edit(FormCollection form)
        {
            var databaseModel = new DatabaseModel();
            UpdateModel(databaseModel, form);
            _databaseLogic.UpdateDbDesc(databaseModel.ToDatabase(),GetLoginUser().UserName);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
