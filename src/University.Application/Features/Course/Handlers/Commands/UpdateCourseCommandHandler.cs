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
using University.Application.Features.Course.Requests.Commands;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CourseDTOs.Validators;
using University.Application.Models.DTOs.CreditWorkDTOs.Validators;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.Responses;

namespace University.Application.Features.Course.Handlers.Commands
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseTitleCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        public UpdateCourseCommandHandler(IUnitOfWork uow, IUserService userService, ICurrentUserService currentUserService)
        {
            _unitOfWork = uow;
            _userService = userService;
            _currentUserService = currentUserService;
        }
        public async Task<BaseCommandResponse> Handle(UpdateCourseTitleCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse
            {
                RecordId = request.CourseId
            };
            try
            {
                var validator = new UpdateCourseTitleDtoValidator(_unitOfWork);
                var validationResult = await validator.ValidateAsync(request.UpdateCourseTitleDto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;

                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                var currentStaffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                if (staff is null)
                {
                    staff = new StaffDto();
                }
                var updatedAt = DateTimeOffset.UtcNow;

                var courseRepository = _unitOfWork.CourseRepository;
                var entity = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
                if (entity is null)
                {
                    throw new NotFoundException();
                }
                request.UpdateCourseTitleDto.UpdateCourse(entity, currentStaffId, updatedAt);
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
