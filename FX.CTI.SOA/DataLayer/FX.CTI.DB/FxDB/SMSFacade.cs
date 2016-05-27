using System.Collections.Generic;
using FX.CTI.DBEntity;

namespace FX.CTI.DB.FxDB
{
    public class SMSFacade
    {
        SMSCmd dbCMD = new SMSCmd();
        SMSQuery dbQuery = new SMSQuery();
        public List<SMS> GetSMSList()
        {
            return dbQuery.GetSMSList();
        }
        public int UpdateSMS(SMS sms)
        {
            return dbCMD.UpdateSMS(sms);
        }
        public int AddSMS(SMS sms)
        {
            return dbCMD.AddSMS(sms);
        }
        public int DeleteSMS(string smsId)
        {
            return dbCMD.DeleteSMS(smsId);
        }
        public bool IsSMSIdExist(string smsId)
        {
            return dbQuery.IsSMSIdExist(smsId);
        }
    }
}
