using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Utils;

namespace University.Application.Models.DTOs.StudentDTOs.Validators
{
    internal class UpdateStudentRollDtoValidator:AbstractValidator<UpdateStudentRollDto>
    {
        public UpdateStudentRollDtoValidator()
        {
            RuleFor(dto => dto.Roll)
                .NotEmpty()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_EMPTY);
        }
    }
}
