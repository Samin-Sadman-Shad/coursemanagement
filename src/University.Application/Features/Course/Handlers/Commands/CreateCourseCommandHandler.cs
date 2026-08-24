using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Course.Requests.Commands;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CourseDTOs.Validators;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs.Validators;
using University.Application.Models.Responses;

namespace University.Application.Features.Course.Handlers.Commands
{
    public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CreateCommandResponse<GetCourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCourseCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
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
                var courseRepository = _unitOfWork.CourseRepository;
                var entity = request.CreateCourseDto.MapToCourse();
                if (entity is null)
                {
                    throw new BadRequestException("can not convert the dto to entity");
                }
                var createdCourse = await courseRepository.CreateAsync(entity);
                if (createdCourse is null)
                {
                    throw new FailToProcessCommandException();
                }
                response.IsSuccessful = true;
                response.Status = System.Net.HttpStatusCode.Created;
                response.RecordId = createdCourse.Id;
                response.Record = createdCourse.MapToGetCourseDto();
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessCommandException(ex);
            }
        }
    }
}
