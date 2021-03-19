using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Services.Abstract;
using Blog.Shared.Utilities.Results.ComplexTypes;

namespace Blog.MVC.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAll();
            return View(result.Data);
        }
        [Area("Admin")]
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }
    }
}
