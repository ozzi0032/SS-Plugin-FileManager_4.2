using SmartStore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Models
{
    public class BST_MediaTagModel : BaseEntity
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
    }
}