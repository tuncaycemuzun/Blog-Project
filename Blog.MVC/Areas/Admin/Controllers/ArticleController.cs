using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.ComplexTypes;
using Blog.Entities.Concrete;
using Blog.Entities.Dtos;
using Blog.MVC.Areas.Admin.Models;
using Blog.MVC.Helpers.Abstract;
using Blog.Services.Abstract;
using Blog.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Identity;

namespace Blog.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        public ArticleController(IArticleService articleService, ICategoryService categoryService,UserManager<User> userManager ,IMapper mapper, IImageHelper imageHelper):base(userManager,mapper, imageHelper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _articleService.GetAllByNonDeletedAsync();
            if (result.ResultStatus == ResultStatus.Success) return View(result.Data);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var result = await _categoryService.GetAllByNonDeletedandActiveAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                return View(new ArticleAddViewModel
                {
                    Categories = result.Data.Categories,
                    
                });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel articleAddViewModel)
        {
            if (ModelState.IsValid)
            {
                var articleAddDto = Mapper.Map<ArticleAddDto>(articleAddViewModel);
                var imageResult = await ImageHelper.Upload(articleAddDto.Title, articleAddViewModel.ThumbnailFile,
                    PictureType.Post);
                articleAddDto.Thumbnail = imageResult.Data.FullName;
                var result = await _articleService.AddAsync(articleAddDto, LoggedInUser.UserName,LoggedInUser.Id);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    TempData.Add("SuccessMessage",result.Message);
                    return RedirectToAction("Index", "Article");
                }
                else
                {
                    ModelState.AddModelError("",result.Message);
                }
            }

            var categories = await _categoryService.GetAllByNonDeletedandActiveAsync();
            articleAddViewModel.Categories = categories.Data.Categories;
            return View(articleAddViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int articleId)
        {
            var articleResult = await _articleService.GetArticleUpdateDtoAsync(articleId);
            var categoriesResult = await _categoryService.GetAllByNonDeletedandActiveAsync();
            if (articleResult.ResultStatus == ResultStatus.Success &&
                categoriesResult.ResultStatus == ResultStatus.Success)
            {
                var articleUpdateViewModal = Mapper.Map<ArticleUpdateViewModel>(articleResult.Data);
                articleUpdateViewModal.Categories = categoriesResult.Data.Categories;
                return View(articleUpdateViewModal);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(ArticleUpdateViewModel articleUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isNewThumbnailUploaded = false;
                var oldThumbnail = articleUpdateViewModel.Thumbnail;
                if (articleUpdateViewModel.ThumbnailFile != null)
                {
                    var uploadedImageResult = await ImageHelper.Upload(articleUpdateViewModel.Title,
                        articleUpdateViewModel.ThumbnailFile, PictureType.Post);
                    articleUpdateViewModel.Thumbnail = uploadedImageResult.ResultStatus == ResultStatus.Success
                        ? uploadedImageResult.Data.FullName
                        : "postImages/defaultThumbnail.jpg";
                    
                }
                if (oldThumbnail != "postImages/defaultThumbnail.jpg")
                {
                    isNewThumbnailUploaded = true;
                }

                var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(articleUpdateViewModel);
                var result = await _articleService.UpdateAsync(articleUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    if (isNewThumbnailUploaded)
                    {
                        ImageHelper.Delete(oldThumbnail);
                    }
                    TempData.Add("SuccessMessage", result.Message);
                    return RedirectToAction("Index", "Article");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                }
            }

            var categories = await _categoryService.GetAllByNonDeletedandActiveAsync();
            articleUpdateViewModel.Categories = categories.Data.Categories;
            return View(articleUpdateViewModel);
        }
    }
}
