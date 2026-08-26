using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.Responses;

namespace University.Application.Models.Identity
{
    public class RegistrationResponse:BaseCommandResponse
    {
        public Guid UserId { get; set; } 

    }
}
