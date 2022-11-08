using BizSol.FileManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Data.Mappings
{
    public class BST_HeaderModelMap : EntityTypeConfiguration<BST_HeaderModel>
    {
        public BST_HeaderModelMap()
        {
            ToTable("BST_Header");
            HasKey(r => r.Id);
        }
    }
}