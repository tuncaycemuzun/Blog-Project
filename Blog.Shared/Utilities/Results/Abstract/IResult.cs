using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Shared.Utilities.Results.ComplexTypes;

namespace Blog.Shared.Utilities.Results.Abstract
{
    public interface IResult
    {
        public ResultStatus ResultStatus { get;} //ResultStatus.Success or ResultStatus.Error
        public string Message { get;}
        public Exception Exception { get;}
    }
    
}
