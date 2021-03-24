using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.Concrete;
using Blog.MVC.Helpers.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Blog.MVC.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public BaseController(UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper)
        {
            UserManager = userManager;
            Mapper = mapper;
            ImageHelper = imageHelper;
        }

        protected UserManager<User> UserManager { get; }
        protected IMapper Mapper { get; }
        protected IImageHelper ImageHelper { get; }
        protected User LoggedInUser => UserManager.GetUserAsync(HttpContext.User).Result;
    }
}
