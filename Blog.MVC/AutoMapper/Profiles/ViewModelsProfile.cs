using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Entities.Dtos;
using Blog.MVC.Areas.Admin.Models;

namespace Blog.MVC.AutoMapper.Profiles
{
    public class ViewModelsProfile : Profile
    {
        public ViewModelsProfile()
        {
            CreateMap<ArticleAddViewModel, ArticleAddDto>();
            CreateMap<ArticleUpdateDto, ArticleUpdateViewModel>().ReverseMap();
        }
    }
}
