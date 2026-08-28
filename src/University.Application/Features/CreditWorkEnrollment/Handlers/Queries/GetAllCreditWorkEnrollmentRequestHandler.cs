using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class GetAllCreditWorkEnrollmentRequestHandler
        : IRequestHandler<
            GetAllCreditWorkEnrollmentRequest,
            BaseQueryListResponse<GetCreditWorkEnrollmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public GetAllCreditWorkEnrollmentRequestHandler(
            IUnitOfWork uow,
            IUserService userService)
        {
            _unitOfWork = uow;
            _userService = userService;
        }

        public async Task<BaseQueryListResponse<GetCreditWorkEnrollmentDto>> Handle(
            GetAllCreditWorkEnrollmentRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = new BaseQueryListResponse<GetCreditWorkEnrollmentDto>();

                var entities = await _unitOfWork
                    .CreditWorkEnrollmentRepository
                    .GetAllEnrollmentAsync();

                var dtos = new List<GetCreditWorkEnrollmentDto>();

                foreach (var entity in entities)
                {
                    var staff = await _userService
                        .GetStaffByIdAsync(entity.CreatedById)
                        ?? new StaffDto();

                    var dto = new GetCreditWorkEnrollmentDto
                    {
                        Id = entity.Id,

                        StudentDto = entity.Student?
                            .MapToGetStudentDto(staff),

                        CreditWorkDto = entity.CreditWork?
                            .MapToGetCreditWorkDto(staff),

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
