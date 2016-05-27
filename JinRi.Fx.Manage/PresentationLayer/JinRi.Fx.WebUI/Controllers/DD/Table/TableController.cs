using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JinRi.Fx.Data.DD;
using JinRi.Fx.Logic.DD;
using JinRi.Fx.Web;
using JinRi.Fx.WebUI.Models.DD;
using Newtonsoft.Json;

namespace JinRi.Fx.WebUI.Controllers.DD.Table
{
    public class TableController : ControllerBaseAdmin
    {
        readonly TableLogic _tableService = new TableLogic();
        [UserAuthentication]
        public ActionResult Table()
        {
            string serverName = Request["ServerName"];
            ViewBag.ServerName = serverName;
            string databaseName = Request["DatabaseName"];
            ViewBag.DatabaseName = databaseName;
            string schemaName = Request["SchemaName"];
            ViewBag.SchemaName = schemaName;
            return View();
        }
        [UserAuthentication]
        public ActionResult TableJson(string serverName, string databaseName, string schemaName)
        {
            IList<TableModel> list = new List<TableModel>();
            var tableModels = _tableService.GetAllTable(serverName, databaseName, schemaName).ToTableModels();
            if (tableModels != null)
            {
                list = tableModels.ToList();
            }
            var page = new Pagination<TableModel> { Total = list.Count, Rows = list };
            return Content(JsonConvert.SerializeObject(page));
        }

        [HttpPost]
        [UserAuthentication]
        public ActionResult Edit(FormCollection form)
        {
            var tableModel = new TableModel();
            UpdateModel(tableModel, form);
            _tableService.UpdateTableDesc(tableModel.ToTable(), GetLoginUser().UserName);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
