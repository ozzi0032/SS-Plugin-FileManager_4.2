using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Models
{
    public class MediaDicViewModel
    {
        public int HeaderId { get; set; }
        public int CustomerId { get; set; }
        [DisplayName("Title")]
        [Required]
        public string Title { get; set; }
        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }
        public string CRC { get; set; }
        public string CodeSnippet { get; set; }
        public string TagString { get; set; }
        public string CreatedOnUtc { get; set; }
        public string UpdatedOnUtc { get; set; }
        public virtual List<MediaTagViewModel> MediaTags { get; set; }
        public virtual List<MediaFileViewModel> MediaFiles { get; set; }

    }
}