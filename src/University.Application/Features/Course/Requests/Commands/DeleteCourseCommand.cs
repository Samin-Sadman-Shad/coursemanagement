using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Features.Course.Requests.Commands
{
    public class DeleteCourseCommand:IRequest<Unit>
    {
        public Guid CourseId { get; set; }
    }
}
