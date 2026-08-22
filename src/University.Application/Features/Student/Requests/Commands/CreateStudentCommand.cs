using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.Student.Requests.Commands
{
    public class CreateStudentCommand:IRequest<CreateCommandResponse<GetStudentDto>>
    {
        public required CreateStudentDto CreateStudentDto { get; set; }
    }
}
