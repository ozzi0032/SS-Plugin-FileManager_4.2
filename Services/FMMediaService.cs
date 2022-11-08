using BizSol.FileManager.Models;
using SmartStore;
using SmartStore.Core.Data;
using SmartStore.Core.IO;
using SmartStore.Data;
using SmartStore.Data.Utilities;
using SmartStore.Services.Media;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Services
{
    public partial class FMMediaService : LocalFileSystem, IFMMediaService
    {


        private readonly IRepository<BST_MediaFileModel> _mediaFileRepo;

        public FMMediaService(IRepository<BST_MediaFileModel> mediaFileRepo)
        {
            _mediaFileRepo = mediaFileRepo;
        }
        
        public IFile GetLocalFile(string path)
        {
            return GetFile(path);
        }

        public IFile ProcessFile(string path, Stream stream)
        {
            try
            {
                var fileInfo = CreateFile(path);  // Creates the File to the storage provider
                SaveStream(fileInfo.Path, stream);  // Save the input stream
                return fileInfo;
            }
            catch
            {
                throw new Exception("File uploading failed!!!");
            }

        }

        public int SaveFile(BST_MediaFileModel mediaFileModel)
        {
            try
            {
                if (mediaFileModel == null) return 0;
                if (mediaFileModel.Id == 0)
                {
                    _mediaFileRepo.Insert(mediaFileModel);
                    return mediaFileModel.Id;
                }
                else
                {
                    _mediaFileRepo.Update(mediaFileModel);
                    return mediaFileModel.Id;
                }
            }
            catch
            {
                throw new Exception("Error occured while uploading file!");
            }
        }

        public void RemoveFile(string path)
        {
            try
            {
                DeleteFile(path);
            }
            catch
            {
                throw new Exception("File not removed from local storage!!!");
            }
        }
    }
}