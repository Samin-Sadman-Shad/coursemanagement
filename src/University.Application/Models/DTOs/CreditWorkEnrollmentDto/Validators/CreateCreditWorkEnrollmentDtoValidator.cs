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
        private readonly ICreditWorkRepository _creditWorkRepository;
        private readonly IStudentRepository _studentRepository;

        public CreateCreditWorkEnrollmentDtoValidator(ICreditWorkRepository cwRepo, IStudentRepository stRepo)
        {
            _creditWorkRepository = cwRepo;
            _studentRepository = stRepo;

            RuleFor(enrollment => enrollment.CreditWork)
                .NotEmpty()
                .MustAsync(async (creditWork, token) =>
                {
                    var creditWorkId = creditWork.Id;
                    return await _creditWorkRepository.ExistsAsync(creditWorkId);
                })
                .WithMessage("CreditWork not found");

            RuleFor(enrollment => enrollment.Student)
                .NotEmpty()
                .MustAsync(async (student, token) =>
                {
                    var studentId = student.Id;
                    return await _studentRepository.ExistsAsync(studentId);
                })
                .WithMessage("Student not found");
        }

    }
}
