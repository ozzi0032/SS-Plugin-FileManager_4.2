using BizSol.FileManager.Models;
using BizSol.FileManager.Services;
using Newtonsoft.Json;
using SmartStore.Core;
using SmartStore.Core.Domain.Customers;
using SmartStore.Services.Customers;
using SmartStore.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace BizSol.FileManager.Controllers
{
    public class FileManagerController : PluginControllerBase
    {
        private readonly IFileManagerService _fileManagerService;
        private readonly IFMMediaService _mediaService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly CustomerSettings _customerSettings;
        public FileManagerController(IFileManagerService fmService, IFMMediaService fMMediaService, IWorkContext workContext, ICustomerService customerService, CustomerSettings customerSettings)
        {
            _fileManagerService = fmService;
            _mediaService = fMMediaService;
            _workContext = workContext;
            _customerService = customerService;
            _customerSettings = customerSettings;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FileList()
        {
            return View();
        }

        public JsonResult GetFileList(int take, int skip, string sort, string filter, string searchTerm)
        {
            sort = (!string.IsNullOrEmpty(sort)) ? sort : "";
            var sortOrder = JsonConvert.DeserializeObject<List<KendoSortColumn>>(sort);

            List<MediaDicViewModel> mediaDicList = new List<MediaDicViewModel>();
            List<string> tagsStringList = new List<string>();
            List<MediaFileViewModel> filesList = new List<MediaFileViewModel>();
            try
            {
                if (_workContext.CurrentCustomer.IsRegistered())
                {
                    var headers = _fileManagerService.GetAllHeaders(_workContext.CurrentCustomer.Id).ToList();
                    foreach (var header in headers)
                    {
                        var files = _fileManagerService.GetAllFiles(_workContext.CurrentCustomer.Id).Where(x => x.HeaderId == header.Id).ToList();
                        var mediaTag = _fileManagerService.GetFileTags(files[0].Id).Select(tag => new MediaTagViewModel // Files with same header have same tags
                        {
                            Id = tag.Id,
                            Name = tag.Name,
                            CustomerId = tag.CustomerId,
                        }).ToList();

                        foreach (var tag in mediaTag)
                        {
                            tagsStringList.Add(tag.Name);
                        }

                        foreach (var file in files)
                        {
                            filesList.Add(new MediaFileViewModel
                            {
                                CustomerId = file.CustomerId,
                                HeaderId = header.Id,
                                FileId = file.Id,
                                FileName = Path.GetFileName(file.FilePath),
                                FilePath = file.FilePath,
                                FileKey = file.FileKey,
                                FileContent = file.FileContent,
                                IsActive = file.IsActive,
                                Size = file.Size,
                                Deleted = file.Deleted,
                                Extension = file.Extension,
                                CreatedBy = file.CreatedBy,
                                ModifiedBy = file.ModifiedBy,
                                CreatedOnUtc = file.CreatedOnUtc.ToShortDateString(),
                                UpdatedOnUtc = file.UpdatedOnUtc.ToString() ?? "",
                                Tags = mediaTag,
                            });
                        }
                        mediaDicList.Add(new MediaDicViewModel
                        {
                            HeaderId = header.Id,
                            CustomerId = header.CustomerId,
                            Title = header.Title,
                            Description = header.Description,
                            CRC = header.CRC ?? string.Empty,
                            CodeSnippet = header.CodeSnippet,
                            TagString = string.Join(",", tagsStringList),
                            MediaTags = mediaTag,
                            MediaFiles = filesList,
                            CreatedOnUtc = files[0].CreatedOnUtc.ToString(), //Files with same header have same Creation Date
                            UpdatedOnUtc = files[0].UpdatedOnUtc.ToString() ?? "",
                        });
                        tagsStringList.Clear();
                    }

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        searchTerm = searchTerm.ToLower();
                        mediaDicList = mediaDicList.Where(model => model.Title.ToLower().Contains(searchTerm)
                        || model.Description.ToLower().Contains(searchTerm)
                        || model.TagString.ToLower().Contains(searchTerm)
                        ).ToList();
                    }

                    if (sortOrder != null && sortOrder.Count > 0)
                    {
                        Expression<Func<MediaDicViewModel, object>> sortExpression = GetSortLambda<MediaDicViewModel>(sortOrder[0].field);
                        if (sortOrder[0].dir == "asc")
                        {
                            mediaDicList = mediaDicList.AsQueryable().OrderBy(sortExpression).ToList();
                        }
                        else
                        {
                            mediaDicList = mediaDicList.AsQueryable().OrderByDescending(sortExpression).ToList();
                        }
                    }

                    mediaDicList = mediaDicList.Skip(skip).Take(take).ToList();
                    KendoListGridResult result = new KendoListGridResult();
                    result.Count = mediaDicList.Count();
                    result.KendoGridViewModel = mediaDicList;

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        private static Expression<Func<T, object>> GetSortLambda<T>(string propertyPath)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var parts = propertyPath.Split('.');
            Expression parent = param;
            foreach (var part in parts)
            {
                parent = Expression.Property(parent, part);
            }

            if (parent.Type.IsValueType)
            {
                var converted = Expression.Convert(parent, typeof(object));
                return Expression.Lambda<Func<T, object>>(converted, param);
            }
            else
            {
                return Expression.Lambda<Func<T, object>>(parent, param);
            }
        }

        public ActionResult MediaTag()
        {
            var model = new MediaDicViewModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult MediaTag(MediaDicViewModel mediaDicVM)
        {
            var fileList = new List<BST_MediaFileModel>();
            var tagList = mediaDicVM.MediaTags.Select(tag => tag.Id).ToList(); //Tag Ids list

            var headerId = _fileManagerService.AddHeader(new BST_HeaderModel
            {
                CustomerId = _workContext.CurrentCustomer.Id,
                Title = mediaDicVM.Title,
                Description = mediaDicVM.Description,
                CRC = "CRC",
                CodeSnippet = mediaDicVM.CodeSnippet,
            });

            foreach (var file in mediaDicVM.MediaFiles)
            {
                // Assign Values here
                var fileModel = new BST_MediaFileModel
                {
                    CustomerId = file.CustomerId,
                    HeaderId = headerId,
                    Extension = file.Extension,
                    FileKey = file.FileKey,
                    FilePath = file.FilePath, //Get the selected file path
                    FileContent = file.FileContent,
                    CreatedOnUtc = DateTime.Parse(file.CreatedOnUtc),
                    UpdatedOnUtc = null,
                    CreatedBy = file.CreatedBy,
                    ModifiedBy = file.ModifiedBy,
                    IsActive = file.IsActive,
                    Deleted = file.Deleted,
                    Size = file.Size, //Get size of selected file
                };
                fileList.Add(fileModel);

            }


            if (fileList.Count > 0)
            {
                foreach (var file in fileList)
                {
                    file.Id = _mediaService.SaveFile(file); // Update the Id of model
                }
                // Tags Mapping
                AddTagMapping(fileList, tagList);
                return Json(fileList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //return RedirectToAction("MediaTag");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult TagList()
        {

            var tags = _fileManagerService.GetTags(_workContext.CurrentCustomer.Id).Select(tag => new MediaTagViewModel
            {
                Id = tag.Id,
                Name = tag.Name,
                CustomerId = tag.CustomerId,
            }).ToList();

            if (tags.Count > 0)
            {
                return Json(tags, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult CreateTag(string name)
        {
            try
            {
                _fileManagerService.AddTag(new BST_MediaTagModel
                {
                    CustomerId = _workContext.CurrentCustomer.Id,
                    Name = name
                });
                var model = _fileManagerService.GetTags(_workContext.CurrentCustomer.Id)
                    .Where(tag => tag.Name == name && tag.CustomerId == _workContext.CurrentCustomer.Id)
                    .Select(tag => new MediaTagViewModel { Name = tag.Name, CustomerId = tag.CustomerId, Id = tag.Id })
                    .FirstOrDefault();
                var serializedJSON = JsonConvert.SerializeObject(model);
                return Json(serializedJSON, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw new Exception("Some error occured while creating tag!");
            }
        }

        void AddTagMapping(List<BST_MediaFileModel> fileList, List<int> tagList)
        {
            try
            {
                for (int i = 0; i < fileList.Count; i++)
                {
                    for (int j = 0; j < tagList.Count; j++)
                    {
                        var tagMapping = new BST_MediaFile_Tag_MappingModel
                        {
                            BST_MediaFile_Id = fileList[i].Id,
                            BST_MediaTag_Id = tagList[j]
                        };
                        _fileManagerService.AddTagMapping(tagMapping);
                    }
                }

            }
            catch { }
        }

        [HttpGet]
        public ActionResult FileDetail(int id)
        {
            var _mediaFilesVMList = new List<MediaFileViewModel>();
            var header = _fileManagerService.GetFileHeader(id);

            var files = _fileManagerService.GetAllFiles(_workContext.CurrentCustomer.Id).Where(file => file.HeaderId == id).ToList();

            var tags = _fileManagerService.GetFileTags(files[0].Id).Select(tag => new MediaTagViewModel //Files with same header have same tags
            {
                Id = tag.Id,
                Name = tag.Name,
                CustomerId = tag.CustomerId,
            }).ToList();

            var codeSnippets = JsonConvert.DeserializeObject<List<string>>(header.CodeSnippet);

            foreach (var file in files)
            {
                _mediaFilesVMList.Add(
                    new MediaFileViewModel
                    {
                        CustomerId = file.CustomerId,
                        HeaderId = header.Id,
                        FileId = file.Id,
                        FileName = Path.GetFileName(file.FilePath),
                        FilePath = file.FilePath,
                        FileKey = file.FileKey,
                        FileContent = file.FileContent,
                        IsActive = file.IsActive,
                        Size = file.Size,
                        Deleted = file.Deleted,
                        Extension = file.Extension,
                        CreatedBy = file.CreatedBy,
                        ModifiedBy = file.ModifiedBy,
                        CreatedOnUtc = file.CreatedOnUtc.ToShortDateString(),
                        UpdatedOnUtc = file.UpdatedOnUtc.ToString() ?? "",
                        Tags = tags,
                    });

            }

            var mediaDicViewModel = new MediaDicViewModel
            {
                HeaderId = header.Id,
                CustomerId = header.CustomerId,
                Title = header?.Title,
                Description = header?.Description,
                CRC = header?.CRC ?? "NA",
                CodeSnippet = header?.CodeSnippet,
                CreatedOnUtc = files[0].CreatedOnUtc.ToString(),
                UpdatedOnUtc = files[0].UpdatedOnUtc.ToString() ?? "",
                MediaTags = tags,
                MediaFiles = _mediaFilesVMList,
            };

            ViewData["snippets"] = codeSnippets;
            return View(mediaDicViewModel);
        }

        [HttpPost]
        public ActionResult FileDetail(MediaDicViewModel mediaDicVM)
        {
            var fileList = new List<BST_MediaFileModel>();
            var tagIDsList = mediaDicVM.MediaTags.Select(tag => tag.Id).ToList();
            var mediaHeader = new BST_HeaderModel
            {
                Id = mediaDicVM.HeaderId,
                CustomerId = mediaDicVM.CustomerId,
                Title = mediaDicVM.Title,
                Description = mediaDicVM.Description,
                CRC = mediaDicVM.CRC ?? "NA",
                CodeSnippet = mediaDicVM.CodeSnippet,
            };
            int headerId = _fileManagerService.AddHeader(mediaHeader); // Header Updated

            foreach (var file in mediaDicVM.MediaFiles)
            {
                if (file.HeaderId == 0 && file.FileId == 0) // Id is '0' means it's new file added to the existing record
                {
                    var fileModel = new BST_MediaFileModel
                    {
                        CustomerId = file.CustomerId,
                        HeaderId = headerId,
                        Extension = file.Extension,
                        FileKey = file.FileKey,
                        FilePath = file.FilePath,
                        FileContent = file.FileContent,
                        CreatedOnUtc = DateTime.Parse(file.CreatedOnUtc),
                        UpdatedOnUtc = DateTime.UtcNow,
                        CreatedBy = file.CreatedBy,
                        ModifiedBy = 0,
                        IsActive = file.IsActive,
                        Deleted = file.Deleted,
                        Size = file.Size, //Get size of selected file
                    };
                    fileList.Add(fileModel);
                }
                else
                {
                    var mediaFileModel = new BST_MediaFileModel
                    {
                        Id = file.FileId,
                        CustomerId = file.CustomerId,
                        HeaderId = file.HeaderId,
                        Extension = file.Extension,
                        FileKey = file.FileKey,
                        FilePath = file.FilePath, //Get the selected file path
                        FileContent = file.FileContent,
                        CreatedOnUtc = DateTime.Parse(file.CreatedOnUtc),
                        UpdatedOnUtc = DateTime.UtcNow, // Set the Modification Time
                        CreatedBy = file.CreatedBy,
                        ModifiedBy = file.ModifiedBy,
                        IsActive = file.IsActive,
                        Deleted = file.Deleted,
                        Size = file.Size,
                    };
                    _mediaService.SaveFile(mediaFileModel);
                }
            }

            if (fileList.Count > 0)
            {
                foreach (var file in fileList)
                {
                    file.Id = _mediaService.SaveFile(file); // Update the Id of model
                }
                // Tags Mapping
                AddTagMapping(fileList, tagIDsList);
            }
            return View(mediaDicVM);
        }

        [HttpPost]
        public JsonResult UploadFile(IEnumerable<HttpPostedFileBase> files)
        {
            List<MediaFileViewModel> _mediaFiles = new List<MediaFileViewModel>();
            foreach (var file in files)
            {
                // Convert file content to base64
                Stream fs = file.InputStream;
                BinaryReader br = new BinaryReader(fs);
                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                string contentBase64 = Convert.ToBase64String(bytes);

                // Process file and Save to the specified path
                string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                var localFile = _mediaService.ProcessFile(_path, file.InputStream); //IFile aka Local File

                var mediaFile = new MediaFileViewModel
                {
                    CustomerId = _workContext.CurrentCustomer.Id,
                    HeaderId = 0,
                    FileId = 0,
                    FileName = localFile.Name,
                    FilePath = localFile.Path,
                    FileKey = Guid.NewGuid().ToString(),
                    FileContent = contentBase64,
                    IsActive = true,
                    Size = int.Parse(file.InputStream.Length.ToString()),
                    Deleted = false,
                    Extension = localFile.Extension,
                    CreatedBy = _workContext.CurrentCustomer.Id,
                    ModifiedBy = 0,
                    CreatedOnUtc = DateTime.Now.ToShortDateString(),
                    UpdatedOnUtc = null,
                };
                _mediaFiles.Add(mediaFile);
            }
            var serializedJSON = JsonConvert.SerializeObject(_mediaFiles);
            return Json(serializedJSON, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult DownloadFile(int id)
        {
            var file = _fileManagerService.GetFileById(id);
            var fileInfo = _mediaService.GetLocalFile(file.FilePath);
            return File(fileInfo.Path, "application/force-download", Path.GetFileName(fileInfo.Path));
        }

        [HttpGet]
        public JsonResult RemoveUploadedFile(string path)
        {
            _mediaService.RemoveFile(path);
            return Json("File removed successfully", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteFile(int id)
        {
            var file = _fileManagerService.GetFileById(id);
            var header = _fileManagerService.GetFileHeader(file.HeaderId);
            _fileManagerService.DeleteFile(file);

            // Removing file from local storage
            _mediaService.RemoveFile(file.FilePath);
            return RedirectToAction("FileDetail", new { id = header.Id });
        }
    }
}