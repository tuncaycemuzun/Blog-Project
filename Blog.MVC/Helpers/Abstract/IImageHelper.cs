using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.ComplexTypes;
using Blog.Entities.Dtos;
using Blog.Shared.Utilities.Results.Abstract;
using Microsoft.AspNetCore.Http;

namespace Blog.MVC.Helpers.Abstract
{
    
    public interface IImageHelper
    {

        Task<IDataResult<ImageUploadedDto>>
            Upload(string Name, IFormFile pictureFile,PictureType pictureType, string folderName=null);

        IDataResult<ImageDeletedDto> Delete(string pictureName);
    }
}
