using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Course.Requests.Commands;
using University.Application.Models.Responses;

namespace University.Application.Features.Course.Handlers.Commands
{
    public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCourseCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<BaseCommandResponse> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse
            {
                RecordId = request.CourseId
            };
            try
            {
                var courseRepository = _unitOfWork.CourseRepository;
                var entity = await courseRepository.GetByIdAsync(request.CourseId);
                if (entity is null)
                {
                    throw new FailToProcessCommandException();
                }
                await courseRepository.DeleteAsync(request.CourseId);
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
