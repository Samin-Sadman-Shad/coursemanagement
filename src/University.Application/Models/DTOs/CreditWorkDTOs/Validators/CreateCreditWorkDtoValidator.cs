using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    public class CreateCreditWorkDtoValidator:AbstractValidator<CreateCreditWorkDto>
    {
        public CreateCreditWorkDtoValidator()
        {
            Include(new ICreditWorkDtoValidator());
        }
    }
}
