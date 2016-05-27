using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Fx.Data.DD;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Logic.DD
{
    public class AdvancedFieldLogic
    {
        private readonly AdvancedFieldDal _advancedFieldDalDal = new AdvancedFieldDal();

        public IEnumerable<Field> GetField(string serverName, string databaseName, string tableName,
            string fieldNameKeyword, string fieldDescriptionKeyword, int pageIndex, int pageSize, out int totalRecords)
        {
            return _advancedFieldDalDal.GetField(serverName, databaseName, tableName, fieldNameKeyword,
                fieldDescriptionKeyword, pageIndex, pageSize, out totalRecords);
        }
    }
}
