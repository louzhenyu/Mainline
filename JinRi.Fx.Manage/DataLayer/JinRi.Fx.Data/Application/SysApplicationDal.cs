using JinRi.Fx.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using JinRi.Fx.Utility;
using JinRi.Fx.RequestDTO;

namespace JinRi.Fx.Data
{
    /// <summary>
    /// 应用数据访问类
    /// </summary>
    public class SysApplicationDal
    {
        /// <summary>
        /// 获取应用列表
        /// </summary>    
        /// <param name="productIdList">1表示国内机票；2表示国际机票；3表示酒店；4表示公共服务；5表示框架；6表示手机</param>
        /// <returns></returns>
        public IEnumerable<SysApplicationEntity> GetSysApplicationList(int appId, int subSystemId, string appName, int appTypeId, int status, PageItem pageItem = null, List<int> productIdList = null, string owner = "")
        {
            StringBuilder sql = new StringBuilder();

            if (productIdList != null && productIdList.Count > 0)
            {
                string sqlStr = @"SELECT CASE 
         WHEN (SubSystemId >= 1000 AND SubSystemId <= 1099) THEN 1
         WHEN (SubSystemId >= 1100 AND SubSystemId <= 1199) THEN 2
         WHEN (SubSystemId >= 1200 AND SubSystemId <= 1299) THEN 3
         WHEN (SubSystemId >= 1300 AND SubSystemId <= 1399) THEN 4
         WHEN (SubSystemId >= 1400 AND SubSystemId <= 1499) THEN 5
         WHEN (SubSystemId >= 1500 AND SubSystemId <= 1599) THEN 6
      END AS ModuleId,AppId,SubSystemId,AppName,(SELECT TOP 1 MenuId FROM dbo.SysMenu WHERE MenuName = '" + AppSettingsHelper.ConfigCenterMenuName  + "') AS MenuId,CAST([AppId] AS NVARCHAR(20)) + '（' + [AppName] + '）' AS MenuName,AppEName,AppTypeId,Owner,Status,Remark,AddTime FROM SysApplication WHERE 1=1 ";
                sql.Append(sqlStr);
            }
            else
            {
                sql.Append("SELECT AppId,SubSystemId,AppName,CAST([AppId] AS NVARCHAR(20)) + '（' + [AppName] + '）' AS MenuName,AppEName,AppTypeId,Owner,Status,Remark,AddTime FROM SysApplication WHERE 1=1 ");
            }
            
            if (appId >= 0)
            {
                sql.AppendFormat("AND AppId={0} ", appId);
            }
            if (subSystemId >= 0)
            {
                sql.AppendFormat("AND SubSystemId={0} ", subSystemId);
            }
            if (productIdList != null && productIdList.Count > 0)
            {
                sql.Append("AND ( ");
                for (int i = 0; i < productIdList.Count; ++i)
                {
                    int productId = productIdList[i];
                    if (i == 0)
                    {
                        switch (productId)
                        {
                            case 1: //1表示国内机票
                                sql.Append("(SubSystemId >= 1000 AND SubSystemId <= 1099) ");
                                break;
                            case 2://2表示国际机票
                                sql.Append("(SubSystemId >= 1100 AND SubSystemId <= 1199) ");
                                break;
                            case 3://传3表示酒店
                                sql.Append("(SubSystemId >= 1200 AND SubSystemId <= 1299) ");
                                break;
                            case 4://传4表示公共服务
                                sql.Append("(SubSystemId >= 1300 AND SubSystemId <= 1399) ");
                                break;
                            case 5://传5表示框架
                                sql.Append("(SubSystemId >= 1400 AND SubSystemId <= 1499) ");
                                break;
                            case 6://传6表示手机
                                sql.Append("(SubSystemId >= 1500 AND SubSystemId <= 1599) ");
                                break;
                        }
                        continue;
                    }

                    switch (productId)
                    {
                        case 1: //1表示国内机票
                            sql.Append("OR (SubSystemId >= 1000 AND SubSystemId <= 1099) ");
                            break;
                        case 2://2表示国际机票
                            sql.Append("OR (SubSystemId >= 1100 AND SubSystemId <= 1199) ");
                            break;
                        case 3://传3表示酒店
                            sql.Append("OR (SubSystemId >= 1200 AND SubSystemId <= 1299) ");
                            break;
                        case 4://传4表示公共服务
                            sql.Append("OR (SubSystemId >= 1300 AND SubSystemId <= 1399) ");
                            break;
                        case 5://传5表示框架
                            sql.Append("OR (SubSystemId >= 1400 AND SubSystemId <= 1499) ");
                            break;
                        case 6://传6表示手机
                            sql.Append("OR (SubSystemId >= 1500 AND SubSystemId <= 1599) ");
                            break;
                    }
                }

                sql.Append(") ");
            }
            if (!string.IsNullOrWhiteSpace(owner))
            {
                sql.AppendFormat("AND Owner = '{0}' ", owner);
            }
            if (appTypeId >= 0)
            {
                sql.AppendFormat("AND AppTypeId={0} ", appTypeId);
            }
            if (status >= 0)
            {
                sql.AppendFormat("AND Status={0} ", status);
            }
            if (!string.IsNullOrEmpty(appName))
            {
                sql.AppendFormat("AND (AppName LIKE '%{0}%' OR AppEName LIKE '%{0}%' )", appName);
            }
            if (productIdList != null && productIdList.Count > 0)
            {
                sql.AppendFormat(" ORDER BY ModuleId, AppId ");
            }
            else
            {
                sql.AppendFormat(" ORDER BY AppId ");
            }            
            return DapperHelper<SysApplicationEntity>.GetPageList(ConnectionStr.FxDb, sql.ToString(), pageItem);
        }
        
