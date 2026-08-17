using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.DTOs.Responses
{
    public class ResponseError
    {
        public RepositoryEnum ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}
