using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.StudentDTOs;

namespace University.Application.Models.Responses
{
    public class CreateStudentResponse:CreateCommandResponse<GetStudentDto>
    {
        public string? PasswordResetToken { get; set; }
    }
}
