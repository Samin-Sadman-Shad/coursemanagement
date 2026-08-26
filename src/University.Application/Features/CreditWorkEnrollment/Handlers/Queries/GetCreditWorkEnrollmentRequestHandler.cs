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
using University.Application.Features.CreditWorkEnrollment.Requests.Requests;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.CreditWorkEnrollmentDto;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWorkEnrollment.Handlers.Queries
{
    public class GetCreditWorkEnrollmentRequestHandler
        : IRequestHandler<GetCreditWorkEnrollmentRequest, BaseQueryResponse<GetCreditWorkEnrollmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public GetCreditWorkEnrollmentRequestHandler(IUnitOfWork unitOfWork, ICurrentUserService currenrUser, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currenrUser;
            _userService = userService;
        }

        public async Task<BaseQueryResponse<GetCreditWorkEnrollmentDto>> Handle(GetCreditWorkEnrollmentRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryResponse<GetCreditWorkEnrollmentDto>();
            try
            {
                var entity = await _unitOfWork.CreditWorkEnrollmentRepository.GetEnrollment(request.CreditWorkEnrollmentId);
                if (entity is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    response.Message = "No credit works found";
                    return response;
                }

                var staffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(staffId) ?? new StaffDto();
                var createdAt = DateTimeOffset.UtcNow;

                var dto = new GetCreditWorkEnrollmentDto
                {
                    CreditWorkDto = entity.CreditWork.MapToGetCreditWorkDto(staff),
                    StudentDto = entity.Student.MapToGetStudentDto(staff),
                    CreatedBy = staff,
                    CreatedAt = createdAt,
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
