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
    public class UpdateCreditWorkHeadingDtoValidator:AbstractValidator<UpdateCreditWorkHeadingDto>
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public UpdateCreditWorkHeadingDtoValidator(IUnitOfWork uow, Guid creditWorkId)
        {
            _unitOfWork = uow;
            RuleFor(cw => cw.Heading)
               .NotEmpty()
               .MaximumLength(10)
               .WithMessage(CONST_STRING.PROPERTY_ERROR_MAX_LENGTH)
               .Must(heading => heading.All(char.IsLetter))
               .WithMessage(CONST_STRING.PROPERTY_ERROR_LETTERS_ONLY)
               .MustAsync(async (dto, heading, cancellation) =>
               {
                   var codeStr = await _unitOfWork.CreditWorkRepository.GetCreditWorkCode(creditWorkId);
                   var exists = await _unitOfWork.CreditWorkRepository
                       .DoesCreditWorkTitleExistAsync(heading, int.Parse(codeStr), creditWorkId);
                   return !exists;
               })
           .WithMessage(CONST_STRING.PROPERTY_ERROR_DUPLICATE);
        }
    }
}
