using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class GetAllCourseEnrollmentRequestHandler
    : IRequestHandler<
        GetAllCourseEnrollmentRequest,
        BaseQueryListResponse<GetCourseEnrollmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public GetAllCourseEnrollmentRequestHandler(
            IUnitOfWork uow,
            IUserService userService)
        {
            _unitOfWork = uow;
            _userService = userService;
        }

        public async Task<BaseQueryListResponse<GetCourseEnrollmentDto>> Handle(
            GetAllCourseEnrollmentRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = new BaseQueryListResponse<GetCourseEnrollmentDto>();

                var entities = await _unitOfWork
                    .CourseEnrollmentRepository
                    .GetAllEnrollmentAsync();

                var dtos = new List<GetCourseEnrollmentDto>();

                foreach (var entity in entities)
                {
                    var staff = await _userService
                        .GetStaffByIdAsync(entity.CreatedById)
                        ?? new StaffDto();

                    var dto = new GetCourseEnrollmentDto
                    {
                        Id = entity.Id,

                        StudentDto = entity.Student?
                            .MapToGetStudentDto(staff),

                        CourseDto = entity.Course?
                            .MapToGetCourseDto(staff),

                        CreatedBy = staff,
                        CreatedAt = entity.CreatedAt
                    };

                    dtos.Add(dto);
                }

                response.IsSuccessful = true;
                response.Status = System.Net.HttpStatusCode.OK;
                response.Records = dtos;

                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }
        }
    }
}
