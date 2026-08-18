using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Utils;

namespace University.Application.Models.DTOs.StudentDTOs.Validators
{
    internal class CreateStudentDtoValidator:AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_EMPTY)
                .MaximumLength(50)
                .WithMessage("{PropertyName} can not exceed 50 characters")
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
