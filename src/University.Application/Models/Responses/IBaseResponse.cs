using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.Responses
{
    public interface IBaseResponse
    {
        bool IsSuccessful { get; set; }
        HttpStatusCode Status { get; set; }
        string? Message { get; set; }

        List<string> Errors { get; set; }
    }
}
