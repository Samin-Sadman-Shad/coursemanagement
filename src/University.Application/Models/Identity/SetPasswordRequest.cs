using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.Identity
{
    public class SetPasswordRequest
    {
        public required Guid UserId { get; set; }
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}
