using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using JinRi.Fx.Data.DD;
using JinRi.Fx.Logic.DD;
using JinRi.Fx.Web;
using JinRi.Fx.WebUI.Models.DD;
using Newtonsoft.Json;

namespace JinRi.Fx.WebUI.Controllers.DD.Field
{
    public class FieldController : ControllerBaseAdmin
    {
        readonly FieldLogic _fieldLogic = new FieldLogic();
        [UserAuthentication]
        public ActionResult Field()
        {
            string serverName = Request["ServerName"];
            ViewBag.ServerName = serverName;
            string databaseName = Request["DatabaseName"];
            ViewBag.DatabaseName = databaseName;
            string schemaName = Request["SchemaName"];
            ViewBag.SchemaName = schemaName;
            string tableName = Request["TableName"];
            ViewBag.TableName = tableName;
            return View();
        }
        [UserAuthentication]
        public ActionResult FieldJson(string serverName, string databaseName, string schemaName, string tableName)
        {
            IList<FieldModel> list = new List<FieldModel>();
            var fieldModels =
                _fieldLogic.GetAllField(serverName, databaseName, schemaName, tableName).ToFieldModels();
            if (fieldModels != null)
            {
                list = fieldModels.ToList();
            }
            var page = new Pagination<FieldModel> { Total = list.Count, Rows = list };
            return Content(JsonConvert.SerializeObject(page));
        }

        [HttpPost]
        [UserAuthentication]
        public ActionResult Edit(FormCollection form)
        {
            var fieldModel = new FieldModel();
            UpdateModel(fieldModel, form);
            _fieldLogic.UpdateFieldDesc(fieldModel.ToField(), GetLoginUser().UserName);
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