        /// <summary>
        /// 获取单个应用信息
        /// </summary>
        /// <param name="appId">应用编号</param>
        /// <returns></returns>
        public SysApplicationEntity GetSysApplicationInfo(int appId)
        {
            string sql = "SELECT TOP 1 AppId,SubSystemId,AppName,AppEName,AppTypeId,Owner,Status,Remark,AddTime FROM SysApplication WITH(NOLOCK) WHERE AppId=@AppId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Query<SysApplicationEntity>(sql, new { AppId = appId }).SingleOrDefault<SysApplicationEntity>();
            }
        }

        public int UpdateSysApplication(SysApplicationEntity model)
        {
            string sql = "UPDATE SysApplication SET SubSystemId=@SubSystemId,AppName=@AppName,AppEName=@AppEName,AppTypeId=@AppTypeId,Owner=@Owner,Status=@Status,Remark=@Remark  WHERE AppId=@AppId";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }
        public int AddSysApplication(SysApplicationEntity model)
        {
            string sql = "INSERT INTO SysApplication(AppId,SubSystemId,AppName,AppEName,AppTypeId,Owner,Status,Remark) VALUES(@AppId,@SubSystemId,@AppName,@AppEName,@AppTypeId,@Owner,@Status,@Remark)";
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql, model);
            }
        }

        public int DeleteApplicationList(List<int> ids)
        {
            string id = string.Empty;
            if (ids == null || ids.Count <= 0) { return 0; }
            for (int index = 0; index < ids.Count; index++)
            {
                id += ids[index] + ",";
            }
            string sql = string.Format("DELETE SysApplication WHERE AppId IN ({0})", id.Trim(','));
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return conn.Execute(sql);
            }
        }


        public IEnumerable<SysApplicationEntity> GetCanBindApplication(DependentSearchRequest request, PageItem pageItem)
        {
            StringBuilder command = new StringBuilder();
            command.AppendLine("SELECT A.AppId,A.AppName,A.AppEName,A.AppTypeId,A.Owner,A.Status,");
            command.AppendLine("CASE WHEN B.DependentAppId IS NOT NULL THEN 1 ELSE 0 END AS IsDependent");
            command.AppendLine("FROM SysApplication A WITH(NOLOCK) ");
            command.AppendFormat("LEFT JOIN SysAppDependent B WITH(NOLOCK) ON A.APPID=B.DependentAppId AND B.APPID={0} ", request.AppId);
            command.AppendFormat("WHERE A.APPID<>{0} ", request.AppId);
            if (!string.IsNullOrEmpty(request.AppName))
            {
                command.AppendFormat("AND (A.AppName LIKE '%{0}%' OR A.AppEName LIKE '%{0}%') ", request.AppName);
            }
            if (request.AppTypeId >= 0)
            {
                command.AppendFormat("AND A.AppTypeId={0} ", request.AppTypeId);
            }
            if (request.Bind >= 0)
            {
                if (request.Bind == 0)
                {
                    command.AppendFormat("AND B.DependentAppId IS NULL ", request.Bind);
                }
                else
                {
                    command.AppendFormat("AND B.DependentAppId IS NOT NULL ", request.Bind); 
                }
            }
            if (request.Status >= 0)
            {
                command.AppendFormat("AND A.Status={0} ", request.Status);
            }
            if (request.SubSystemId >= 0)
            {
                command.AppendFormat("AND A.SubSystemId={0} ", request.SubSystemId);
            }
            command.AppendLine("ORDER BY AppId");
            using (var conn = new SqlConnection(ConnectionStr.FxDb))
            {
                conn.Open();
                return DapperHelper<SysApplicationEntity>.GetPageList(ConnectionStr.FxDb, command.ToString(), pageItem);
            }
        }
    }
}
