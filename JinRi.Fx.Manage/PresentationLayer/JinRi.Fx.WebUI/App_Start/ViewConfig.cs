using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;
using System.Web.Mvc;

namespace JinRi.Fx.WebUI
{
    public class ViewConfig : RazorViewEngine
    {
        public ViewConfig()
        {
            ViewLocationFormats = new[]
            {
                "~/Views/{1}/{0}.cshtml", 
                "~/Views/Eterm/{1}/{0}.cshtml",
                "~/Views/Application/{1}/{0}.cshtml",
                "~/Views/DD/{1}/{0}.cshtml"
            };
        }
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }
        public static void RegisterView()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new ViewConfig());
        }
    }
}