using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Utils;

namespace University.Application.Models.DTOs.CourseDTOs.Validators
{
    internal class ICourseDtoValidator:AbstractValidator<ICourseDto>
    {
        public ICourseDtoValidator()
        {
            RuleFor(c => c.CourseTitle)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("The {PropertyName} can not excced 20 characters")
                .Must(title => title.All(char.IsLetterOrDigit))
                .WithMessage(CONST_STRING.PROPERTY_ERROR_ALPHA_NUMERIC_ONLY);
        }
    }
}
