using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinRi.Fx.WebUI.Models
{
    public class SysApiSearchArgs
    {
        public SysApiSearchArgs()
        {
            this.AppId = "";
            this.Status = -1;
        }
        public string ApiName { get; set; }
        public string AppId { get; set; }
        public int Status { get; set; }
    }
}