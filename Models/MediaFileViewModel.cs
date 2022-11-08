using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Models
{
    public class MediaFileViewModel
    {
        public int CustomerId { get; set; }
        public int HeaderId { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileKey { get; set; }
        public string FileContent { get; set; }
        public bool IsActive { get; set; }
        public int Size { get; set; }
        public bool Deleted { get; set; }
        public string Extension { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public string CreatedOnUtc { get; set; }
        public string UpdatedOnUtc { get; set; }
        public virtual List<MediaTagViewModel> Tags { get; set; }
    }
}