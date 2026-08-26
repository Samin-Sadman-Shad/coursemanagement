using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.API;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CreditWork.Requests.Commands;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs.Validators;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWork.Handlers.Commands
{
    public class CreateCreditWorkCommandHandler : IRequestHandler<CreateCreditWorkCommand, CreateCommandResponse<GetCreditWorkDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        public CreateCreditWorkCommandHandler(IUnitOfWork uow, IUserService userService, ICurrentUserService currentUserService)
        {
            _unitOfWork = uow;
            _userService = userService;
            _currentUserService = currentUserService;
        }
        public async Task<CreateCommandResponse<GetCreditWorkDto>> Handle(CreateCreditWorkCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateCommandResponse<GetCreditWorkDto>();
            try
            {
                var validator = new CreateCreditWorkDtoValidator(_unitOfWork);
                var validationResult = await validator.ValidateAsync(request.CreateCreditWorkDto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return response;
                }

                var currentStaffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                if (staff is null)
                {
                    staff = new StaffDto();
                }
                var createdAt = DateTimeOffset.UtcNow;

                var creditWorkRepository = _unitOfWork.CreditWorkRepository;
                var entity = request.CreateCreditWorkDto.MaptoCreditWork(currentStaffId, createdAt);
                if(entity is null)
                {
                    throw new BadRequestException("can not convert the dto to entity");
                }
                var creditedEntity = await creditWorkRepository.CreateAsync(entity);
                if(creditedEntity is null)
                {
                    throw new FailToProcessCommandException();
                }
                response.IsSuccessful = true;
                response.Status = System.Net.HttpStatusCode.Created;
                response.RecordId = creditedEntity.Id;
                response.Record = creditedEntity.MapToGetCreditWorkDto(staff);
                return response;
            }
            catch(Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }
        }
    }
}
