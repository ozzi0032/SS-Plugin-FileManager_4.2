using SmartStore.Core;
using SmartStore.Core.Domain.Customers;
using SmartStore.Core.Plugins;
using SmartStore.Services.Customers;
using SmartStore.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BizSol.FileManager.Filters
{
    public class FileListFilter : IActionFilter
    {
        private readonly IWorkContext _workContext;
        private readonly IPluginFinder _pluginFinder;

        public FileListFilter(IWorkContext workContext, IPluginFinder pluginFinder)
        {
            _workContext = workContext;
            _pluginFinder = pluginFinder;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var plugin = _pluginFinder.GetPluginDescriptorBySystemName("BizSol.FileManager");

            if (context == null ||
                context.ActionDescriptor == null ||
                context.HttpContext == null ||
                context.HttpContext.Request == null)
            {
                return;
            }

            if (_workContext.CurrentCustomer.IsRegistered())
            {
                if (plugin != null && plugin.Installed)
                {
                    if (context.Controller.GetType().Equals(typeof(HomeController)) &&
                    context.ActionDescriptor.ActionName.Equals("Index"))
                    {
                        if (context.HttpContext.Request.HttpMethod == "GET")
                        {
                            var _routeValues = new RouteValueDictionary(
                                new
                                {
                                    controller = "FileManager",
                                    action = "FileList"
                                }
                                );
                            context.Result = new RedirectToRouteResult("BizSol.FileManager", _routeValues);
                        }
                    }
                }
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}