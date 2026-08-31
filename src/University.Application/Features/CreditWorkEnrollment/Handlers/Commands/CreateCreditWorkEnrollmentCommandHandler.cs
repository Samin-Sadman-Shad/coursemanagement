using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.API;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CreditWorkEnrollment.Requests.Commands;
using University.Application.Models.DTOs.CreditWorkEnrollmentDto;
using University.Application.Models.DTOs.CreditWorkEnrollmentDto.Validators;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.Responses;
using University.Domain.Entities.BaseEntities;
using Entities = University.Domain.Entities.JunctionEntities;


namespace University.Application.Features.CreditWorkEnrollment.Handlers.Commands
{
    public class CreateCreditWorkEnrollmentCommandHandler
        : IRequestHandler<CreateCreditWorkEnrollmentCommand, CreateCommandResponse<GetCreditWorkEnrollmentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        public CreateCreditWorkEnrollmentCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currenrUser, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currenrUser;
            _userService = userService;
        }
        public async Task<CreateCommandResponse<GetCreditWorkEnrollmentDto>> Handle(CreateCreditWorkEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateCommandResponse<GetCreditWorkEnrollmentDto>();
            var validator = new CreateCreditWorkEnrollmentDtoValidator(_unitOfWork, cancellationToken);
            var validationResult = await validator.ValidateAsync(request.CreditWorkEnrollmentDto);
            if (!validationResult.IsValid)
            {
                response.IsSuccessful = false;
                response.Status = HttpStatusCode.BadRequest;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                return response;
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var creditWork = await _unitOfWork.CreditWorkRepository.GetByIdAsync(request.CreditWorkEnrollmentDto.CreditWorkId, cancellationToken);
                var student = await _unitOfWork.StudentRepository.GetByIdAsync(request.CreditWorkEnrollmentDto.StudentId, cancellationToken);
                if (student is null || creditWork is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    return response;
                }

                var staffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(staffId) ?? new StaffDto();

                var enrollment = new Entities.CreditWorkEnrollment
                {
                    CreditWorkId = creditWork.Id,
                    CreditWork = creditWork,
                    StudentId = student.UserId,
                    Student = student,
                    EnrolledAt = DateTimeOffset.UtcNow,
                    EnrolledById = staffId,
                    CreatedById = staffId,
                    CreatedAt = DateTimeOffset.UtcNow,
                };
                var createdEntity = await _unitOfWork.CreditWorkEnrollmentRepository.CreateCreditWorkEnrollment(enrollment);

                await _unitOfWork.SaveChangesAsync();
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.Created;
                response.RecordId = createdEntity.Id;
                return response;
            }
            catch 
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
