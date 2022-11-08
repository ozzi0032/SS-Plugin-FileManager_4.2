using SmartStore.Web.Framework.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BizSol.FileManager
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {

            routes.MapRoute("BizSol.FileManager",
                "Plugins/BSFileManager/{action}/{id}",
                new { controller = "FileManager", action = "Index",id = UrlParameter.Optional},
                new[] { "BizSol.FileManager.Controllers" }
           )
           .DataTokens["area"] = Plugin.SystemName;
        }

        public int Priority => 0;
    }
}