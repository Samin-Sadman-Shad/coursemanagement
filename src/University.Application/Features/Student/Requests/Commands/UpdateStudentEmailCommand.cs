using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.StudentDTOs;

namespace University.Application.Features.Student.Requests.Commands
{
    public class UpdateStudentEmailCommand:IRequest, IStudentUpdateCommand
    {
        public required Guid StudentId { get; set; }
        public required UpdateStudentEmailDto Student { get; set; }
    }
}
