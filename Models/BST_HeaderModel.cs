using SmartStore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Models
{
    public class BST_HeaderModel : BaseEntity
    {
        public int CustomerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CRC { get; set; }
        public string CodeSnippet { get; set; }
    }
}