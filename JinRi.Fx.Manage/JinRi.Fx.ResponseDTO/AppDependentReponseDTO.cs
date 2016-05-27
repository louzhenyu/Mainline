using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Fx.Entity;

namespace JinRi.Fx.ResponseDTO
{
    public class AppDependentReponseDTO
    {
        public AppDependentReponseDTO()
        {
            this.AppType = new List<SysAppTypeEntity>();
            this.Applications = new List<ApplicationDTO>();
            this.AppDependentInfo = new List<AppDependentDTO>();
        }
        public List<SysAppTypeEntity> AppType { get; set; }
        public List<ApplicationDTO> Applications { get; set; }
        public List<AppDependentDTO> AppDependentInfo { get; set; }
    }

    public class ApplicationDTO
    {
        public int AppId { get; set; }
        public int AppType { get; set; }
        public string AppName { get; set; }
        public string AppEName { get; set; }
    }

    public class AppDependentDTO
    {
        public ApplicationDTO Source { get; set; }
        public ApplicationDTO Target { get; set; }
    }
}
