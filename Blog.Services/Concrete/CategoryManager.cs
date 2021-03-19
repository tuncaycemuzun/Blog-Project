using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Abstract;
using Blog.Entities.Concrete;
using Blog.Entities.Dtos;
using Blog.Services.Abstract;
using Blog.Shared.Utilities.Results.Abstract;
using Blog.Shared.Utilities.Results.ComplexTypes;
using Blog.Shared.Utilities.Results.Concrete;

namespace Blog.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IDataResult<Category>> Get(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c=>c.Id==categoryId,c=>c.Articles);
            if (category != null)
            {
                return new DataResult<Category>(ResultStatus.Success, category);
            }
            return new DataResult<Category>(ResultStatus.Error, "Böyle bir kategori bulunamadı",null);
        }

        public async Task<IDataResult<IList<Category>>> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<IList<Category>>(ResultStatus.Success, categories);
            }
            return new DataResult<IList<Category>>(ResultStatus.Error, "Hiç bir kategori bulunamadı", null);
        }

        public async Task<IDataResult<IList<Category>>> GetAllByNonDeleted()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<IList<Category>>(ResultStatus.Success, categories);
            }
            return new DataResult<IList<Category>>(ResultStatus.Error, "Hiç bir kategori bulunamadı", null);
        }

        public async Task<IResult> Add(CategoryAddDto categoryAddDto, string createdByName)
        {
            await _unitOfWork.Categories.AddAsync(new Category
            {
                Name = categoryAddDto.Name,
                Description = categoryAddDto.Description,
                Note = categoryAddDto.Note,
                IsActive = categoryAddDto.IsActive,
                CreatedByName = createdByName,
                CreatedDate = DateTime.Now,
                ModifiedByName = createdByName,
                ModifiedDate = DateTime.Now,
                IsDeleted = false
            }).ContinueWith(t=>_unitOfWork.SaveAsync());
            //await _unitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, $"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir");
        }

        public Task<IResult> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Delete(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> HardDelete(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
