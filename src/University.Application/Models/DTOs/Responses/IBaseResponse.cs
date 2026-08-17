using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.DTOs.Responses
{
    public interface IBaseResponse
    {
        bool IsSuccess { get; set; }
        List<ResponseError> Errors { get; set; }
        string ResponseMessage { get; set; }
    }
}
