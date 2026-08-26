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
using University.Application.Contracts.Identity;
using University.Application.Contracts.API;
using University.Application.Models.DTOs.Staff;

namespace University.Application.Features.Student.Handlers.Commands
{
    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, CreateStudentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        public CreateStudentCommandHandler(IUnitOfWork unitOfWork, IUserService userService, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
            _currentUserService = currentUserService;
        }

        public async Task<CreateStudentResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateStudentResponse();

            var studentRepository = _unitOfWork.StudentRepository;
            var dto = request.CreateStudentDto;
            var validator = new CreateStudentDtoValidator(_unitOfWork);
            var validationResult = await validator.ValidateAsync(dto);
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

                var currentStaffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                if (staff is null)
                {
                    staff = new StaffDto();
                }
                var createdAt = DateTimeOffset.UtcNow;

                var entity = dto.MapToStudent(currentStaffId, createdAt);                              
                var (userId, resetToken) = await _userService.CreateStudentAccountAsync(dto.Email, dto.Name);            
                entity.UserId = userId;

                var entityCreated = await studentRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync();
                if (entityCreated is null)
                {
                    throw new FailToProcessCommandException();
                }
                response.IsSuccessful = true;
                response.RecordId = entityCreated.UserId;
                response.Record = entity.MapToGetStudentDto(staff);
                response.Record.CreatedBy = staff;

                response.Status = HttpStatusCode.Created;
                response.PasswordResetToken = resetToken;
                return response;
            }
            catch { await _unitOfWork.RollbackAsync(); throw; }

        }
    }
}
