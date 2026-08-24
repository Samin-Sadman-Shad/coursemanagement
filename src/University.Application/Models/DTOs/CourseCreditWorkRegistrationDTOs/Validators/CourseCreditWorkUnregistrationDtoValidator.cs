using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;

namespace University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs.Validators
{
    public class CourseCreditWorkUnregistrationDtoValidator:AbstractValidator<CourseCreditWorkUnregistrationDto>
    {
        private readonly ICourseCreditWorkRegistrationRepository _courseCreditRegistrationRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CourseCreditWorkUnregistrationDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _courseCreditRegistrationRepository = _unitOfWork.CourseCreditWorkRegistrationRepository;

            RuleFor(unregister => unregister.RegistrationId)
                .NotEmpty()
                .MustAsync(async (id, token) =>
                {
                    return await _courseCreditRegistrationRepository.ExistsAsync(id);
                })
                .WithMessage("Course is not registered to the Credit work");
            
        }
    }
}
