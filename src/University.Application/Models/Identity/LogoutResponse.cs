using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.Identity
{
    public class LogoutResponse
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
    }
}
