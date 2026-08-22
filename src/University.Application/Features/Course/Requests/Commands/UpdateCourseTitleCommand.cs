using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.Course.Requests.Commands
{
    public class UpdateCourseTitleCommand:IRequest<BaseCommandResponse>
    {
        public Guid CourseId {  get; set; }
        public required UpdateCourseTitleDto UpdateCourseTitleDto {  get; set; }
    }
}
