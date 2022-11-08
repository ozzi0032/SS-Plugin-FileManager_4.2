using BizSol.FileManager.Data.Mappings;
using SmartStore.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Data
{
    public class FileManagerObjectContext : ObjectContextBase
    {
        public const string ALIASKEY = "bs_object_context_filemanager";

        public FileManagerObjectContext(string nameOrConnectionString) : base(nameOrConnectionString, ALIASKEY)
        {
            AutoCommitEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BST_HeaderModelMap());
            modelBuilder.Configurations.Add(new BST_MediaFileModelMap());
            modelBuilder.Configurations.Add(new BST_MediaTagModelMap());
            modelBuilder.Configurations.Add(new BST_MediaFile_Tag_MappingModelMap());

            Database.SetInitializer<FileManagerObjectContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}