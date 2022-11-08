using SmartStore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Models
{
    public class BST_MediaFile_Tag_MappingModel : BaseEntity
    {
        public int BST_MediaFile_Id { get; set; }
        public int BST_MediaTag_Id { get; set; }
    }
}