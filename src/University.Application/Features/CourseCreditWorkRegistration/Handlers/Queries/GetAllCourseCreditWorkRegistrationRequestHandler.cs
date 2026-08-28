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
using University.Application.Features.CourseCreditWorkRegistration.Requests.Queries;
using University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.Responses;

namespace University.Application.Features.CourseCreditWorkRegistration.Handlers.Queries
{
    public class GetAllCourseCreditWorkRegistrationRequestHandler : IRequestHandler<GetAllCourseCreditWorkRegistrationRequest, BaseQueryListResponse<GetCourseToCreditWorkMapDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public GetAllCourseCreditWorkRegistrationRequestHandler(IUnitOfWork uow, IUserService userService)
        {
            _unitOfWork
                = uow;
            _userService = userService;
        }
        public async Task<BaseQueryListResponse<GetCourseToCreditWorkMapDto>> Handle(GetAllCourseCreditWorkRegistrationRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new BaseQueryListResponse<GetCourseToCreditWorkMapDto>();
                var entities = await _unitOfWork.CourseCreditWorkRegistrationRepository.GetAllAsync();
                var dtos = new List<GetCourseToCreditWorkMapDto>();
                foreach (var entity in entities)
                {
                    var staff = await _userService.GetStaffByIdAsync(entity.CreatedById) ?? new StaffDto();
                    var dto = new GetCourseToCreditWorkMapDto
                    {
                        Registrationid = entity.Id,
                        GetCourseDto = entity.Course.MapToGetCourseDto(staff),
                        CreditWorkDto = entity.CreditWork.MapToGetCreditWorkDto(staff),
                        CreatedBy = staff
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
