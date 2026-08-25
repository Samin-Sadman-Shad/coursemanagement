using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Contracts.Persistance;
using University.Application.Utils;

namespace University.Application.Models.DTOs.StudentDTOs.Validators
{
    public class IStudentDtoValidator:AbstractValidator<IStudentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public IStudentDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            RuleFor(dto => dto.Name)
                .NotEmpty()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_EMPTY)
                .MaximumLength(50)
                .WithMessage(CONST_STRING.PROPERTY_ERROR_MAX_LENGTH)
                .Must(name => name.All(char.IsLetter))
                .WithMessage(CONST_STRING.PROPERTY_ERROR_LETTERS_ONLY);

            RuleFor(dto => dto.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_VALID_EMAIL)
                .MustAsync(async (email, token) =>
                {
                    var emailExist = await _unitOfWork.StudentRepository.DoesEmailExistAsync(email);
                    return !emailExist;
                })
                .WithMessage(CONST_STRING.PROPERTY_ERROR_DUPLICATE)
                .When(dto => !string.IsNullOrWhiteSpace(dto.Email));

            RuleFor(dto => dto.Roll)
                .NotEmpty()
                .WithMessage(CONST_STRING.PROPERTY_ERROR_EMPTY);
        }
    }
}
