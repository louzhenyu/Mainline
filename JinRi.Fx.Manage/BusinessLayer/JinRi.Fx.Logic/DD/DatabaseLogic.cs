using System;
using System.Collections.Generic;
using JinRi.Fx.Data.DD;
using JinRi.Fx.Entity.DD;

namespace JinRi.Fx.Logic.DD
{
    public class DatabaseLogic
    {
        private readonly DatabaseDal _databaseDal = new DatabaseDal();
        public IEnumerable<Database> GetAllDatabase(string serverName)
        {
            return _databaseDal.GetAll(serverName);
        }

        public void UpdateDbDesc(Database database, string dealer)
        {
            _databaseDal.UpdateDbDesc(database, dealer);
        }
    }
}
