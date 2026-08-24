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
        private readonly IUnitOfWork _unitOfWork;
        public CreateCourseEnrollmentDtoValidator(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            var studentRepo = _unitOfWork.StudentRepository;
            var courseRepo = _unitOfWork.CourseRepository;

            RuleFor(x => x.StudentId)
                .NotEmpty()
                .MustAsync(async (id, token) => await studentRepo.ExistsAsync(id))
                .WithMessage("Student not found to be enrolled");

            RuleFor(x => x.CourseId)
                .NotEmpty()
                .MustAsync(async (id, token) => await courseRepo.ExistsAsync(id))
                .WithMessage("Course not found");
            //RuleFor(enrollment => enrollment.Student)
            //    .NotEmpty()
            //    .MustAsync(async (student, token) =>
            //    {
            //        var studentId = student.Id;
            //        return await _studentRepository.ExistsAsync(studentId);
            //    }).
            //    WithMessage("Student not found to be enrolled");

            //RuleFor(enrollment => enrollment.Course)
            //    .NotEmpty()
            //    .MustAsync(async (course, token) =>
            //    {
            //        var courseId = course.Id;
            //        return await _courseRepository.ExistsAsync(courseId);
            //    })
            //    .WithMessage("Course not found");
        }
    }
}
