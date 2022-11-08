using SmartStore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Models
{
    public class BST_MediaFileModel : BaseEntity
    {
        public int CustomerId { get; set; }
        public int HeaderId { get; set; }
        public string FilePath { get; set; }
        public string FileKey { get; set; }
        public string FileContent { get; set; }
        public bool IsActive { get; set; }
        public int Size { get; set; }
        public bool Deleted { get; set; }
        public string Extension { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
    }
}