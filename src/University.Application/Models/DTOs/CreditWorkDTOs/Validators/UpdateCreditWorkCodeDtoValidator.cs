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
    public class UpdateCreditWorkCodeDtoValidator:AbstractValidator<UpdateCreditWorkCodeDto>
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public UpdateCreditWorkCodeDtoValidator(IUnitOfWork uow, Guid creditWorkId)
        {
            _unitOfWork = uow;
            RuleFor(cw => cw.Code)
                .NotEmpty()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_EMPTY)
                .MustAsync(async (dto, code, token) =>
                {
                    var heading = await _unitOfWork.CreditWorkRepository.GetCreditWorkHeading(creditWorkId);
                    var exists = await _unitOfWork.CreditWorkRepository.DoesCreditWorkTitleExistAsync(heading, dto.Code, creditWorkId);
                    return !exists;
                })
                .WithMessage(CONST_STRING.PROPERTY_ERROR_DUPLICATE);       
        }
    }
}
