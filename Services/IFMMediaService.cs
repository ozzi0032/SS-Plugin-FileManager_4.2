using BizSol.FileManager.Models;
using SmartStore.Core.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Services
{
    public partial interface IFMMediaService
    {
        IFile GetLocalFile(string path);
        IFile ProcessFile(string path,Stream stream);
        int SaveFile(BST_MediaFileModel mediaFileModel);
        void RemoveFile(string path);
        
    }
}