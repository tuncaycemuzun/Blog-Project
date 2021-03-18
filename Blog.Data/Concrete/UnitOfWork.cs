using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Blog.Data.Concrete.EntityFramework.Contexts;
using Blog.Data.Concrete.EntityFramework.Repositories;

namespace Blog.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogContext _context;
        private EfArticleRepository _articleRepository;
        private EfCategoryRepository _efCategoryRepository;
        private EfCommentRepository _efCommentRepository;
        private EfRoleRepository _efRoleRepository;
        private EfUserRepository _efUserRepository;
        public UnitOfWork(BlogContext context)
        {
            _context = context;
        }

        public IArticleRepository Articles => _articleRepository ?? new EfArticleRepository(_context);
        public ICategoryRepository Categories => _efCategoryRepository ?? new EfCategoryRepository(_context);
        public ICommentRepository Comments => _efCommentRepository ?? new EfCommentRepository(_context);
        public IRoleRepository Roles => _efRoleRepository ?? new EfRoleRepository(_context);
        public IUserRepository Users => _efUserRepository ?? new EfUserRepository(_context);
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        
    }
}
