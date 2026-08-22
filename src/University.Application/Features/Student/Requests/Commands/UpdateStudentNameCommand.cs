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
    public class UpdateStudentNameCommand:IRequest<BaseCommandResponse>, IStudentUpdateCommand
    {
        public required Guid StudentId { get; set; }
        public required UpdateStudentNameDto StudentNameDto { get; set; }
    }
}
