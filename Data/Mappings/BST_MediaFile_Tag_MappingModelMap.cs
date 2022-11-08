using BizSol.FileManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Data.Mappings
{
    public class BST_MediaFile_Tag_MappingModelMap : EntityTypeConfiguration<BST_MediaFile_Tag_MappingModel>
    {
        public BST_MediaFile_Tag_MappingModelMap()
        {
            ToTable("BST_MediaFile_Tag_Mapping");
            HasKey(r => r.Id);
        }
    }
}