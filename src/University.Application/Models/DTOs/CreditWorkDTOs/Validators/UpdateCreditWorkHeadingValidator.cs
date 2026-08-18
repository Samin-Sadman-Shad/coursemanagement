using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Utils;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    internal class UpdateCreditWorkHeadingValidator:AbstractValidator<UpdateCreditWorkHeadingDto>
    {
        public UpdateCreditWorkHeadingValidator()
        {
            RuleFor(cw => cw.Heading)
               .NotEmpty()
               .MaximumLength(10)
               .WithMessage("{PropertyName} can not exceed 10 characters")
               .Must(heading => heading.All(char.IsLetter))
               .WithMessage(CONST_STRING.PROPERTY_ERROR_LETTERS_ONLY);
        }
    }
}
