using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Utils;

namespace University.Application.Models.DTOs.StudentDTOs.Validators
{
    internal class UpdateStudentEmailDtoValidator:AbstractValidator<UpdateStudentEmailDto>
    {
        public UpdateStudentEmailDtoValidator()
        {
            RuleFor(dto => dto.Email)
                .EmailAddress()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_VALID_EMAIL);
        }
    }
}
