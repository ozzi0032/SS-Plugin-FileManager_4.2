using BizSol.FileManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Services
{
    public partial interface IFileManagerService
    {
        int AddHeader(BST_HeaderModel header);
        IQueryable<BST_HeaderModel> GetAllHeaders(int customerId);
        BST_HeaderModel GetFileHeader(int headerId);
        BST_MediaFileModel GetFileById(int id);
        IQueryable<BST_MediaFileModel> GetAllFiles(int userId);
        List<BST_MediaTagModel> GetFileTags(int fileId);
        IQueryable<BST_MediaTagModel> GetTags(int userId);
        void AddTag(BST_MediaTagModel tag);
        void AddTagMapping(BST_MediaFile_Tag_MappingModel tagMappingModel);
        void UpdateTagMapping(int fileId, List<int> tags);
        void DeleteFile(BST_MediaFileModel file);
    }
}