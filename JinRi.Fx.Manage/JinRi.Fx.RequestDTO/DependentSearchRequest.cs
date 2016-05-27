using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.RequestDTO
{
    public class DependentSearchRequest
    {
        public int AppId { get; set; }
        public int SubSystemId { get; set; }
        public int Status { get; set; }
        public int AppTypeId { get; set; }
        public string AppName { get; set; }
        public int Bind { get; set; }
    }
}
