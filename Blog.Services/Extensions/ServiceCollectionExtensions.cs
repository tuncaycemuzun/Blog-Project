using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Blog.Data.Concrete;
using Blog.Data.Concrete.EntityFramework.Contexts;
using Blog.Entities.Concrete;
using Blog.Services.Abstract;
using Blog.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<BlogContext>();
            serviceCollection.AddIdentity<User, Role>(options=>
            {
                //Şifre Seçenekleri
                options.Password.RequireDigit = false;//Şifrede rakam olması zorunlu mu
                options.Password.RequiredLength = 5; //Zorunlu şifre uzunluğu
                options.Password.RequiredUniqueChars = 0;//Unique karakterden en az kaç tane olması gerekiyor?
                options.Password.RequireNonAlphanumeric = false;//Özel karakterler kullanılsın mı?
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                //Kullanıcı Adı ve Email Seçenekleri
                options.User.AllowedUserNameCharacters= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<BlogContext>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            return serviceCollection;

        }
    }
}
