using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Utils;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    public class UpdateCreditWorkDescriptionDtoValidator:AbstractValidator<UpdateCreditWorkDescriptionDto>
    {
        public UpdateCreditWorkDescriptionDtoValidator()
        {
            RuleFor(cw => cw.Description)
                .MaximumLength(100)
                .WithMessage(CONST_STRING.PROPERTY_ERROR_MAX_LENGTH);
        }
    }
}
