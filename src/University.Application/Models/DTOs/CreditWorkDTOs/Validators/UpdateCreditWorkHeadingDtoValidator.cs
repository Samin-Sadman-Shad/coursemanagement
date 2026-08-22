using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Utils;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    public class UpdateCreditWorkHeadingDtoValidator:AbstractValidator<UpdateCreditWorkHeadingDto>
    {
        public UpdateCreditWorkHeadingDtoValidator()
        {
            RuleFor(cw => cw.Heading)
               .NotEmpty()
               .MaximumLength(10)
               .WithMessage(CONST_STRING.PROPERTY_ERROR_MAX_LENGTH)
               .Must(heading => heading.All(char.IsLetter))
               .WithMessage(CONST_STRING.PROPERTY_ERROR_LETTERS_ONLY);
        }
    }
}
