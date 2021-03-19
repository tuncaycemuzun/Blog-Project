﻿using System;
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

        public async Task<IResult> Update(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var categori = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
            if (categori != null)
            {
                categori.Name = categoryUpdateDto.Name;
                categori.Description = categoryUpdateDto.Description;
                categori.Note = categoryUpdateDto.Note;
                categori.IsActive = categoryUpdateDto.IsActive;
                categori.IsDeleted = categoryUpdateDto.IsDeleted;
                categori.ModifiedByName = modifiedByName;
                categori.ModifiedDate=DateTime.Now;
                await _unitOfWork.Categories.UpdateAsync(categori).ContinueWith(t => _unitOfWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{categoryUpdateDto.Name} adlı kategori başarıyla güncellenmiştir.");
            }

            return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı");
        }

        public async Task<IResult> Delete(int categoryId,string modifiedByName)
        {
            var categori = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (categori != null)
            {
                categori.IsDeleted = false;
                categori.ModifiedByName = modifiedByName;
                categori.ModifiedDate=DateTime.Now;
                await _unitOfWork.Categories.UpdateAsync(categori).ContinueWith(t => _unitOfWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{categori.Name} adlı kategori başarıyla silinmiştir.");
            }
            return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı");
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
            var categori = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (categori != null)
            {
                categori.IsDeleted = false;
                await _unitOfWork.Categories.DeleteAsync(categori).ContinueWith(t=>_unitOfWork.SaveAsync());
                return new Result(ResultStatus.Success, $"{categori.Name} adlı kategori başarıyla veri tabanından silinmiştir.");
            }
            return new Result(ResultStatus.Error, "Böyle bir kategori bulunamadı");
        }
    }
}
