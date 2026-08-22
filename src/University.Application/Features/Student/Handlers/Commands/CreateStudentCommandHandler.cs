using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using University.Application.Contracts.Persistance;
using University.Application.Features.Student.Requests.Commands;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.DTOs.StudentDTOs.Validators;
using University.Application.Models.Responses;
using University.Application.Exceptions;

namespace University.Application.Features.Student.Handlers.Commands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, CreateCommandResponse<GetStudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateStudentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateCommandResponse<GetStudentDto>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateCommandResponse<GetStudentDto>();
            try
            {
                var studentRepository = _unitOfWork.StudentRepository;
                var dto = request.CreateStudentDto;
                var validator = new CreateStudentDtoValidator();
                var validationResult = await validator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return response;
                }
                var entity = dto.MapToStudent();
                var entityCreated = await studentRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                if (entityCreated is null)
                {
                    throw new FailToProcessCommandException();
                }
                response.IsSuccessful = true;
                response.RecordId = entityCreated.UserId;
                response.Record = entity.MapToGetStudentDto();
                response.Status = HttpStatusCode.Created;
                return response;
            }
            catch(Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }


        }
    }
}
