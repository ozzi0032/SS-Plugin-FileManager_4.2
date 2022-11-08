using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizSol.FileManager.Models
{
    public class KendoFilter
    {
        public string logic { get; set; }
        public Filter[] filters { get; set; }
    }
}