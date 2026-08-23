using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.Responses;

namespace University.Application.Features.CourseCreditWorkRegistration.Requests.Commands
{
    public class UnregisterCourseToCreditWorkCommand:IRequest<BaseCommandResponse>
    {
        public Guid CourseCreditWorkId { get; set; }
    }
}
