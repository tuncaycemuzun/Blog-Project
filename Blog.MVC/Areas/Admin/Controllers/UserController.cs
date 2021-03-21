using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.Concrete;
using Blog.Entities.Dtos;
using Blog.MVC.Areas.Admin.Models;
using Blog.Shared.Utilities.Extensions;
using Blog.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

namespace Blog.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly SignInManager<User> _singInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper, SignInManager<User> singInManager)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
            _singInManager = singInManager;
        }

        [Authorize(Roles = "Admin")]//[Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            });

        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if (user!=null)
                {
                    var result = await _singInManager.PasswordSignInAsync(user, userLoginDto.Password,
                        userLoginDto.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        ModelState.AddModelError("","E-Posta Adresiniz veya Şifreniz Yanlış");
                        return View("UserLogin");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "E-Posta Adresiniz veya Şifreniz Yanlış");
                    return View("UserLogin");
                }
            }
            else
            {
                return View("UserLogin");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.Picture = await ImageUpload(userAddDto.Username,userAddDto.PictureFile);
                var user = _mapper.Map<User>(userAddDto);
                var result = await _userManager.CreateAsync(user, userAddDto.Password);
                if (result.Succeeded)
                {
                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} adlı kullanıcı adına sahip kullanıcı başarıyla eklenmiştir.",
                            User = user
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }

                    var useraddAjaxErrorModal = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto = userAddDto,
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(useraddAjaxErrorModal);
                }
            }
            var useraddAjaxModalStateErrorModal = JsonSerializer.Serialize(new UserAddAjaxViewModel
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(useraddAjaxModalStateErrorModal);
        }

        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> Delete(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı başarıyla silinmiştir.",
                    User = user
                });
                return Json(deletedUser);
            }
            else
            {
                string errorMessage = string.Empty;
                foreach(var error in result.Errors)
                {
                    errorMessage = $"*{error.Description}\n";
                }

                var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{user.UserName} adlı kullanıcı silinirken hata oluştu.\n{errorMessage}",
                    User = null
                });
                return Json(deletedUserErrorModel);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u=>u.Id==userId);
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userUpdateDto);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            bool isNewPictureUploaded = false;
            if (ModelState.IsValid)
            {
                var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var oldUserImage = oldUser.Picture;
                if (userUpdateDto.PictureFile != null)
                {
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.Username,userUpdateDto.PictureFile);
                    isNewPictureUploaded = true;
                }

                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPictureUploaded)
                    {
                        ImageDelete(oldUserImage);
                    }

                    var userUpdateViewModal = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellendi",
                            User = updatedUser
                        },
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial",userUpdateDto)
                    });
                    return Json(userUpdateViewModal);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                    var userUpdateErrorViewModal = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto = userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userUpdateErrorViewModal);
                }
            }
            else
            {
                var userUpdateModalStateErrorViewModal = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                });
                return Json(userUpdateModalStateErrorViewModal);
            }
        }

        [Authorize(Roles = "Admin,Editor")]
        public async Task<string> ImageUpload(string userName, IFormFile pictureFile)
        {
            string wwroot = _env.WebRootPath;
            string fileExtension = Path.GetExtension(pictureFile.FileName);
            DateTime dateTime = DateTime.Now;
            string fileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{wwroot}/userImage",fileName);
            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream);
            }

            return fileName;
        }

        [Authorize(Roles = "Admin,Editor")]
        public bool ImageDelete(string pictureName)
        {
            var wwwroot = _env.WebRootPath;
            var fileToDelete = Path.Combine($"{wwwroot}/userImage/", pictureName);
            if (System.IO.File.Exists(fileToDelete))
            {
                System.IO.File.Delete(fileToDelete);
                return true;
            }

            return false;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userListDto = JsonSerializer.Serialize(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            },
            new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }
    }
}
