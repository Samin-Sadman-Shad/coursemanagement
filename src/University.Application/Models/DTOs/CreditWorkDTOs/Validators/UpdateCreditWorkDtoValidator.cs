using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Utils;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    public class UpdateCreditWorkDtoValidator:AbstractValidator<UpdateCreditWorkDto>
    {
        public UpdateCreditWorkDtoValidator()
        {
            Include(new ICreditWorkDtoValidator());
        }
    }
}
