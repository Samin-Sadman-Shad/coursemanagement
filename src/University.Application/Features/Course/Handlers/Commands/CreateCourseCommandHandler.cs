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
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs.Validators;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.Responses;

namespace University.Application.Features.Course.Handlers.Commands
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CreateCommandResponse<GetCourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        public CreateCourseCommandHandler(IUnitOfWork uow, IUserService userService, ICurrentUserService currentUserService)
        {
            _unitOfWork = uow;
            _userService = userService;
            _currentUserService = currentUserService;
        }
        public async Task<CreateCommandResponse<GetCourseDto>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var response = new CreateCommandResponse<GetCourseDto>();
            try
            {
                var validator = new CreateCourseDtoValidator(_unitOfWork);
                var validationResult = await validator.ValidateAsync(request.CreateCourseDto, cancellationToken);
                if (!validationResult.IsValid)
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.BadRequest;
                    response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                    return response;
                }

                var currentStaffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                if (staff is null)
                {
                    staff = new StaffDto();
                }
                var createdAt = DateTimeOffset.UtcNow;

                var courseRepository = _unitOfWork.CourseRepository;
                var entity = request.CreateCourseDto.MapToCourse(currentStaffId, createdAt);
                if (entity is null)
                {
                    throw new BadRequestException("can not convert the dto to entity");
                }
                var createdCourse = await courseRepository.CreateAsync(entity);
                if (createdCourse is null)
                {
                    throw new FailToProcessCommandException();
                }
                await _unitOfWork.SaveChangesAsync();
                response.IsSuccessful = true;
                response.Status = System.Net.HttpStatusCode.Created;
                response.RecordId = createdCourse.Id;
                response.Record = createdCourse.MapToGetCourseDto(staff);
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }
        }
    }
}
