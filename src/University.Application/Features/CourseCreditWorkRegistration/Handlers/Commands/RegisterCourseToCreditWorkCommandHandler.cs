using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CourseCreditWorkRegistration.Requests.Commands;
using University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs.Validators;
using University.Application.Models.Responses;
using University.Domain.Entities.JunctionEntities;

namespace University.Application.Features.CourseCreditWorkRegistration.Handlers.Commands
{
    public class RegisterCourseToCreditWorkCommandHandler
        : IRequestHandler<RegisterCourseToCreditWorkCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegisterCourseToCreditWorkCommandHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
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
                var courseCreditWorkRepository = _unitOfWork.CourseCreditWorkRegistrationRepository;

                //var courseRepository = _unitOfWork.CourseRepository;
                //var creditWorkRepository = _unitOfWork.CreditWorkRepository;

                //var course = await courseRepository.GetByIdAsync(request.courseCreditWorkDto.CourseId)
                //        ?? throw new NotFoundException(nameof(Course));

                //var creditWork = await creditWorkRepository.GetByIdAsync(request.courseCreditWorkDto.CreditWorkId)
                //    ?? throw new NotFoundException(nameof(CreditWork));

                //var exists = await courseCreditWorkRepository
                //    .ExistsAsync(request.courseCreditWorkDto.CourseId, request.courseCreditWorkDto.CreditWorkId);
                //if (exists)
                //    throw new ConflictException("CreditWork already registered to this Course.");

                //var junction = new CourseCreditWork
                //{
                //    CourseId = course.Id,
                //    Course = course,
                //    CreditWorkId = creditWork.Id,
                //    CreditWork = creditWork
                //};

                var entity = await courseCreditWorkRepository.RegisterCourseToCreditWork(
                    request.courseCreditWorkDto.CourseId, 
                    request.courseCreditWorkDto.CreditWorkId);
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
