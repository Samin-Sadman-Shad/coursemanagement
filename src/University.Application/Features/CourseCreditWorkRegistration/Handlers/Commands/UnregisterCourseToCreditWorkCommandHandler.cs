using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CourseCreditWorkRegistration.Requests.Commands;
using University.Application.Models.Responses;

namespace University.Application.Features.CourseCreditWorkRegistration.Handlers.Commands
{
    public class UnregisterCourseToCreditWorkCommandHandler
        : IRequestHandler<UnregisterCourseToCreditWorkCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnregisterCourseToCreditWorkCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseCommandResponse> Handle(UnregisterCourseToCreditWorkCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                var courseCreditWork = await _unitOfWork.CourseCreditWorkRegistrationRepository
                             .GetByIdAsync(request.CourseCreditWorkId, cancellationToken);

                if (courseCreditWork is null)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.NotFound;
                    response.RecordId = request.CourseCreditWorkId;
                    return response;
                }

                 _unitOfWork.CourseCreditWorkRegistrationRepository.UnregisterCourseFromCreditWork(courseCreditWork);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                response.IsSuccessful = true;
                response.Status = HttpStatusCode.NoContent;
                response.RecordId = request.CourseCreditWorkId;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }
        }
    }
}
