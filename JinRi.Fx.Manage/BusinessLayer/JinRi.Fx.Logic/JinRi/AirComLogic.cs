using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;

namespace JinRi.Fx.Logic
{
    public class AirComLogic
    {
        private AirComDal AirComDal = new AirComDal();


        public IEnumerable<AirCom> GetAirComList()
        {
            return AirComDal.GetAirComList();
        }

    }
}
