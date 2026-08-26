using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.API;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CourseEnrollment.Requests.Queries;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CourseEnrollmentDTOs;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.CourseEnrollment.Handlers.Queries
{
    public class GetCourseEnrollmentRequestHandler : IRequestHandler<GetCourseEnrollmentRequest, BaseQueryResponse<GetCourseEnrollmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public GetCourseEnrollmentRequestHandler(IUnitOfWork unitOfWork, ICurrentUserService currenrUser, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currenrUser;
            _userService = userService;
        }

        public async Task<BaseQueryResponse<GetCourseEnrollmentDto>> Handle(GetCourseEnrollmentRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryResponse<GetCourseEnrollmentDto>();
            try
            {
                var entity = await _unitOfWork.CourseEnrollmentRepository.GetEnrollment(request.CourseEnrollmentId);
                if (entity is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    response.Message = "No credit works found";
                    return response;
                }

                var staffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(staffId) ?? new StaffDto();

                var dto = new GetCourseEnrollmentDto
                {
                    StudentDto = entity.Student.MapToGetStudentDto(staff),
                    CourseDto = entity.Course.MapToGetCourseDto(staff),
                    EnrolledBy = staff,
                    CreatedBy = staff,
                    CreatedAt = DateTime.UtcNow,
                };

                response.IsSuccessful = true;
                response.Status = System.Net.HttpStatusCode.OK;
                response.Record = dto;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessQueryException(ex);
            }

        }
    }
}
