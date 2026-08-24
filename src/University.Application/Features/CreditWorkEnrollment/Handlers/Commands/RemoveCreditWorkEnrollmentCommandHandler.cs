using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CreditWorkEnrollment.Requests.Commands;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWorkEnrollment.Handlers.Commands
{
    public class RemoveCreditWorkEnrollmentCommandHandler
        : IRequestHandler<RemoveCreditWorkEnrollmentCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RemoveCreditWorkEnrollmentCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<BaseCommandResponse> Handle(RemoveCreditWorkEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                var enrollment = await _unitOfWork.CreditWorkEnrollmentRepository
                        .GetEnrollment(request.CreditWorkEnrollmentId);
                if(enrollment is null)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.NotFound;
                    return response;
                }
                _unitOfWork.CreditWorkEnrollmentRepository.RemoveCreditWorkEnrollment(enrollment);
                await _unitOfWork.SaveChangesAsync();
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.NoContent;
                response.RecordId = enrollment.Id;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }
        }
    }
}
