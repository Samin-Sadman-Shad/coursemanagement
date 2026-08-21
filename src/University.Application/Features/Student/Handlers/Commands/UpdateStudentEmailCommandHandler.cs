using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Student.Requests.Commands;
using University.Application.Models.DTOs.StudentDTOs.Validators;
using University.Application.Models.Responses;

namespace University.Application.Features.Student.Handlers.Commands
{
    public class UpdateStudentEmailCommandHandler : IRequestHandler<UpdateStudentEmailCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateStudentEmailCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseCommandResponse> Handle(UpdateStudentEmailCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse
            {
                RecordId = request.StudentId
            };
            try
            {
                var validator = new UpdateStudentEmailDtoValidator();
                var validationResult = await validator.ValidateAsync(request.StudentEmailDto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;
                    
                    response.Errors = validationResult.Errors.Select(e=> e.ErrorMessage).ToList();
                    return response;
                }
                var studentRepository = _unitOfWork.StudentRepository;
                var entity = await studentRepository.GetByIdAsync(request.StudentId);
                if (entity is null)
                {
                    throw new FailToProcessCommandException();
                }
                entity.Email = request.StudentEmailDto.Email;
                await _unitOfWork.SaveChangesAsync();
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.NoContent;

                return response;
            }
            catch(Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }

        }
    }
}
