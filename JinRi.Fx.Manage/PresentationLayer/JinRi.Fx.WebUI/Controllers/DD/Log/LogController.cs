using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinRi.Fx.Entity;
using JinRi.Fx.Logic;
using JinRi.Fx.Logic.DD;
using JinRi.Fx.Web;
using JinRi.Fx.WebUI.Models.DD;
using Newtonsoft.Json;

namespace JinRi.Fx.WebUI.Controllers.DD.Log
{
    public class LogController : ControllerBaseAdmin
    {
        private readonly LogLogic _logLogic = new LogLogic();
        [UserAuthentication]
        public ActionResult Log()
        {
            return View();
        }
        [UserAuthentication]
        public ActionResult LogJson()
        {
            int pageIndex = Request["page"] == null ? 1 : Convert.ToInt32(Request["page"]);
            int pageSize = Request["rows"] == null ? 10 : Convert.ToInt32(Request["rows"]);
            string dealer = Request["Operator"];
            string beginTime = Request["BeginTime"];
            string endTime = Request["EndTime"];
            string server = Request["Server"];
            string db = Request["Db"];
            string table = Request["Table"];
            string col = Request["Col"];
            int totalRecords = 0;

            var result = _logLogic.Query(dealer, beginTime, endTime, server, db, table, col, pageIndex, pageSize, out totalRecords).ToLogModels();
            IList<LogModel> list = result == null ? new List<LogModel>() : result.ToList();
            var page = new Pagination<LogModel> {Total = totalRecords, Rows = list};
            return Content(JsonConvert.SerializeObject(page));
        }

        [UserAuthentication]
        public ActionResult UserJson()
        {
            var logic = new SysUserLogic();
            var userList = logic.GetUserList(null, -1, -1).ToList();
            var list = new List<IdNameModel>
            {
                new IdNameModel()
                {
                    Text = "全部",
                    Value = ""
                }
            };
            list.AddRange(userList.Select(i => new IdNameModel()
            {
                Text = i.UserName, Value = i.UserName
            }));
            return Content(JsonConvert.SerializeObject(list));
        }

        [UserAuthentication]
        public ActionResult DbJson(string serverName)
        {
            var list = new List<IdNameModel>
            {
                new IdNameModel()
                {
                    Text = "全部",
                    Value = ""
                }
            };
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
            return Content(JsonConvert.SerializeObject(list));
        }
        [UserAuthentication]
        public ActionResult TableJson(string serverName, string databaseName)
        {
            var list = new List<IdNameModel>
            {
                new IdNameModel()
                {
                    Text = "全部",
                    Value = ""
                }
            };
            if (serverName != "" && databaseName != "")
            {
                var tableService = new TableLogic();
                IList<TableModel> listTableModel = new List<TableModel>();
                var tableModels = tableService.GetAllTable(serverName, databaseName, "dbo").ToTableModels();
                if (tableModels != null)
                {
                    listTableModel = tableModels.ToList();
                }
                list.AddRange(listTableModel.Select(i => new IdNameModel()
                {
                    Text = i.TableName,
                    Value = i.TableName
                }));
            }
            return Content(JsonConvert.SerializeObject(list));
        }
        [UserAuthentication]
        public ActionResult ColJson(string serverName, string databaseName, string tableName)
        {
            var list = new List<IdNameModel>
            {
                new IdNameModel()
                {
                    Text = "全部",
                    Value = ""
                }
            };
            if (serverName != "" && databaseName != "" && tableName != "")
            {
                var fieldLogic = new FieldLogic();
                var listFieldModel = new List<FieldModel>();
                var fieldModels =
                    fieldLogic.GetAllField(serverName, databaseName, "dbo", tableName).ToFieldModels();
                if (fieldModels != null)
                {
                    listFieldModel = fieldModels.ToList();
                }
                list.AddRange(listFieldModel.Select(i => new IdNameModel()
                {
                    Text = i.FieldName,
                    Value = i.FieldName
                }));
            }
            return Content(JsonConvert.SerializeObject(list));
        }
    }
}
