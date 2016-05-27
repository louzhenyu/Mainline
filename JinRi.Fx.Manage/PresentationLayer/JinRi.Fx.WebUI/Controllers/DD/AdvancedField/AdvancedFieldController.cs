using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinRi.Fx.Logic.DD;
using JinRi.Fx.Web;
using JinRi.Fx.WebUI.Models.DD;
using Newtonsoft.Json;

namespace JinRi.Fx.WebUI.Controllers.DD.AdvancedField
{
    public class AdvancedFieldController : ControllerBaseAdmin
    {
        private readonly AdvancedFieldLogic _advancedFieldLogic = new AdvancedFieldLogic();
        private readonly FieldLogic _fieldLogic = new FieldLogic();
        [UserAuthentication]
        public ActionResult AdvancedField()
        {
            string serverName = Request["ServerName"];
            ViewBag.ServerName = serverName;
            return View();
        }
        [UserAuthentication]
        public ActionResult AdvancedFieldJson()
        {
            int pageIndex = Request["page"] == null ? 1 : Convert.ToInt32(Request["page"]);
            int pageSize = Request["rows"] == null ? 10 : Convert.ToInt32(Request["rows"]);
            string server = Request["Server"];
            string db = Request["Db"];
            string table = Request["Table"];
            string col = Request["Col"];
            string colDesc = Request["ColDesc"];
            int totalRecords = 0;

            var result =
                _advancedFieldLogic.GetField(server, db, table, col, colDesc, pageIndex, pageSize, out totalRecords)
                    .ToFieldModels();
            IList<FieldModel> list = result == null ? new List<FieldModel>() : result.ToList();
            var page = new Pagination<FieldModel> { Total = totalRecords, Rows = list };
            return Content(JsonConvert.SerializeObject(page));
        }
        [HttpPost]
        [UserAuthentication]
        public ActionResult Edit(FormCollection form)
        {
            var fieldModel = new FieldModel();
            UpdateModel(fieldModel, form);
            fieldModel.SchemaName = "dbo";
            _fieldLogic.UpdateFieldDesc(fieldModel.ToField(), GetLoginUser().UserName);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        [UserAuthentication]
        public ActionResult DbJson(string serverName)
        {
            var list = new List<IdNameModel>();
            if (serverName != "")
            {
                var databaseLogic = new DatabaseLogic();
                var listDatabaseModel = new List<DatabaseModel>();
                var databaseModels = databaseLogic.GetAllDatabase(serverName).ToDatabaseModels();
                if (databaseModels != null)
                {
                    listDatabaseModel = databaseModels.ToList();
                }
                list.AddRange(listDatabaseModel.Select(i => new IdNameModel()
                {
                    Text = i.DatabaseName,
                    Value = i.DatabaseName
                }));
            }
            list.Add(new IdNameModel()
            {
                Text = "全部",
                Value = ""
            });
            return Content(JsonConvert.SerializeObject(list));
        }
    }
}
