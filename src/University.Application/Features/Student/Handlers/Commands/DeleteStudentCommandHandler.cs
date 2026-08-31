using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Student.Requests.Commands;
using University.Application.Models.DTOs.StudentDTOs.Validators;
using University.Application.Models.Responses;

namespace University.Application.Features.Student.Handlers.Commands
{
    public class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteStudentCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<BaseCommandResponse> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse
            {
                RecordId = request.StudentId
            };
            try
            {
                var studentRepository = _unitOfWork.StudentRepository;
                var entity = await studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
                if (entity is null)
                {
                    throw new FailToProcessCommandException();
                }
                await studentRepository.DeleteAsync(request.StudentId);
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
