using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Utils;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    public class UpdateCreditWorkDtoValidator:AbstractValidator<UpdateCreditWorkDto>
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public UpdateCreditWorkDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            Include(new ICreditWorkDtoValidator(_unitOfWork));
        }
    }
}
