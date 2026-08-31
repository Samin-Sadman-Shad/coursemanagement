using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.API;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CreditWork.Requests.Commands;
using University.Application.Models.DTOs.CreditWorkDTOs.Validators;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWork.Handlers.Commands
{
    public class UpdateCreditWorkCodeCommandHandler
        : IRequestHandler<UpdateCreditWorkCodeCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public UpdateCreditWorkCodeCommandHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
        {
            _unitOfWork = uow;
            _currentUserService = currentUserService;
        }
        public async Task<BaseCommandResponse> Handle(UpdateCreditWorkCodeCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse
            {
                RecordId = request.CreditWorkId
            };
            try
            {
                var validator = new UpdateCreditWorkCodeDtoValidator(_unitOfWork, request.CreditWorkId);
                var validationResult = await validator.ValidateAsync(request.CreditWorkDto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;

                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }
                var creditWorkRepository = _unitOfWork.CreditWorkRepository;
                var entity = await creditWorkRepository.GetByIdAsync(request.CreditWorkId, cancellationToken);
                if (entity is null)
                {
                    throw new NotFoundException();
                }

                var currentStaffId = _currentUserService.UserId;
                var updatedAt = DateTimeOffset.UtcNow;
                entity.LastModifiedById = currentStaffId;
                entity.LastModifiedAt = updatedAt;

                await creditWorkRepository.UpdateCreditWorkCode(entity, request.CreditWorkDto.Code);
                await _unitOfWork.SaveChangesAsync();
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.NoContent;

                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }
        }
    }
}
