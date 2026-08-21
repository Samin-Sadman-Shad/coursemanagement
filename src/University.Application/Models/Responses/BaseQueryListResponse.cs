using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;

namespace University.Application.Models.Responses
{
    public class BaseQueryListResponse : IBaseResponse
    {
        public bool IsSuccessful { get ; set ; }
        public HttpStatusCode Status { get ; set ; }
        public string? Message { get ; set ; }

        private List<string> _errors = new List<string>();
        public List<string> Errors
        {
            get
            {
                if (!IsSuccessful) return _errors;
                else throw new InvalidOperationException("Access denied. Errors are not available for a successful response");
            }
            set
            {
                if (!IsSuccessful) _errors = value;
                else throw new InvalidOperationException("Access denied. Errors are not available for a successful response");
            }
        }

        public virtual List<BaseQueryDto> Records { get; set; } = new List<BaseQueryDto>();
    }

    public class BaseQueryListResponse<T> : IBaseResponse where T : BaseQueryDto
    {
        public bool IsSuccessful { get; set; }
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; }

        private List<string> _errors = new List<string>();
        public List<string> Errors
        {
            get
            {
                if (!IsSuccessful) return _errors;
                else throw new InvalidOperationException("Access denied. Errors are not available for a successful response");
            }
            set
            {
                if (!IsSuccessful) _errors = value;
                else throw new InvalidOperationException("Access denied. Errors are not available for a successful response");
            }
        }
        public List<T> Records { get; set; } = new List<T>();
    }
}
