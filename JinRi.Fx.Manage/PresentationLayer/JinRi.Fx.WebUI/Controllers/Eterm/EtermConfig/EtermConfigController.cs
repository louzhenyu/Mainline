using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinRi.Fx.Entity;
using JinRi.Fx.Utility;
using JinRi.Fx.Logic;
using Webdiyer.WebControls.Mvc;
using JinRi.Fx.Web;

namespace JinRi.Fx.WebUI.Controllers.Eterm
{
    public class EtermConfigController :   ControllerBaseAdmin
    {
        EtermConfigLogic logic = new EtermConfigLogic();
        //
        // GET: /Eterm/
        //public ActionResult Index(int state = -1, int pageIndex = 1,string serverUrl="",string officeNo="")
        public ActionResult Index(int pageIndex = 1,int state=-1 , string url = "", string office = "")
        {
            PageItem pageItem = new PageItem { PageIndex = pageIndex, PageSize = 15 };
            //List<EtermConfig> etermConfigList = logic.GetEtermConfigList(state, serverUrl, officeNo, pageItem).ToList<EtermConfig>();
            List<EtermConfig> etermConfigList = logic.GetEtermConfigList(state, url, office, pageItem).ToList<EtermConfig>();
            var model = new PagedList<EtermConfig>(etermConfigList, pageItem.PageIndex, pageItem.PageSize, pageItem.TotalCount);
            return View(model); 
        }

        public ActionResult Edit(int id)
        {
            EtermConfig model = logic.GetEtermConfig(id);
            SetCmdTypeDate(model);
            return View(model);
        }
        public ActionResult Create()
        { 
            SetCmdTypeDate(null);
            return View();
        }

        private void SetCmdTypeDate(EtermConfig etermConfig)
        {
            var CmdTypeData = EnumHelper.GetItemValueList<CmdType>();
            this.ViewBag.CmdTypeData = CmdTypeData;

            var ModelState = EnumHelper.GetItemValueList<EntityStatus>();
            this.ViewBag.ModelState = new SelectList(ModelState, "Key", "Value", etermConfig == null ? 0 : etermConfig.ConfigState);

            var ConfigLevelData = EnumHelper.GetItemValueList<ConfigLevel>();
            this.ViewBag.ConfigLevelData = new SelectList(ConfigLevelData, "Key", "Value", etermConfig == null ? 0 : (int)etermConfig.ConfigLevel);

            var AirComData = new AirComLogic().GetAirComList();
            this.ViewBag.AirComData = AirComData;
        
        
        }

        [HttpPost]
        public ActionResult Delete(List<int> ids)
        {
             logic.DeleteEtermConfigList(ids);
             return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(EtermConfig etermConfig, FormCollection c)
        {
            if (ModelState.IsValid)
            {
                string types = c["chkitem"] == null ? "" : c["chkitem"].ToString();
                string AllowAirComs = c["chkAllowAirComItem"] == null ? "" : c["chkAllowAirComItem"].ToString();
                string DenyAirComs = c["chkDenyAirComItem"] == null ? "" : c["chkDenyAirComItem"].ToString();
                etermConfig.AllowAirLine = AllowAirComs;
                etermConfig.DenyAirLine = DenyAirComs;
                etermConfig.ConfigType = types;
                etermConfig.ConfigLevel = (ConfigLevel)Convert.ToInt32(c["ConfigLevelData"]);
                etermConfig.ConfigState = Convert.ToInt32(c["ModelState"]);
                etermConfig.ServerUrl = etermConfig.ServerUrl.Trim();
                etermConfig.OfficeNo = string.IsNullOrWhiteSpace(etermConfig.OfficeNo) ? "" : etermConfig.OfficeNo.Trim();
                logic.UpdateEtermConfig(etermConfig);
                return this.RefreshParent();
            }
            else
            {
                return View(etermConfig);
            }
        }
        [HttpPost]
        public ActionResult Create(EtermConfig etermConfig, FormCollection c)
        {
            if (ModelState.IsValid)
            {
                string types = c["chkitem"] == null ? "" : c["chkitem"].ToString();
                string AllowAirComs = c["chkAllowAirComItem"] == null ? "" : c["chkAllowAirComItem"].ToString();
                string DenyAirComs = c["chkDenyAirComItem"] == null ? "" : c["chkDenyAirComItem"].ToString();
                etermConfig.AllowAirLine = AllowAirComs;
                etermConfig.DenyAirLine = DenyAirComs;
                etermConfig.ConfigType = types;
                etermConfig.ConfigLevel =(ConfigLevel) Convert.ToInt32(c["ConfigLevelData"]);
                etermConfig.ConfigState = Convert.ToInt32(c["ModelState"]);
                etermConfig.ServerUrl = etermConfig.ServerUrl.Trim();
                etermConfig.OfficeNo = string.IsNullOrWhiteSpace(etermConfig.OfficeNo) ? "" : etermConfig.OfficeNo.Trim();
                logic.AddEtermConfig(etermConfig);
                return this.RefreshParent();
            }
            else
            {
                return View(etermConfig);
            }
        }
        
    }
}
