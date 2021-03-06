using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Entities.Concrete;
using Blog.Entities.Dtos;

namespace Blog.MVC.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int CategoriesCount { get; set; }
        public int ArticlesCount { get; set; }
        public int CommentCount { get; set; }
        public int UsersCount { get; set; }
        public ArticleListDto Articles { get; set; }
    }
}
