using BizSol.FileManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Data.Mappings
{
    public class BST_MediaFileModelMap : EntityTypeConfiguration<BST_MediaFileModel>
    {
        public BST_MediaFileModelMap()
        {
            ToTable("BST_MediaFile");
            HasKey(r => r.Id);
        }
    }
}