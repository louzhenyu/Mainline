using System;
using System.Collections.Generic;
using JinRi.Fx.Data.DD;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Logic.DD
{
    public class TableLogic
    {
        private readonly TableDal _tableDal = new TableDal();
        public IEnumerable<Table> GetAllTable(string serverName, string databaseName, string schemaName)
        {
            return _tableDal.GetAll(serverName, databaseName, schemaName);
        }
        public void UpdateTableDesc(Table table, string dealer)
        {
            _tableDal.UpdateTableDesc(table, dealer);
        }
    }
}
