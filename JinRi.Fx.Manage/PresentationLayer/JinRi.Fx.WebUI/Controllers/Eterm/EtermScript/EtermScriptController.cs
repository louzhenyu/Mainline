using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JinRi.Fx.Web;
using JinRi.Fx.Logic;
using JinRi.Fx.Utility;
using JinRi.Fx.Entity;
using Webdiyer.WebControls.Mvc;

namespace JinRi.Fx.WebUI.Controllers.EtermScript
{
    public class EtermScriptController : ControllerBaseAdmin
    {
        EtermScriptLogic logic = new EtermScriptLogic();

        //
        // GET: /EtermScript/
        [UserAuthentication]
        public ActionResult Index(string methodName = "")
        {         
            List<JinRi.Fx.Entity.EtermScript> etermScriptList = logic.GetEtermScriptPageList(new JinRi.Fx.Entity.EtermScript { MethodName = methodName }, null).ToList<JinRi.Fx.Entity.EtermScript>();
            ViewBag.EtermScriptList = etermScriptList;
            return View();    
        }

        //
        // GET: /EtermScript/Create
        [UserAuthentication]
        public ActionResult Create()
        {
            return View(new JinRi.Fx.Entity.EtermScript());
        }

        //
        // POST: /EtermScript/Create
        [HttpPost]
        [UserAuthentication]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                JinRi.Fx.Entity.EtermScript model = new JinRi.Fx.Entity.EtermScript();
                model.MethodName = collection["MethodName"];               
                model.ScriptContent = collection["ScriptContent"];
                model.Remark = collection["Remark"];        
                if (string.IsNullOrEmpty(model.MethodName))
                {
                    return this.Back("请输入方法名。");
                }
                if (string.IsNullOrEmpty(model.ScriptContent))
                {
                    return this.Back("请输入Eterm脚本内容。");
                }                
                if (logic.GetEtermScript(model.MethodName) == null)
                {
                    logic.AddEtermScript(model);
                    return this.RefreshParent();
                }
                else
                {
                    return this.Back("方法名重复。");
                }
            }
            catch (Exception ex)
            {
                return this.Back("新增Eterm脚本记录发生异常。" + ex.Message);
            }
        }

        //
        // GET: /EtermScript/Edit/5
        [UserAuthentication]
        public ActionResult Edit(int id)
        {
            var model = logic.GetEtermScript(id);
            if (model == null)
            {
                return this.Back("数据异常。");
            }
            return View(model);
        }

        //
        // POST: /EtermScript/Edit/5
        [HttpPost]
        [UserAuthentication]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                JinRi.Fx.Entity.EtermScript model = new JinRi.Fx.Entity.EtermScript();
                model.EtermScriptID = id;
                model.MethodName = collection["MethodName"];
                model.ScriptContent = collection["ScriptContent"];
                model.Remark = collection["Remark"];
                if (string.IsNullOrEmpty(model.ScriptContent))
                {
                    return this.Back("请输入Eterm脚本内容。");
                }                
                logic.UpdateEtermScript(model);
                return this.RefreshParent();
            }
            catch (Exception ex)
            {
                return this.Back("修改Eterm脚本记录发生异常。" + ex.Message);
            }
        }

        //
        // POST: /EtermScript/Delete/
        [HttpPost]
        [UserAuthentication]
        public ActionResult Delete(List<int> ids)
        {
            if (ids == null || !ids.Any())
            {
                return RedirectToAction("Index");
            }

            List<JinRi.Fx.Entity.EtermScript> list = new List<Entity.EtermScript>();
            foreach(int id in ids)
            {
                JinRi.Fx.Entity.EtermScript etermScript = new JinRi.Fx.Entity.EtermScript();
                etermScript.EtermScriptID = id;
                list.Add(etermScript);
            }

            logic.DeleteEtermScriptList(list);
            return RedirectToAction("Index");
        }
    }
}
