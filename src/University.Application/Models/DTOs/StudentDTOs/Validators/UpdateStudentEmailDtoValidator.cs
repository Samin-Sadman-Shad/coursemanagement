using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Utils;

namespace University.Application.Models.DTOs.StudentDTOs.Validators
{
    public class UpdateStudentEmailDtoValidator:AbstractValidator<UpdateStudentEmailDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateStudentEmailDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            RuleFor(dto => dto.Email)
                .EmailAddress()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_VALID_EMAIL)
                .MustAsync(async (email, token) =>
                {
                    var emailExist = await _unitOfWork.StudentRepository.DoesEmailExistAsync(email);
                    return !emailExist;
                })
                .WithMessage(CONST_STRING.PROPERTY_ERROR_DUPLICATE)
                .When(dto => !string.IsNullOrWhiteSpace(dto.Email));

        }
    }
}
