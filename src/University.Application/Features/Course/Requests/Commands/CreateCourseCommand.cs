using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CourseDTOs;

namespace University.Application.Features.Course.Requests.Commands
{
    public class CreateCourseCommand:IRequest<Guid>
    {
        public required CreateCourseDto CreateCourseDto { get; set; }
    }
}
