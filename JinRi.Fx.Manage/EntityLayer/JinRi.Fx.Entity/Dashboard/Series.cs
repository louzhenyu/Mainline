using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Fx.Entity
{
    public class Series
    {
        public string Name { get; set; }
   
        public List<double> YAxisValueList { get; set; }

        public Series()
        {
            YAxisValueList = new List<double>();
        }
    }
}
