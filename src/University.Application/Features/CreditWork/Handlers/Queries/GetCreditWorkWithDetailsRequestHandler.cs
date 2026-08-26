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
using University.Application.Models.Responses;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Features.CreditWork.Handlers.Queries
{
    public class GetCreditWorkWithDetailsRequestHandler :
        IRequestHandler<GetCreditWorkWithDetailsRequest, BaseQueryResponse<GetCreditWorkWithDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public GetCreditWorkWithDetailsRequestHandler(IUnitOfWork uow, IUserService userService)
        {
            _unitOfWork = uow;
            _userService = userService;
        }
        public async Task<BaseQueryResponse<GetCreditWorkWithDetailsDto>> Handle(GetCreditWorkWithDetailsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryResponse<GetCreditWorkWithDetailsDto> ();
            try
            {
                var repository = _unitOfWork.CreditWorkRepository;
                var entity = await repository.GetByIdDetailAsync(request.CreditWorkId);
                if (entity is null)
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

                var staff = await _userService.GetStaffByIdAsync(entity!.CreatedById) ?? new StaffDto();
                var dto = entity.MapToGetCreditWorkWithDetailsDto(staff);

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
