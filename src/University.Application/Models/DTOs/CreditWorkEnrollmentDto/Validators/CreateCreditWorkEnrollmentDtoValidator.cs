using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;

namespace University.Application.Models.DTOs.CreditWorkEnrollmentDto.Validators
{
    public class CreateCreditWorkEnrollmentDtoValidator:AbstractValidator<CreateCreditWorkEnrollmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCreditWorkEnrollmentDtoValidator(IUnitOfWork uow, CancellationToken token)
        {
            _unitOfWork = uow;
            var _creditWorkRepository = _unitOfWork.CreditWorkRepository;
            var _studentRepository = _unitOfWork.StudentRepository;

            //RuleFor(enrollment => enrollment.CreditWork)
            //    .NotEmpty()
            //    .MustAsync(async (creditWork, token) =>
            //    {
            //        var creditWorkId = creditWork.Id;
            //        return await _creditWorkRepository.ExistsAsync(creditWorkId);
            //    })
            //    .WithMessage("CreditWork not found");

            //RuleFor(enrollment => enrollment.Student)
            //    .NotEmpty()
            //    .MustAsync(async (student, token) =>
            //    {
            //        var studentId = student.Id;
            //        return await _studentRepository.ExistsAsync(studentId);
            //    })
            //    .WithMessage("Student not found");

            RuleFor(dto => dto.CreditWorkId)
                .NotEmpty()
                .MustAsync(async (id, token) => await _creditWorkRepository.ExistsAsync(id, token))
                .WithMessage("CreditWork not found");

            RuleFor(dto => dto.StudentId)
                .NotEmpty()
                .MustAsync(async (id, token) => await _studentRepository.ExistsAsync(id, token))
                .WithMessage("Student not found");
            RuleFor(dto => dto)
                .MustAsync(async (dto, token) =>
                {
                    var exist = await _unitOfWork.CreditWorkEnrollmentRepository
                    .ExistsAsync(dto.StudentId, dto.CreditWorkId);
                    return !exist;
                })
                .WithMessage("Student is already enrolled in this credit work.");
        }

    }
}
