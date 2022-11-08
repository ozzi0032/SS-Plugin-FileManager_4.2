using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using BizSol.FileManager.Data;
using BizSol.FileManager.Filters;
using BizSol.FileManager.Models;
using BizSol.FileManager.Services;
using SmartStore.Core.Data;
using SmartStore.Core.Infrastructure;
using SmartStore.Core.Infrastructure.DependencyManagement;
using SmartStore.Data;
using SmartStore.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSol.FileManager
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, bool isActiveModule)
        {

            builder.Register<IDbContext>(c => new FileManagerObjectContext(DataSettings.Current.DataConnectionString))
                           .Named<IDbContext>(FileManagerObjectContext.ALIASKEY)
                           .InstancePerRequest();

            builder.RegisterType<FileManagerService>().As<IFileManagerService>().InstancePerRequest();
            builder.RegisterType<FMMediaService>().As<IFMMediaService>().InstancePerRequest();

            builder.RegisterType<EfRepository<BST_HeaderModel>>()
                .As<IRepository<BST_HeaderModel>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(FileManagerObjectContext.ALIASKEY))
                .InstancePerRequest();
            builder.RegisterType<EfRepository<BST_MediaFileModel>>()
                .As<IRepository<BST_MediaFileModel>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(FileManagerObjectContext.ALIASKEY))
                .InstancePerRequest();
            builder.RegisterType<EfRepository<BST_MediaTagModel>>()
                .As<IRepository<BST_MediaTagModel>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(FileManagerObjectContext.ALIASKEY))
                .InstancePerRequest();
            builder.RegisterType<EfRepository<BST_MediaFile_Tag_MappingModel>>()
                .As<IRepository<BST_MediaFile_Tag_MappingModel>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(FileManagerObjectContext.ALIASKEY))
                .InstancePerRequest();
            //Filters
            builder.RegisterType<FileListFilter>()
                    .AsActionFilterFor<HomeController>().InstancePerRequest();
        }

        public int Order => 1;
    }
}