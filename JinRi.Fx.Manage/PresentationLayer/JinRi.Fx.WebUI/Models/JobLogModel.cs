using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinRi.Fx.WebUI.Models
{
    public class JobLogModel
    {
        public JobLogModel()
        {
            this.StartTime = DateTime.Parse(DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:00:00"));
            this.EndTime = DateTime.Parse(DateTime.Now.AddHours(1).ToString("yyyy-MM-dd HH:00:00"));
        }
        public int JobId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}