using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinRi.Fx.WebUI.Models
{
    public class AppDependentSearchArgs
    {
        public AppDependentSearchArgs()
        {
            this.SubSystemId = -1;
            this.Status = -1;
            this.AppTypeId = -1;
            this.Bind = -1;
            this.AppId = -1;
        }
        public int SubSystemId { get; set; }
        public int Status { get; set; }
        public int AppTypeId { get; set; }
        public string AppName { get; set; }
        public int Bind { get; set; }
        public int AppId { get; set; }
    }
}