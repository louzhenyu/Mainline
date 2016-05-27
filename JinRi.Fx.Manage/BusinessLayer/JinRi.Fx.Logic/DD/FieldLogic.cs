using System;
using System.Collections.Generic;
using JinRi.Fx.Data.DD;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Logic.DD
{
    public class FieldLogic
    {
        private readonly FieldDal _fieldDal = new FieldDal();
        public IEnumerable<Field> GetAllField(string serverName, string databaseName, string schemaName, string tableName)
        {
            return _fieldDal.GetAll(serverName, databaseName, schemaName, tableName);
        }
        public void UpdateFieldDesc(Field field, string dealer)
        {
            _fieldDal.UpdateFieldDesc(field, dealer);
        }
    }
}
