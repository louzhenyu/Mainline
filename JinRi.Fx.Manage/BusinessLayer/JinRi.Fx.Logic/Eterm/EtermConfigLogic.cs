using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Fx.Data;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;

namespace JinRi.Fx.Logic
{
    public class EtermConfigLogic
    {
        private EtermConfigDal etermConfigDal = new EtermConfigDal();

        public bool AddEtermConfig()
        {
            return false;
        }

        public IEnumerable<EtermConfig> GetEtermConfigList(int state = 0,  string serverUrl = "",string officeNo="",PageItem pageItem = null)
        {
            return etermConfigDal.GetEtermConfigList(state, serverUrl, officeNo,pageItem);
        }

        public EtermConfig GetEtermConfig(int id)
        {
            return etermConfigDal.GetEtermConfig(id);
        }
        public bool UpdateEtermConfig(EtermConfig etermConfig)
        {
            if (!string.IsNullOrEmpty(etermConfig.ConfigType)) { etermConfig.ConfigType =etermConfig.ConfigType.TrimEnd(',')+","; }
            bool ret= etermConfigDal.UpdateEtermConfig(etermConfig)>0;
            CacheManager.ClearCache(CacheManager.EtermConfigCacheKey);
            return ret;
        }

        public bool DeleteEtermConfigList(List<int> ids)
        {
            bool ret = etermConfigDal.DeleteEtermConfigList(ids) > 0;
            CacheManager.ClearCache(CacheManager.EtermConfigCacheKey);
            return ret;
        }
        public bool AddEtermConfig(EtermConfig etermConfig)
        {
            bool ret = etermConfigDal.AddEtermConfig(etermConfig) > 0;
            CacheManager.ClearCache(CacheManager.EtermConfigCacheKey);
            return ret;
        }

    }
}
