using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace JinRi.Fx.Data
{
    public class AirComDal
    {
        public IEnumerable<AirCom> GetAirComList()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT * FROM jinri.dbo.tblAircom WITH(NOLOCK) ORDER BY SORT ");
            return DapperHelper<AirCom>.GetPageList(ConnectionStr.JinRiDb, sql.ToString(), null);
        }

    }
}
