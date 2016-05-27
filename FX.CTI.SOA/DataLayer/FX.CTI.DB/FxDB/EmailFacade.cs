using System.Collections.Generic;
using FX.CTI.DBEntity;

namespace FX.CTI.DB.FxDB
{
    public class EmailFacade
    {
        EmailCmd dbCMD = new EmailCmd();
        EmailQuery dbQuery = new EmailQuery();
        public List<Email> GetEmailList()
        {
            return dbQuery.GetEmailList();
        }
        public int UpdateEmail(Email email)
        {
            return dbCMD.UpdateEmail(email);
        }
        public int AddEmail(Email email)
        {
            return dbCMD.AddEmail(email);
        }
        public int DeleteEmail(string emailId)
        {
            return dbCMD.DeleteEmail(emailId);
        }

        public bool IsEmailIdExist(string emailId)
        {
            return dbQuery.IsEmailIdExist(emailId);
        }
    }
}
