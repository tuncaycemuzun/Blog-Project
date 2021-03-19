using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Entities.Concrete;
using Blog.Entities.Dtos;
using Blog.Shared.Utilities.Results.Abstract;

namespace Blog.Services.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> Get(int categoryId);
        Task<IDataResult<ArticleListDto>> GetAll();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeleted();
        Task<IResult> Add(ArticleAddDto articleAddDto, string createdByName);
        Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        Task<IResult> Delete(int articleId, string modifiedByName);
        Task<IResult> HardDelete(int articleId);
    }
}
