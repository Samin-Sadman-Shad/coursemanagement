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
        private readonly ICourseRepository _courseRepository;
        private readonly ICreditWorkRepository _creditWorkRepository;
        private readonly ICourseCreditWorkRegistrationRepository _courseCreditRegistrationRepository;
        public CourseCreditWorkUnregistrationDtoValidator(ICourseRepository courseRepository, 
            ICreditWorkRepository creditWorkRepository,
            ICourseCreditWorkRegistrationRepository courseCreditRegistrationRepository)
        {
            _courseRepository = courseRepository;
            _creditWorkRepository = creditWorkRepository;
            _courseCreditRegistrationRepository = courseCreditRegistrationRepository;

            Include(new CourseCreditWorkRegistrationDtoValidator(_courseRepository, _creditWorkRepository));

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
