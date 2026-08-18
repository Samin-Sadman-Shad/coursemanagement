using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    internal class CreateCreditWorkDtoValidator:AbstractValidator<CreateCreditWorkDto>
    {
        public CreateCreditWorkDtoValidator()
        {
            //RuleFor(cw => cw.Heading)
            //    .NotEmpty()
            //    .MaximumLength(10)
            //    .WithMessage("{PropertyName} can not exceed 10 characters")
            //    .Must(heading => heading.All(char.IsLetter))
            //    .WithMessage("{PropertyName} can contain only letters");

            //RuleFor(cw => cw.Code)
            //    .NotEmpty()
            //    .WithMessage("{PropertyName} can not be empty");
            Include(new ICreditWorkDtoValidator());

        }
    }
}
