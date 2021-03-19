using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.Data.Concrete.EntityFramework.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();//Identity alanımız.
            builder.Property(a => a.Title).HasMaxLength(100);
            builder.Property(a => a.Title).IsRequired(true);
            builder.Property(a => a.Content).IsRequired(true);
            builder.Property(a => a.Content).HasColumnType("NVARCHAR(MAX)");
            builder.Property(a => a.Date).IsRequired(true);
            builder.Property(a => a.SeoAuthor).IsRequired(true);
            builder.Property(a => a.SeoAuthor).HasMaxLength(50);
            builder.Property(a => a.SeoDescription).HasMaxLength(150);
            builder.Property(a => a.SeoDescription).IsRequired(true);
            builder.Property(a => a.SeoTags).IsRequired(true);
            builder.Property(a => a.SeoTags).HasMaxLength(70);
            builder.Property(a => a.ViewsCount).IsRequired(true);
            builder.Property(a => a.CommentCount).IsRequired(true);
            builder.Property(a => a.Thumbnail).IsRequired(true);
            builder.Property(a => a.Thumbnail).HasMaxLength(250);
            builder.Property(a => a.CreatedByName).IsRequired(true);
            builder.Property(a => a.CreatedByName).HasMaxLength(50);
            builder.Property(a => a.ModifiedByName).IsRequired(true);
            builder.Property(a => a.ModifiedByName).HasMaxLength(50);
            builder.Property(a=>a.CreatedDate).IsRequired(true);
            builder.Property(a=>a.ModifiedDate).IsRequired(true);
            builder.Property(a => a.IsActive).IsRequired(true);
            builder.Property(a => a.IsDeleted).IsRequired(true);
            builder.Property(a => a.Note).HasMaxLength(500);
            builder.HasOne<Category>(a => a.Category).WithMany(c=>c.Articles).HasForeignKey(a=>a.CategoryId);
            builder.HasOne<User>(a => a.User).WithMany(u => u.Article).HasForeignKey(a => a.UserId);
            builder.ToTable("Articles");
            builder.HasData(
                new Article
                {
                    Id=1,
                    CategoryId = 1,
                    Title = "C# 9.0 ve .NET 5 Yenilikleri",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Thumbnail = "Default.jpg",
                    SeoDescription = "C# 9.0 ve .NET 5 Yenilikleri",
                    SeoTags = "C#, C# 9, .NET5, .NET FRAMEWORK, .NET Core",
                    SeoAuthor = "Tuncay Cem Uzun",
                    Date = DateTime.Now,
                    UserId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "C# 9.0 ve .NET 5 Yenilikleri",
                    ViewsCount = 100,
                    CommentCount = 1
                },
                new Article
                {
                    Id = 2,
                    CategoryId = 2,
                    Title = "C++ 11 ve 19 Yenilikleri",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Thumbnail = "Default.jpg",
                    SeoDescription = "C++ 11 ve 19 Yenilikleri",
                    SeoTags = "C++ 11 ve 19 Yenilikleri",
                    SeoAuthor = "Tuncay Cem Uzun",
                    Date = DateTime.Now,
                    UserId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "C++ 11 ve 19 Yenilikleri",
                    ViewsCount = 295,
                    CommentCount = 1
                },
                new Article
                {
                    Id = 3,
                    CategoryId = 3,
                    Title = "Javascript ES2019 ve ES2020 Yenilikleri",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                    Thumbnail = "Default.jpg",
                    SeoDescription = "Javascript ES2019 ve ES2020 Yenilikleri",
                    SeoTags = "Javascript ES2019 ve ES2020 Yenilikleri",
                    SeoAuthor = "Tuncay Cem Uzun",
                    Date = DateTime.Now,
                    UserId = 1,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note = "Javascript ES2019 ve ES2020 Yenilikleri",
                    ViewsCount = 12,
                    CommentCount = 1
                });
        }
    }
}
