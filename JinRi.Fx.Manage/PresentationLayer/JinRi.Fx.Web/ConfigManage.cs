using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Web
{
    public class ConfigManage
    {
        private static string[] _AdministratorRoles = null;

        public static string[] AdministratorRoles
        {
            get
            {
                if (_AdministratorRoles == null)
                {
                    string str = System.Configuration.ConfigurationManager.AppSettings["AdministratorRole"].Trim(',');
                    if (!string.IsNullOrEmpty(str))
                    {
                        _AdministratorRoles = str.Split(',');
                    }
                    else
                    {
                        _AdministratorRoles = new string[] { };
                    }
                }
                return ConfigManage._AdministratorRoles;
            }
        }
    }
}
