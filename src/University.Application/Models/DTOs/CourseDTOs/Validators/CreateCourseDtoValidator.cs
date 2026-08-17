using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.DTOs.CourseDTOs.Validators
{
    public class CreateCourseDtoValidator:AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator()
        {
            RuleFor(c => c.CourseTitle)
                .NotEmpty()
                .MaximumLength(20)
                .WithMessage("The {PropertyName} can not excced 20 characters")
                .Must(title => title.All(char.IsLetterOrDigit))
                .WithMessage("The {PropertyName} can contains only alphanumeric characters");
        }
    }
}
