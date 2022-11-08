using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Models
{
    public class KendoListGridResult
    {
        public int Count { get; set; }
        public virtual List<MediaDicViewModel> KendoGridViewModel { get; set; }
    }
}