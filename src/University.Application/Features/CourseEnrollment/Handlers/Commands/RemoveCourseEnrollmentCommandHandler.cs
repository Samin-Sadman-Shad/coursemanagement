using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CourseEnrollment.Requests.Commands;
using University.Application.Models.Responses;

namespace University.Application.Features.CourseEnrollment.Handlers.Commands
{
    public class RemoveCourseEnrollmentCommandHandler
        :IRequestHandler<RemoveCourseEnrollmentCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RemoveCourseEnrollmentCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<BaseCommandResponse> Handle(RemoveCourseEnrollmentCommand request, CancellationToken ct)
        {
            var response = new BaseCommandResponse();
            try
            {
                var enrollment = await _unitOfWork.CourseEnrollmentRepository
                    .GetEnrollment(request.CourseEnrollmentId);
                if(enrollment is null)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.NotFound;
                    return response;
                }
                 _unitOfWork.CourseEnrollmentRepository.RemoveCourseEnrollment(enrollment);
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
