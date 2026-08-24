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
        private readonly ICourseCreditWorkRegistrationRepository _courseCreditWorkRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CourseCreditWorkRegistrationDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _courseRepository = _unitOfWork.CourseRepository;
            _creditWorkRepository = _unitOfWork.CreditWorkRepository;
            _courseCreditWorkRepository = _unitOfWork.CourseCreditWorkRegistrationRepository;

            RuleFor(x => x.CourseId)
                .NotEmpty()
                .MustAsync(async (courseId, token) => await _courseRepository.ExistsAsync(courseId))
                .WithMessage("Course not found");

            RuleFor(x => x.CreditWorkId)
                .NotEmpty()
                .MustAsync(async (creditWorkId, token) => await _creditWorkRepository.ExistsAsync(creditWorkId))
                .WithMessage("Credit work not found");

            RuleFor(x => x)
                .MustAsync(async (dto, token) =>
                    !await _courseCreditWorkRepository.ExistsAsync(dto.CourseId, dto.CreditWorkId))
                .WithMessage("This credit work is already registered to this course.")
                .WithName(nameof(CourseCreditWorkRegistrationDto));
        }
    }
}
