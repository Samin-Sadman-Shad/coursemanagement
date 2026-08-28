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
using University.Application.Features.CourseCreditWorkRegistration.Requests.Commands;
using University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs.Validators;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.Responses;
using University.Domain.Entities.JunctionEntities;

namespace University.Application.Features.CourseCreditWorkRegistration.Handlers.Commands
{
    public class RegisterCourseToCreditWorkCommandHandler
        : IRequestHandler<RegisterCourseToCreditWorkCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;
        public RegisterCourseToCreditWorkCommandHandler(IUnitOfWork uow,  ICurrentUserService currenrUser, IUserService userService)
        {
            _unitOfWork = uow;
            _currentUserService = currenrUser;
            _userService = userService;
        }
        public async Task<BaseCommandResponse> Handle(RegisterCourseToCreditWorkCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                var validator = new CourseCreditWorkRegistrationDtoValidator(_unitOfWork);
                var validationResult = await validator.ValidateAsync(request.courseCreditWorkDto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return response;
                }

                var staffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(staffId) ?? new StaffDto();
                var createdAt = DateTimeOffset.UtcNow;

                var courseCreditWorkRepository = _unitOfWork.CourseCreditWorkRegistrationRepository;

                var entity = await courseCreditWorkRepository.RegisterCourseToCreditWork(
                    request.courseCreditWorkDto.CourseId, 
                    request.courseCreditWorkDto.CreditWorkId,
                    staffId);
                await _unitOfWork.SaveChangesAsync();
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.Created;
                response.RecordId = entity.Id;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }
        }
    }
}
