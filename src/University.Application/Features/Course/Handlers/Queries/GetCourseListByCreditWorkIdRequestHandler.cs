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
    public class GetCourseListByCreditWorkIdRequestHandler
        : IRequestHandler<GetCourseListByCreditWorkIdRequest, BaseQueryListResponse<GetCourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        public GetCourseListByCreditWorkIdRequestHandler(IUnitOfWork uow, IUserService userService, ICurrentUserService currentUserService)
        {
            _unitOfWork = uow;
            _userService = userService;
            _currentUserService = currentUserService;
        }
        public async Task<BaseQueryListResponse<GetCourseDto>> Handle(GetCourseListByCreditWorkIdRequest request, 
            CancellationToken cancellationToken)
        {
            var response = new BaseQueryListResponse<GetCourseDto>();
            try
            {
                var repository = _unitOfWork.CourseRepository;
                var entities = await repository.GetCoursesByCreditWorkIdAsync(request.CreditWorkId);
                if (entities is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    response.Message = "No credit works found";
                    return response;
                }

                var currentStaffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                if (staff is null)
                {
                    staff = new StaffDto();
                }

                var dtos = entities.Select(e => e.MapToGetCourseDto(staff)).ToList();
                response.IsSuccessful = true;
                response.Records = dtos;
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
