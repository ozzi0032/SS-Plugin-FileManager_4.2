using BizSol.FileManager.Models;
using SmartStore.Core;
using SmartStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizSol.FileManager.Services
{
    public partial class FileManagerService : IFileManagerService
    {
        private readonly IRepository<BST_HeaderModel> _mediaHeaderRepo;
        private readonly IRepository<BST_MediaFileModel> _mediaFileRepo;
        private readonly IRepository<BST_MediaTagModel> _mediaTagRepo;
        private readonly IRepository<BST_MediaFile_Tag_MappingModel> _tagMappingRepo;
        private readonly IWorkContext _workContext;

        public FileManagerService(IRepository<BST_HeaderModel> mediaHeaderRepo, IRepository<BST_MediaFileModel> mediaFileRepo, IRepository<BST_MediaTagModel> tagRepo, IRepository<BST_MediaFile_Tag_MappingModel> mappingRepo, IWorkContext workContext)
        {
            _mediaHeaderRepo = mediaHeaderRepo;
            _mediaFileRepo = mediaFileRepo;
            _mediaTagRepo = tagRepo;
            _tagMappingRepo = mappingRepo;
            _workContext = workContext;
        }

        public int AddHeader(BST_HeaderModel header)
        {
            try
            {
                if (header.Id == 0)
                {
                    _mediaHeaderRepo.Insert(header);
                    return header.Id;
                }
                else
                {
                    _mediaHeaderRepo.Update(header);
                    return header.Id;
                }
            }
            catch
            {
                throw new Exception("Header not Added!");
            }
        }

        public IQueryable<BST_HeaderModel> GetAllHeaders(int customerId)
        {
            try
            {
                var headers = _mediaHeaderRepo.Table.Where(x => x.CustomerId == customerId);
                return headers;
            }
            catch
            {
                throw new Exception("Header not found!");
            }
        }

        public BST_HeaderModel GetFileHeader(int headerId)
        {
            try
            {
                var header = _mediaHeaderRepo.Table.Where(x => x.Id == headerId).FirstOrDefault();
                return header;
            }
            catch
            {
                throw new Exception("Header not found!");
            }
        }

        public BST_MediaFileModel GetFileById(int id)
        {
            try
            {
                var file = _mediaFileRepo.Table.Where(x => x.Id == id).FirstOrDefault();
                return file;
            }
            catch
            {
                throw new Exception("File not found!");
            }
        }

        public IQueryable<BST_MediaFileModel> GetAllFiles(int customerId)
        {
            try
            {
                var files = _mediaFileRepo.Table.Where(file => file.CustomerId == customerId);
                return files;
            }
            catch
            {
                throw new Exception("No files found!");
            }
        }

        public List<BST_MediaTagModel> GetFileTags(int fileId)
        {
            try
            {
                var _queryTags = (from t in _mediaTagRepo.Table
                                  join m in _tagMappingRepo.Table on t.Id equals m.BST_MediaTag_Id
                                  join c in _mediaFileRepo.Table on m.BST_MediaFile_Id equals c.Id
                                  where m.BST_MediaFile_Id == fileId
                                  select t).ToList();
                return _queryTags;
            }
            catch
            {
                throw new Exception("File does not contains tags!");
            }
        }

        public IQueryable<BST_MediaTagModel> GetTags(int customerId)
        {
            try
            {
                var tags = _mediaTagRepo.Table.Where(tag => tag.CustomerId == customerId);
                return tags;
            }
            catch
            {
                throw new Exception("Tags not available!");
            }
        }

        public void AddTag(BST_MediaTagModel tagModel)
        {
            try
            {
                if (tagModel.Id == 0)
                {
                    _mediaTagRepo.Insert(tagModel);
                }
                else
                {
                    _mediaTagRepo.Update(tagModel);
                }
            }
            catch
            {
                throw new Exception("Adding tag failed!");
            }
        }

        public void AddTagMapping(BST_MediaFile_Tag_MappingModel tagMappingModel)
        {
            try
            {
                if (tagMappingModel.Id == 0)
                {
                    _tagMappingRepo.Insert(tagMappingModel);
                }
                else
                {
                    _tagMappingRepo.Update(tagMappingModel);
                }
            }
            catch
            {
                throw new Exception("Tag mapping failed!");
            }
        }

        public void UpdateTagMapping(int fileId, List<int> tags)
        {
            try
            {
                var mappings = _tagMappingRepo.Table.Where(map => map.BST_MediaFile_Id == fileId).ToList();
                for (var i = 0; i < tags.Count; i++)
                {
                   int index = mappings.IndexOf(_tagMappingRepo.Table.Where(map => map.BST_MediaTag_Id != tags[i]).FirstOrDefault());
                    if(index != -1)
                    {
                        _tagMappingRepo.Delete(mappings[index]);
                        mappings.RemoveAt(index);
                    }
                    else
                    {
                        mappings.Add(new BST_MediaFile_Tag_MappingModel
                        {
                            BST_MediaFile_Id = fileId,
                            BST_MediaTag_Id = tags[i],
                        });
                    }
                }

                foreach (var m in mappings)
                {
                    if(m.Id == 0)
                    {
                        _tagMappingRepo.Insert(m);
                    }
                    else
                    {
                        _tagMappingRepo.Update(m);
                    }
                }
            }
            catch
            {
                throw new Exception("Tag mapping update failed!");
            }
        }

        public void DeleteFile(BST_MediaFileModel file)
        {
            try
            {
                // Removing tags mapping
                var tagMappings = (from m in _tagMappingRepo.Table
                                   where m.BST_MediaFile_Id == file.Id
                                   select m).ToList();
                foreach (var tagMapping in tagMappings)
                {
                    _tagMappingRepo.Delete(tagMapping);
                }
                _mediaFileRepo.Delete(file);
            }
            catch
            {
                throw new Exception("File not Deleted!");
            }
        }
    }
}