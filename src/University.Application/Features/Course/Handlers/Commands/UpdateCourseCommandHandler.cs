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
using University.Application.Models.DTOs.CreditWorkDTOs.Validators;
using University.Application.Models.Responses;

namespace University.Application.Features.Course.Handlers.Commands
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseTitleCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCourseCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
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
                var courseRepository = _unitOfWork.CourseRepository;
                var entity = await courseRepository.GetByIdAsync(request.CourseId);
                if (entity is null)
                {
                    throw new NotFoundException();
                }
                request.UpdateCourseTitleDto.UpdateCourse(entity);
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
