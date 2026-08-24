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
using University.Application.Models.DTOs.CreditWorkEnrollmentDto.Validators;
using University.Application.Models.Responses;
using University.Domain.Entities.BaseEntities;
using Entities = University.Domain.Entities.JunctionEntities;


namespace University.Application.Features.CreditWorkEnrollment.Handlers.Commands
{
    public class CreateCreditWorkEnrollmentCommandHandler
        : IRequestHandler<CreateCreditWorkEnrollmentCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCreditWorkEnrollmentCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<BaseCommandResponse> Handle(CreateCreditWorkEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                var validator = new CreateCreditWorkEnrollmentDtoValidator(_unitOfWork);
                var validationResult = await validator.ValidateAsync(request.CreditWorkEnrollmentDto);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return response;
                }
                var creditWork = await _unitOfWork.CreditWorkRepository.GetByIdAsync(request.CreditWorkEnrollmentDto.CreditWorkId);
                var student = await _unitOfWork.StudentRepository.GetByIdAsync(request.CreditWorkEnrollmentDto.StudentId);
                if (student is null || creditWork is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    return response;
                }

                var enrollment = new Entities.CreditWorkEnrollment
                {
                    CreditWorkId = creditWork.Id,
                    CreditWork = creditWork,
                    StudentId = student.UserId,
                    Student = student,
                    EnrolledAt = DateTime.UtcNow,
                    StaffId = request.CreditWorkEnrollmentDto.CreatedBy.UserId,
                    EnrolledBy = request.CreditWorkEnrollmentDto.CreatedBy,
                    CreatedBy = request.CreditWorkEnrollmentDto.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                };
                var createdEntity = await  _unitOfWork.CreditWorkEnrollmentRepository.CreateCreditWorkEnrollment(enrollment);
                await _unitOfWork.SaveChangesAsync();
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.Created;
                response.RecordId = createdEntity.Id;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }
        }
    }
}
