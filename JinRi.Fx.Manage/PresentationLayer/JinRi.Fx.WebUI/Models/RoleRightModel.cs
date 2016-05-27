using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinRi.Fx.WebUI.Models
{
    public class RoleRightModel
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public int AppId { get; set; }
        public bool Checked { get; set; }

    }
}