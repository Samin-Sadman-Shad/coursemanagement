using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.API;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CourseEnrollment.Requests.Commands;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CourseEnrollmentDTOs;
using University.Application.Models.DTOs.CourseEnrollmentDTOs.Validators;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;
using University.Domain.Entities.BaseEntities;
using junctionEntities = University.Domain.Entities.JunctionEntities;

namespace University.Application.Features.CourseEnrollment.Handlers.Commands
{
    public class CreateCourseEnrollmentCommandHandler
        : IRequestHandler<CreateCourseEnrollmentCommand, CreateCommandResponse<GetCourseEnrollmentDto>>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        public CreateCourseEnrollmentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currenrUser, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currenrUser;
            _userService = userService;
        }
        public async Task<CreateCommandResponse<GetCourseEnrollmentDto>> Handle(CreateCourseEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateCommandResponse<GetCourseEnrollmentDto>();
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

                var staffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(staffId) ?? new StaffDto();
                var createdAt = DateTimeOffset.UtcNow;

                var enrollment = new junctionEntities.CourseEnrollment
                {
                    StudentId = student.UserId,
                    Student = student,
                    CourseId = course.Id,
                    Course = course,
                    EnrolledAt = createdAt,
                    EnrolledById = staffId,
                    CreatedById = staffId,
                    CreatedAt = createdAt,
                };
                var createdEntity = await _unitOfWork.CourseEnrollmentRepository.CreateCourseEnrollment(enrollment);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = new GetCourseEnrollmentDto
                {
                    StudentDto = student.MapToGetStudentDto(staff),
                    CourseDto = course.MapToGetCourseDto(staff),
                    //EnrolledBy = staff,
                    CreatedBy = staff,
                    CreatedAt = createdAt
                };

                response.IsSuccessful = true;
                response.Status = HttpStatusCode.Created;
                response.RecordId = createdEntity.Id;
                response.Record = responseDto;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }

        }
    }
}
