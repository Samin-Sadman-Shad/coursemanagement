using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.Responses
{
    public class BaseCommandResponse:IBaseResponse
    {
        public Guid RecordId { get; set; }
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        private List<string> _errors = new List<string>();
        public HttpStatusCode Status {  get; set; }
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
    }
}
