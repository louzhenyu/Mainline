using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB;
using System.Configuration;
using log4net;
using MongoDB.Bson;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;
using JinRi.Fx.Logic;

namespace LogView
{
    public partial class LogSearch : System.Web.UI.Page
    {
        ILog log = LogManager.GetLogger(typeof(LogSearch));
        private string PageNum = "1";
        private int PageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"].ToString());

        Function myFun = new Function();
        private string QueryStr = "LogSearch.aspx?";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                txt_ApplyStartTime.Text = DateTime.Now.AddHours(-1).ToString();
                txt_ApplyEndTime.Text = DateTime.Now.ToString();
                tbInfo.Visible = false;
                if ((string)Session["Search"] == "SeniorSearch")
                {
                    BoxVisible(true);
                    btnSeniorSearch.Visible = false;
                }
                SelectNextPageValue();
                
                BindData();
               
            }
        }

        /// <summary>
        /// 取下一页值
        /// </summary>
        private void SelectNextPageValue()
        {
            #region 取下一页值
            if (Request.QueryString["page"] != null)
            {
                PageNum = Request.QueryString["page"].ToString().Trim();
                if (!myFun.IsNum(Request.QueryString["page"].ToString()))
                {
                    PageNum = "1";
                }
                else
                {
                    if (int.Parse(Request.QueryString["page"].ToString()) < 1)
                    {
                        PageNum = "1";
                    }
                }
            }
            else
            {
                PageNum = "1";
            }


            if (Request.QueryString["lev"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["lev"].ToString().Trim()) && Request.QueryString["lev"].ToString().Trim() != "0")
                {
                    this.dplistLevl.SelectedValue = Request.QueryString["lev"].ToString().Trim();
                }
                QueryStr += string.Format("lev={0}&", Request.QueryString["lev"].ToString().Trim());
            }
            else
            {
                QueryStr += string.Format("lev={0}&", "");
            }


            if (Request.QueryString["appid"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["appid"].ToString().Trim()))
                {
                    this.txtAppid.Text = Request.QueryString["appid"].ToString().Trim();
                }
                QueryStr += string.Format("appid={0}&", Request.QueryString["appid"].ToString().Trim());
            }
            else
            {
                QueryStr += string.Format("appid={0}&", "");
            }

            if (Request.QueryString["ip"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ip"].ToString().Trim()))
                {
                    this.txtIP.Text = Request.QueryString["ip"].ToString().Trim();
                }
                QueryStr += string.Format("ip={0}&", Request.QueryString["ip"].ToString().Trim());
            }
            else
            {
                QueryStr += string.Format("ip={0}&", "");
            }

            if (Request.QueryString["message"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["message"].ToString().Trim()))
                {
                    this.txtMessage.Text = Request.QueryString["message"].ToString().Trim();
                }
                QueryStr += string.Format("message={0}&", Request.QueryString["message"].ToString().Trim());
            }
            else
            {
                QueryStr += string.Format("message={0}&", "");
            }
            if (Request.QueryString["classname"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["classname"].ToString().Trim()))
                {
                    this.txtClassName.Text = Request.QueryString["classname"].ToString().Trim();
                }
                QueryStr += string.Format("classname={0}&", Request.QueryString["classname"].ToString().Trim());
            }
            else
            {
                QueryStr += string.Format("classname={0}&", "");
            }
            if (Request.QueryString["method"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["method"].ToString().Trim()))
                {
                    this.txtMethod.Text = Request.QueryString["method"].ToString().Trim();
                }
                QueryStr += string.Format("method={0}&", Request.QueryString["method"].ToString().Trim());
            }
            else
            {
                QueryStr += string.Format("method={0}&", "");
            }

            if (Request.QueryString["begtime"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["begtime"].ToString().Trim()))
                {
                    this.txt_ApplyStartTime.Text = Request.QueryString["begtime"].ToString().Trim();
                }
                QueryStr += string.Format("begtime={0}&", Request.QueryString["begtime"].ToString().Trim());
            }
            else
            {
                QueryStr += string.Format("begtime={0}&", "");
            }

            if (Request.QueryString["endtime"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["endtime"].ToString().Trim()))
                {
                    this.txt_ApplyEndTime.Text = Request.QueryString["endtime"].ToString().Trim();
                }
                QueryStr += string.Format("endtime={0}&", Request.QueryString["endtime"].ToString().Trim());
            }
            else
            {
                QueryStr += string.Format("endtime={0}&", "");
            }
            if (Request.QueryString["traceid"] != null)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["traceid"].ToString().Trim()))
                {
                    this.txtTraceid.Text = Request.QueryString["traceid"].ToString().Trim();
                }
                QueryStr += string.Format("traceid={0}&", Request.QueryString["traceid"].ToString().Trim());
            }
            else
            {
                QueryStr += string.Format("traceid={0}&", "");
            }
            #endregion
 
        }

        ///<summary>
        /// 按日期分页显示日志
        ///</summary>
        ///<param name="date"></param>
        ///<param name="pageSize"></param>
        ///<param name="pageNum"></param>
        ///<returns></returns>
        public static ResultSet ShowLogs(string message, int pageSize, int pageNum, string appid, string ip, DateTime begtime, DateTime endtime, string lev,string classname,string method)
        {
            long resultCount = 0;

            List<Log4NetEntiy> docResultSet = new List<Log4NetEntiy>();

            Mongo mongoDBLog = null;

            try
            {
                mongoDBLog = new Mongo(ConfigurationManager.AppSettings["mongoDBConfig"]);
                mongoDBLog.Connect();


                var dbLog = mongoDBLog.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"].ToString());

                var collection = dbLog.GetCollection<Log4NetEntiy>(ConfigurationManager.AppSettings["mongoDBCollection"].ToString());

                // resultCount = collection.Count();

                if (!string.IsNullOrEmpty(appid.Trim()))
                {
                    #region appid is not null
                    switch (lev)
                    {
                        case "ALL":
                            var queryResultSet = from p in (collection.Linq().Skip(pageSize * (pageNum - 1)).Take(pageSize))
                                                 where p.timestamp >= begtime && p.timestamp <= endtime
                                                 && p.message.Contains(message) && p.ip.Contains(ip) && p.appid == appid.Trim() && p.method.Contains(method.Trim())&&p.className.Contains(classname)
                                                 orderby p.timestamp descending
                                                 select new
                                                 {
                                                     p._id,
                                                     p.appid,
                                                     p.ip,
                                                     p.level,
                                                     p.method,
                                                     p.timestamp
                                                 };
                          
                            var list = queryResultSet.ToList();

                            foreach (var item in list)
                            {
                                Log4NetEntiy ent = new Log4NetEntiy();
                                ent._id = item._id;
                                ent.appid = item.appid;
                                ent.ip = item.ip;
                                ent.level = item.level;
                                ent.timestamp = item.timestamp;
                                ent.method = item.method;
                                docResultSet.Add(ent);
                            }
                            break;
                        default:
                            queryResultSet = from p in (collection.Linq().Skip(pageSize * (pageNum - 1)).Take(pageSize))
                                             where p.timestamp >= begtime && p.timestamp <= endtime && p.level == lev.Trim()
                                             && p.message.Contains(message) && p.ip.Contains(ip) && p.appid == appid.Trim() && p.method.Contains(method.Trim()) && p.className.Contains(classname)
                                             orderby p.timestamp descending
                                             select new
                                             {
                                                 p._id,
                                                 p.appid,
                                                 p.ip,
                                                 p.level,
                                                 p.method,
                                                 p.timestamp
                                             };
                         
                            list = queryResultSet.ToList();
                         

                            foreach (var item in list)
                            {
                                Log4NetEntiy ent = new Log4NetEntiy();
                                ent._id = item._id;
                                ent.appid = item.appid;
                                ent.ip = item.ip;
                                ent.level = item.level;
                                ent.timestamp = item.timestamp;
                                ent.method = item.method;
                                docResultSet.Add(ent);
                            }
                            break;
                    }

                    #endregion


                }
                else
                {
                    #region appid is null
                    switch (lev)
                    {
                        case "ALL":
                            var queryResultSet = from p in (collection.Linq().Skip(pageSize * (pageNum - 1)).Take(pageSize))
                                                 where p.timestamp >= begtime && p.timestamp <= endtime
                                                 && p.message.Contains(message) && p.ip.Contains(ip) && p.method.Contains(method.Trim()) && p.className.Contains(classname)
                                                 orderby p.timestamp descending
                                                 select new
                                                 {
                                                     p._id,
                                                     p.appid,
                                                     p.ip,
                                                     p.level,
                                                     p.method,
                                                     p.timestamp
                                                 };
                            var list = queryResultSet.ToList();

                            foreach (var item in list)
                            {
                                Log4NetEntiy ent = new Log4NetEntiy();
                                ent._id = item._id;
                                ent.appid = item.appid;
                                ent.ip = item.ip;
                                ent.level = item.level;
                                ent.timestamp = item.timestamp;
                                ent.method = item.method;
                                docResultSet.Add(ent);
                            }
                            break;
                        default:
                            queryResultSet = from p in (collection.Linq().Skip(pageSize * (pageNum - 1)).Take(pageSize))
                                             where p.timestamp >= begtime && p.timestamp <= endtime && p.level == lev.Trim()
                                               && p.message.Contains(message) && p.ip.Contains(ip) && p.method.Contains(method.Trim()) && p.className.Contains(classname)
                                             orderby p.timestamp descending
                                             select new
                                             {
                                                 p._id,
                                                 p.appid,
                                                 p.ip,
                                                 p.level,
                                                 p.method,
                                                 p.timestamp
                                             };
                            list = queryResultSet.ToList();

                            foreach (var item in list)
                            {
                                Log4NetEntiy ent = new Log4NetEntiy();
                                ent._id = item._id;
                                ent.appid = item.appid;
                                ent.ip = item.ip;
                                ent.level = item.level;
                                ent.timestamp = item.timestamp;
                                ent.method = item.method;
                                docResultSet.Add(ent);
                            }
                            break;
                    }
                    #endregion
                }
            }
            catch
            {
                //do something

            }
            finally
            {
                if (mongoDBLog != null)
                {
                    mongoDBLog.Disconnect();
                    mongoDBLog.Dispose();
                }
            }


            return new ResultSet { resultCount = resultCount, result = docResultSet };
        }

        public static ResultSet ShowLogs(string message, int pageSize, int pageNum, string appid, string ip, DateTime begtime, DateTime endtime, string lev, string classname, string method,string traceid)
        {
            long resultCount = 0;

            List<Log4NetEntiy> docResultSet = new List<Log4NetEntiy>();

            Mongo mongoDBLog = null;

            try
            {
                mongoDBLog = new Mongo(ConfigurationManager.AppSettings["mongoDBConfig"]);
                mongoDBLog.Connect();


                var dbLog = mongoDBLog.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"].ToString());

                var collection = dbLog.GetCollection<Log4NetEntiy>(ConfigurationManager.AppSettings["mongoDBCollection"].ToString());

                // resultCount = collection.Count();

                if (!string.IsNullOrEmpty(appid.Trim()))
                {
                    #region appid is not null
                    switch (lev)
                    {
                        case "ALL":
                            var queryResultSet = from p in (collection.Linq().Skip(pageSize * (pageNum - 1)).Take(pageSize))
                                                 where p.timestamp >= begtime && p.timestamp <= endtime
                                                 && p.message.Contains(message) && p.ip.Contains(ip) && p.appid == appid.Trim() && p.method.Contains(method.Trim()) && p.className.Contains(classname)&&p.traceId.Contains(traceid)
                                                 orderby p.timestamp descending
                                                 select new
                                                 {
                                                     p._id,
                                                     p.appid,
                                                     p.ip,
                                                     p.level,
                                                     p.method,
                                                     p.timestamp
                                                 };

                            var list = queryResultSet.ToList();

                            foreach (var item in list)
                            {
                                Log4NetEntiy ent = new Log4NetEntiy();
                                ent._id = item._id;
                                ent.appid = item.appid;
                                ent.ip = item.ip;
                                ent.level = item.level;
                                ent.timestamp = item.timestamp;
                                ent.method = item.method;
                                docResultSet.Add(ent);
                            }
                            break;
                        default:
                            queryResultSet = from p in (collection.Linq().Skip(pageSize * (pageNum - 1)).Take(pageSize))
                                             where p.timestamp >= begtime && p.timestamp <= endtime && p.level == lev.Trim()
                                             && p.message.Contains(message) && p.ip.Contains(ip) && p.appid == appid.Trim() && p.method.Contains(method.Trim()) && p.className.Contains(classname) && p.traceId.Contains(traceid)
                                             orderby p.timestamp descending
                                             select new
                                             {
                                                 p._id,
                                                 p.appid,
                                                 p.ip,
                                                 p.level,
                                                 p.method,
                                                 p.timestamp
                                             };

                            list = queryResultSet.ToList();


                            foreach (var item in list)
                            {
                                Log4NetEntiy ent = new Log4NetEntiy();
                                ent._id = item._id;
                                ent.appid = item.appid;
                                ent.ip = item.ip;
                                ent.level = item.level;
                                ent.timestamp = item.timestamp;
                                ent.method = item.method;
                                docResultSet.Add(ent);
                            }
                            break;
                    }

                    #endregion


                }
                else
                {
                    #region appid is null
                    switch (lev)
                    {
                        case "ALL":
                            var queryResultSet = from p in (collection.Linq().Skip(pageSize * (pageNum - 1)).Take(pageSize))
                                                 where p.timestamp >= begtime && p.timestamp <= endtime
                                                 && p.message.Contains(message) && p.ip.Contains(ip) && p.method.Contains(method.Trim()) && p.className.Contains(classname) && p.traceId.Contains(traceid)
                                                 orderby p.timestamp descending
                                                 select new
                                                 {
                                                     p._id,
                                                     p.appid,
                                                     p.ip,
                                                     p.level,
                                                     p.method,
                                                     p.timestamp
                                                 };
                            var list = queryResultSet.ToList();

                            foreach (var item in list)
                            {
                                Log4NetEntiy ent = new Log4NetEntiy();
                                ent._id = item._id;
                                ent.appid = item.appid;
                                ent.ip = item.ip;
                                ent.level = item.level;
                                ent.timestamp = item.timestamp;
                                ent.method = item.method;
                                docResultSet.Add(ent);
                            }
                            break;
                        default:
                            queryResultSet = from p in (collection.Linq().Skip(pageSize * (pageNum - 1)).Take(pageSize))
                                             where p.timestamp >= begtime && p.timestamp <= endtime && p.level == lev.Trim()
                                               && p.message.Contains(message) && p.ip.Contains(ip) && p.method.Contains(method.Trim()) && p.className.Contains(classname) && p.traceId.Contains(traceid)
                                             orderby p.timestamp descending
                                             select new
                                             {
                                                 p._id,
                                                 p.appid,
                                                 p.ip,
                                                 p.level,
                                                 p.method,
                                                 p.timestamp
                                             };
                            list = queryResultSet.ToList();

                            foreach (var item in list)
                            {
                                Log4NetEntiy ent = new Log4NetEntiy();
                                ent._id = item._id;
                                ent.appid = item.appid;
                                ent.ip = item.ip;
                                ent.level = item.level;
                                ent.timestamp = item.timestamp;
                                ent.method = item.method;
                                docResultSet.Add(ent);
                            }
                            break;
                    }
                    #endregion
                }
            }
            catch
            {
                //do something

            }
            finally
            {
                if (mongoDBLog != null)
                {
                    mongoDBLog.Disconnect();
                    mongoDBLog.Dispose();
                }
            }


            return new ResultSet { resultCount = resultCount, result = docResultSet };
        }

        private void BindData()
        {

            try
            {
                DateTime dt1 = DateTime.Now;
                string level = string.Empty;
                string appid = string.Empty;
                string strWhere = string.Empty;
                level = dplistLevl.SelectedValue.ToUpper();
                if(!string.IsNullOrEmpty(txtAppid.Text.Trim()))
                {
                appid=txtAppid.Text.Trim().Split(' ')[0];
                }
                ResultSet RS = new ResultSet();
                if (string.IsNullOrEmpty(txtTraceid.Text.Trim()))
                {
                    RS = ShowLogs(txtMessage.Text.Trim(), PageSize, int.Parse(PageNum), appid.Trim(), txtIP.Text.Trim(), DateTime.Parse(txt_ApplyStartTime.Text.Trim()), DateTime.Parse(txt_ApplyEndTime.Text.Trim()), level, txtClassName.Text.Trim(), txtMethod.Text.Trim());
                }
                else
                {
                    RS = ShowLogs(txtMessage.Text.Trim(), PageSize, int.Parse(PageNum), appid.Trim(), txtIP.Text.Trim(), DateTime.Parse(txt_ApplyStartTime.Text.Trim()), DateTime.Parse(txt_ApplyEndTime.Text.Trim()), level, txtClassName.Text.Trim(), txtMethod.Text.Trim(),txtTraceid.Text.Trim());

                }

                strWhere = SetSQLStr();
                rptUserInfoList.DataSource = RS.result;
                rptUserInfoList.DataBind();
                tbInfo.Visible = false;
                Lb_PageShow.Text = myFun.GetPages(long.Parse(PageNum), PageSize, QueryStr, RS.result.Count());
                DateTime dt2 = DateTime.Now;
                TimeSpan t = dt2 - dt1;
                lblTimeSpan.Text = "耗时：" + t.TotalSeconds.ToString() + "s";
              
            }
            catch (Exception ex)
            {

                log.Error(ex.ToString());
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
           
            BindData();
        }

        //设置查询条件和翻页的URL
        protected string SetSQLStr()
        {
            StringBuilder sb = new StringBuilder();
            string ReturnStr = string.Empty;//查询 语句
            QueryStr = string.Empty;
            QueryStr += "LogSearch.aspx?";

            if (this.dplistLevl.SelectedValue.ToString().Trim() != "" && this.dplistLevl.SelectedValue.ToString().Trim() != "All")
            {
                QueryStr += string.Format("lev={0}&", this.dplistLevl.SelectedValue.Trim());
            }

            if (!string.IsNullOrEmpty(this.txtAppid.Text.Trim()))
            {
                QueryStr += string.Format("appid={0}&", this.txtAppid.Text.Trim());
            }
            else
            {
                QueryStr += string.Format("appid={0}&", "");
            }

            if (!string.IsNullOrEmpty(this.txtMessage.Text.Trim()))
            {
              
                QueryStr += string.Format("message={0}&", this.txtMessage.Text.Trim());
            }
            else
            {
                QueryStr += string.Format("message={0}&", "");
            }

            if (!string.IsNullOrEmpty(this.txtIP.Text.Trim()))
            {
                QueryStr += string.Format("ip={0}&", this.txtIP.Text.Trim());
            }
            else
            {
                QueryStr += string.Format("ip={0}&", "");
            }

            if (!string.IsNullOrEmpty(this.txt_ApplyStartTime.Text.Trim()))
            {
                QueryStr += string.Format("begtime={0}&", this.txt_ApplyStartTime.Text.Trim());
            }
            else
            {
                QueryStr += string.Format("begtime={0}&", "");
            }

            if (!string.IsNullOrEmpty(this.txt_ApplyEndTime.Text.Trim()))
            {
                QueryStr += string.Format("endtime={0}&", this.txt_ApplyEndTime.Text.Trim());
            }
            else
            {
                QueryStr += string.Format("endtime={0}&", "");
            }

            if (!string.IsNullOrEmpty(this.txtClassName.Text.Trim()))
            {
                QueryStr += string.Format("classname={0}&", this.txtClassName.Text.Trim());
            }
            else
            {
                QueryStr += string.Format("classname={0}&", "");
            }
            if (!string.IsNullOrEmpty(this.txtMethod.Text.Trim()))
            {
                QueryStr += string.Format("method={0}&", this.txtMethod.Text.Trim());
            }
            else
            {
                QueryStr += string.Format("method={0}&", "");
            }
            if (!string.IsNullOrEmpty(this.txtTraceid.Text.Trim()))
            {
                QueryStr += string.Format("traceid={0}&", this.txtTraceid.Text.Trim());
            }
            else
            {
                QueryStr += string.Format("traceid={0}&", "");
            }
            return (sb.ToString());
        }
        protected void btnInfo_Click(object sender, EventArgs e)
        {
            string logid = ((LinkButton)sender).CommandArgument.ToString();
            LoadData(logid);

            foreach (RepeaterItem item in rptUserInfoList.Items)
            {
                HtmlTableRow trlogid = (HtmlTableRow)item.FindControl("TrID");

                Label lblid = (Label)item.FindControl("lblID");
                if (lblid != null&&trlogid!=null)
                {
                    if (trlogid.Attributes["title"] == logid)
                    {
                        trlogid.BgColor = "#bdddee";
                    }
                    else
                    {
                        trlogid.BgColor = "#ffffff";
                    }
                }
            }
        }


        //加载数据
        private void LoadData(string logid)
        {

            try
            {
                if (!string.IsNullOrEmpty(logid))
                {

                    BsonDocument log4info = new BsonDocument();
                    BsonElement emt =null;
                    log4info = LogSelectOne(logid);
                    if (log4info.ElementCount > 0)
                    {
                        tbInfo.Visible = true;

                        lblMessage.Text =log4info["message"].AsString;
                        lblTime.Text = log4info["timestamp"].AsDateTime.AddHours(8).ToString();
                        lblAppid.Text = log4info["appid"].AsString;
                        this.lblCassName.Text = log4info["className"].AsString;
                        lblLineNumber.Text = log4info["lineNumber"].AsString;
                        lblMethod.Text = log4info["method"].AsString;
                        lblIP.Text = log4info["ip"].AsString;
                        lblLevel.Text = log4info["level"].AsString;
                        
                        if (log4info.GetElement("fileName") != null)
                        {
                            lblFileName.Text = log4info["fileName"].AsString;
                        }
                        if (log4info.TryGetElement("traceId", out emt))
                        {

                            lblTraceID.Text = log4info["traceId"].AsString;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }

        }

        /// <summary>
        /// 根据ObjectID 查询
        /// </summary>
        public BsonDocument LogSelectOne(string objId)
        {
            BsonDocument docFind = new BsonDocument();
            MongoDB.Driver.MongoServer server = MongoDB.Driver.MongoServer.Create(ConfigurationManager.AppSettings["mongoDBConfig"]);
            server.Connect();
            //获取指定数据库
            MongoDB.Driver.MongoDatabase db = server.GetDatabase(ConfigurationManager.AppSettings["mongoDBName"].ToString());
            //获取表
            MongoDB.Driver.MongoCollection<BsonDocument> col = db.GetCollection<BsonDocument>(ConfigurationManager.AppSettings["mongoDBCollection"].ToString());

            try
            {
                var query = new MongoDB.Driver.QueryDocument("_id", new ObjectId(objId));
                docFind = col.FindOne(query);

            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
            finally
            {
                server.Disconnect();

            }
            return docFind;
        }

        [System.Web.Services.WebMethodAttribute(),
    System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetAppidList(string prefixText, int count,
       string contextKey)
        {
            SysApplicationLogic sysappliction = new SysApplicationLogic();
            return sysappliction.GetAppidList(prefixText, "http://fx.jinri.org.cn/api/app");
        }

        private void BoxVisible(bool setValue)
        {
            trIP.Visible = setValue;
            trMethod.Visible = setValue;
            trTraceid.Visible = setValue;
            trClassName.Visible = setValue;
            btnSenior.Visible = setValue;
            btnSeniorSearch.Visible = setValue;


        }
        protected void btnSeniorSearch_Click(object sender, EventArgs e)
        {
            BoxVisible(true);
            Session["Search"] = "SeniorSearch";

            btnSeniorSearch.Visible = false;
        }

        protected void btnSenior_Click(object sender, EventArgs e)
        {
            BoxVisible(false);
            Session["Search"] = "Senior";
            btnSeniorSearch.Visible = true;
            btnSenior.Visible = false;
            txtIP.Text = "";
            txtMethod.Text = "";
            txtClassName.Text = "";
            Session["Search"] = "";
        }
    }
}