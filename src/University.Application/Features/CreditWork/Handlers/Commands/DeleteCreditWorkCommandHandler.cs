using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CreditWork.Requests.Commands;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWork.Handlers.Commands
{
    public class DeleteCreditWorkCommandHandler : IRequestHandler<DeleteCreditWorkCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCreditWorkCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<BaseCommandResponse> Handle(DeleteCreditWorkCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse
            {
                RecordId = request.CreditWorkId
            };
            try
            {
                var creditWorkRepository = _unitOfWork.CreditWorkRepository;
                var entity = await creditWorkRepository.GetByIdAsync(request.CreditWorkId, cancellationToken);
                if (entity is null)
                {
                    throw new FailToProcessCommandException();
                }
                await creditWorkRepository.DeleteAsync(request.CreditWorkId);
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
