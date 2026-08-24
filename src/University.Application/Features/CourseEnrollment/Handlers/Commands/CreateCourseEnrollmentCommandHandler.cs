using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CourseEnrollment.Requests.Commands;
using University.Application.Models.DTOs.CourseEnrollmentDTOs.Validators;
using University.Application.Models.Responses;
using junctionEntities = University.Domain.Entities.JunctionEntities;

namespace University.Application.Features.CourseEnrollment.Handlers.Commands
{
    public class CreateCourseEnrollmentCommandHandler
        : IRequestHandler<CreateCourseEnrollmentCommand, BaseCommandResponse>
    {

        private readonly IUnitOfWork _unitOfWork;
        public CreateCourseEnrollmentCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<BaseCommandResponse> Handle(CreateCourseEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                var dto = request.CourseEnrollmentDto;

                var student = await _unitOfWork.StudentRepository.GetByIdAsync(dto.StudentId);
                var course = await _unitOfWork.CourseRepository.GetByIdAsync(dto.CourseId);

                if (student is null || course is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    return response;
                }
                var validator = new CreateCourseEnrollmentDtoValidator(_unitOfWork);
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return response;
                }
                var enrollment = new junctionEntities.CourseEnrollment
                {
                    StudentId = student.UserId,
                    Student = student,
                    CourseId = course.Id,
                    Course = course,
                    EnrolledAt = DateTime.UtcNow,
                    StaffId = dto.CreatedBy.UserId,
                    EnrolledBy = dto.CreatedBy,
                    CreatedBy = dto.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                };
                var createdEntity = await _unitOfWork.CourseEnrollmentRepository.CreateCourseEnrollment(enrollment);
                await _unitOfWork.SaveChangesAsync();
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.Created;
                response.RecordId = createdEntity.Id;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }

        }
    }
}
