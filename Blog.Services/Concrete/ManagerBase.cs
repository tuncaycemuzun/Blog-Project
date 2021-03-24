using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data.Abstract;

namespace Blog.Services.Concrete
{
    public class ManagerBase 
    {
        protected IUnitOfWork UnitOfWork;
        protected readonly IMapper Mapper;

        public ManagerBase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }
    }
}
