using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;

namespace University.Application.Models.DTOs.CourseEnrollmentDTOs.Validators
{
    public class CreateCourseEnrollmentDtoValidator:AbstractValidator<CreateCourseEnrollmentDto>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        public CreateCourseEnrollmentDtoValidator(IStudentRepository studentRepo, ICourseRepository courseRepo)
        {
            _studentRepository = studentRepo;
            _courseRepository = courseRepo;
            RuleFor(enrollment => enrollment.Student)
                .NotEmpty()
                .MustAsync(async (student, token) =>
                {
                    var studentId = student.Id;
                    return await _studentRepository.ExistsAsync(studentId);
                }).
                WithMessage("Student not found to be enrolled");

            RuleFor(enrollment => enrollment.Course)
                .NotEmpty()
                .MustAsync(async (course, token) =>
                {
                    var courseId = course.Id;
                    return await _courseRepository.ExistsAsync(courseId);
                })
                .WithMessage("Course not found");
        }
    }
}
