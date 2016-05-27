using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinRi.Fx.WebUI.Models
{
    public class SysApplicationSearchArgs
    {
        public SysApplicationSearchArgs()
        {
            this.SubSystemId = -1;
            this.AppTypeId = -1;
            this.Status = -1;
        }
        public string AppId { get; set; }
        public int SubSystemId { get; set; }
        public string AppName { get; set; }
        public int AppTypeId { get; set; }
        public int Status { get; set; }

    }
}