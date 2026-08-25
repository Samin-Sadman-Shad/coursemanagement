using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.Identity
{
    public class SetPasswordResponse
    {
        public bool IsSuccessful { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
