using SmartStore.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSol.FileManager
{
    public class Plugin : BasePlugin
    {
        public static string SystemName => "BizSol.FileManager";

        public override void Install()
        {
            base.Install();
        }

        public override void Uninstall()
        {
            base.Uninstall();
        }
    }
}