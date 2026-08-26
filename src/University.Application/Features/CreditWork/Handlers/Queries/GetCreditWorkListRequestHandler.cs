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
using University.Application.Features.CreditWork.Requests.Queries;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWork.Handlers.Queries
{
    public class GetCreditWorkListRequestHandler :
        IRequestHandler<GetCreditWorkListRequest, BaseQueryListResponse<GetCreditWorkDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public GetCreditWorkListRequestHandler(IUnitOfWork uow, IUserService userService)
        {
            _unitOfWork = uow;
            _userService = userService;
        }
        public async Task<BaseQueryListResponse<GetCreditWorkDto>> Handle(GetCreditWorkListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryListResponse<GetCreditWorkDto>();
            try
            {
                var repository = _unitOfWork.CreditWorkRepository;
                var entities = await repository.GetAllAsync();
                if(entities is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    response.Message = "No credit works found";
                    return response;
                }

                //var currentStaffId = _currentUserService.UserId;
                //var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                //if (staff is null)
                //{
                //    staff = new StaffDto();
                //}

                var dtos = new List<GetCreditWorkDto>();
                foreach (var entity in entities)
                {
                    var staff = await _userService.GetStaffByIdAsync(entity.CreatedById) ?? new StaffDto();
                    var dto = entity.MapToGetCreditWorkDto(staff);
                    dtos.Add(dto);
                }

                //var dtos = entities.Select(e => e.MapToGetCreditWorkDto(staff)).ToList();
                response.IsSuccessful = true;
                response.Records = dtos;
                response.Status = System.Net.HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessQueryException(ex);
            }
        }
    }
}
