using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.DTOs.StudentDTOs.Validators
{
    public class UpdateStudentDtoValidator:AbstractValidator<UpdateStudentDto>
    {
        public UpdateStudentDtoValidator()
        {
            Include(new IStudentDtoValidator());
        }
    }
}
