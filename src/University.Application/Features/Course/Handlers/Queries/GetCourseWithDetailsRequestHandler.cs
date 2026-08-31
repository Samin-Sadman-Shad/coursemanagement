using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.API;
using University.Application.Contracts.Identity;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Course.Requests.Queries;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.Responses;

namespace University.Application.Features.Course.Handlers.Queries
{
    public class GetCourseWithDetailsRequestHandler
        : IRequestHandler<GetCourseWithDetailsRequest, BaseQueryResponse<GetCourseWithDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public GetCourseWithDetailsRequestHandler(IUnitOfWork uow, IUserService userService)
        {
            _unitOfWork = uow;
            _userService = userService;
        }
        public async Task<BaseQueryResponse<GetCourseWithDetailsDto>> Handle(GetCourseWithDetailsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryResponse<GetCourseWithDetailsDto>();
            try
            {
                var repository = _unitOfWork.CourseRepository;
                var entity = await repository.GetByIdAsync(request.CourseId, cancellationToken);
                if (entity is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    response.Message = "No credit works found";
                    return response;
                }

                //var currentStaffId = _currentUserService.UserId;
                //var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                //if (staff is null)
                //{
                //    staff = new StaffDto();
                //}

                var staff = await _userService.GetStaffByIdAsync(entity!.CreatedById) ?? new StaffDto();

                var dto = entity.MapToGetCourseWithDetailsDto(staff);
                response.IsSuccessful = true;
                response.Record = dto;
                response.Status = System.Net.HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessQueryException(ex);
            }
        }
    }
}
