using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Utils;

namespace University.Application.Models.DTOs.StudentDTOs.Validators
{
    public class IStudentDtoValidator:AbstractValidator<IStudentDto>
    {
        public IStudentDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_EMPTY)
                .MaximumLength(50)
                .WithMessage(CONST_STRING.PROPERTY_ERROR_MAX_LENGTH)
                .Must(name => name.All(char.IsLetter))
                .WithMessage(CONST_STRING.PROPERTY_ERROR_LETTERS_ONLY);

            RuleFor(dto => dto.Email)
                .EmailAddress()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_VALID_EMAIL);

            RuleFor(dto => dto.Roll)
                .NotEmpty()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_EMPTY);
        }
    }
}
