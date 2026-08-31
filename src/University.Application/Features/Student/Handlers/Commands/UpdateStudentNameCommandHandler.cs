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
using University.Application.Features.Student.Requests.Commands;
using University.Application.Models.DTOs.StudentDTOs.Validators;
using University.Application.Models.Responses;

namespace University.Application.Features.Student.Handlers.Commands
{
    public class UpdateStudentNameCommandHandler : IRequestHandler<UpdateStudentNameCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public UpdateStudentNameCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<BaseCommandResponse> Handle(UpdateStudentNameCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse
            {
                RecordId = request.StudentId
            };
            try
            {
                var validator = new UpdateStudentNameDtoValidator();
                var validationResult = await validator.ValidateAsync(request.StudentNameDto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;

                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }
                var studentRepository = _unitOfWork.StudentRepository;
                var entity = await studentRepository.GetByIdAsync(request.StudentId, cancellationToken);
                if (entity is null)
                {
                    throw new FailToProcessCommandException();
                }

                entity.Name = request.StudentNameDto.Name;

                var currentStaffId = _currentUserService.UserId;
                var updatedAt = DateTimeOffset.UtcNow;
                entity.LastModifiedById = currentStaffId;
                entity.LastModifiedAt = updatedAt;

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
