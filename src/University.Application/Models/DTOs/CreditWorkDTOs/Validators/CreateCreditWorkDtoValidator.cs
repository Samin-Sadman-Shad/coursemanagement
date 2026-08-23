using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    public class CreateCreditWorkDtoValidator:AbstractValidator<CreateCreditWorkDto>
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public CreateCreditWorkDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            Include(new ICreditWorkDtoValidator(_unitOfWork));
        }
    }
}
