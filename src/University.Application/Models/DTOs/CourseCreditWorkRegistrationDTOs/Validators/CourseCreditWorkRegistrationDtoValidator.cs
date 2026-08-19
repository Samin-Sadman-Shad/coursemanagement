using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs;

namespace University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs.Validators
{
    public class CourseCreditWorkRegistrationDtoValidator:AbstractValidator<CourseCreditWorkRegistrationDto>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICreditWorkRepository _creditWorkRepository;

        public CourseCreditWorkRegistrationDtoValidator(ICourseRepository courseRepository, ICreditWorkRepository creditWorkRepository)
        {
            _courseRepository = courseRepository;
            _creditWorkRepository = creditWorkRepository;

            RuleFor(registration => registration.course)
                .NotEmpty()
                .MustAsync(async (dto, token) =>
                {
                    var courseId = dto.Id;
                    return await _courseRepository.ExistsAsync(courseId);
                })
                .WithMessage("Course not found");

            RuleFor(registration => registration.creditWork)
                .NotEmpty()
                .MustAsync(async (dto, token) =>
                {
                    var creditWorkId = dto.Id;
                    return await _creditWorkRepository.ExistsAsync(creditWorkId);
                })
                .WithMessage("Credit work not found");
        }
    }
}
