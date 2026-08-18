using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Utils;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    internal class UpdateCreditWorkCodeDtoValidator:AbstractValidator<UpdateCreditWorkCodeDto>
    {
        public UpdateCreditWorkCodeDtoValidator()
        {
            RuleFor(cw => cw.Code)
                .NotEmpty()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_EMPTY);
        }
    }
}
