using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.Responses;

namespace University.Application.Features.CourseEnrollment.Requests.Commands
{
    public class RemoveCourseEnrollmentCommand:IRequest<BaseCommandResponse>
    {
        public Guid CourseEnrollmentId {  get; set; }
    }
}
