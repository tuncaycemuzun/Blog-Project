using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Utilities
{
    public static class Messages
    {
        public static class Category
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural)
                    return "Hiç bir kategori bulunamadı";
                return "Böyle bir kategori bulunamadı";
            }

            public static string Add(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla eklenmiştir";
            }
            public static string Update(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla güncellenmiştir.";
            }

            public static string Delete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla silinmiştir.";
            }

            public static string HardDelete(string categoryName)
            {
                return $"{categoryName} adlı kategori başarıyla veri tabanından silinmiştir.";
            }
        }

        public static class Article
        {
            public static string NotFound(bool isPlural)
            {
                if (isPlural)
                    return "Makaleler bulunamadı bulunamadı";
                return "Böyle bir makale bulunamadı";
            }

            public static string Add(string Title)
            {
                return $"{Title} başlıklı makale başarıyla eklendi";
            }

            public static string Update(string Title)
            {
                return $"{Title} başlıklı makale başarıyla güncellendi";
            }
            public static string Delete(string Title)
            {
                return $"{Title} başlıklı makale başarıyla silindi";
            }

            public static string HardDelete(string Title)
            {
                return $"{Title} başlıklı makale başarıyla veritabanından silindi";
            }
        }
    }
}
