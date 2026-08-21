using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;

namespace University.Application.Models.Responses
{
    public class CreateCommandResponse:BaseCommandResponse
    {
        public IQueryDto? Record { get; set; }
    }

    public class CreateCommandResponse<T> : BaseCommandResponse where T:IQueryDto
    {
        public T? Record { get; set; }
    }
}
