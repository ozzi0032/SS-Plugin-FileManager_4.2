using BizSol.FileManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Data.Mappings
{
    public class BST_MediaTagModelMap : EntityTypeConfiguration<BST_MediaTagModel>
    {
        public BST_MediaTagModelMap()
        {
            ToTable("BST_MediaTag");
            HasKey(r => r.Id);
        }
    }
}