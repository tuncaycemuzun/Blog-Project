using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.ComplexTypes;
using Blog.Entities.Dtos;
using Blog.MVC.Helpers.Abstract;
using Blog.Shared.Utilities.Extensions;
using Blog.Shared.Utilities.Results.Abstract;
using Blog.Shared.Utilities.Results.ComplexTypes;
using Blog.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Blog.MVC.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string imgFolder = "img";
        private const string userImagesFolder = "userImages";
        private const string postImagesFolder = "postImages";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = _env.WebRootPath;
        }

        public async Task<IDataResult<ImageUploadedDto>> Upload(string Name, IFormFile pictureFile, PictureType pictureType, string folderName=null)
        {
            //Eğer folder değişkeni null gelirse resim tipine göre klasör ataması yapılır
            folderName ??= pictureType == PictureType.User ? userImagesFolder : postImagesFolder;

            //Eğer verilen dosya yoksa ilgili konuma dosya oluşturulur
            if (!Directory.Exists($"{_wwwroot}/{imgFolder}/{folderName}"))
            {
                Directory.CreateDirectory($"{_wwwroot}/{imgFolder}/{folderName}");
            }
            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            string fileExtension = Path.GetExtension(pictureFile.FileName);
            DateTime dateTime = DateTime.Now;
            string newFileName = $"{Name}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{_wwwroot}/{imgFolder}/{folderName}", newFileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            string message = pictureType == PictureType.User
                ? $"{Name} adlı kullanıcının resmi başarıyla yüklenmiştir"
                : $"{Name} adlı makalenin resmi başarıyla yüklenmiştir";
            return new DataResult<ImageUploadedDto>(ResultStatus.Success,message,new ImageUploadedDto
            {
                FullName = $"{folderName}/{newFileName}",
                OldName = oldFileName,
                Extention = fileExtension,
                FolderName = folderName,
                Path = path,
                Size = pictureFile.Length
            });
        }

        public IDataResult<ImageDeletedDto> Delete(string pictureName)
        {
            
            var fileToDelete = Path.Combine($"{_wwwroot}/{imgFolder}", pictureName);
            if (System.IO.File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                var imageDeletedDto = new ImageDeletedDto
                {
                    FullName = pictureName,
                    Extensicon = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length
                };
                System.IO.File.Delete(fileToDelete);
                return new DataResult<ImageDeletedDto>(ResultStatus.Success,imageDeletedDto);
            }
            else
            {
                return new DataResult<ImageDeletedDto>(ResultStatus.Error, $"Böyle bir resim bulunamadı.",null);
            }
        }
    }
}
