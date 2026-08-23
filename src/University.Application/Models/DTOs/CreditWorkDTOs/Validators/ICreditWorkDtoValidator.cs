using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Contracts.Persistance;
using University.Application.Utils;

namespace University.Application.Models.DTOs.CreditWorkDTOs.Validators
{
    public class ICreditWorkDtoValidator:AbstractValidator<ICreditWorkDto>
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public ICreditWorkDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            RuleFor(cw => cw.Heading)
                .NotEmpty()
                .MaximumLength(10)
                .WithMessage(CONST_STRING.PROPERTY_ERROR_MAX_LENGTH)
                .Must(heading => heading.All(char.IsLetter))
                .WithMessage(CONST_STRING.PROPERTY_ERROR_LETTERS_ONLY);

            RuleFor(cw => cw.Code)
                .NotEmpty()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_EMPTY)
                .MustAsync(async (dto, code, cancellation) =>
                    {
                        var exist = await _unitOfWork.CreditWorkRepository.DoesCreditWorkTitleExistAsync(dto.Heading, code);
                        return !exist;
                    }
                )
            .WithMessage(CONST_STRING.PROPERTY_ERROR_DUPLICATE);

            RuleFor(cw => cw.Description)
                .MaximumLength(100)
                .WithMessage(CONST_STRING.PROPERTY_ERROR_MAX_LENGTH);
        }
    }
}
