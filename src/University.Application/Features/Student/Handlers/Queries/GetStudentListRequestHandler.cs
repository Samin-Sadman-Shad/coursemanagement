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
using University.Application.Features.Student.Requests.Queries;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.Student.Handlers.Queries
{
    public class GetStudentListRequestHandler : IRequestHandler<GetStudentListRequest, BaseQueryListResponse<GetStudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        public GetStudentListRequestHandler(IUnitOfWork uow, IUserService userService, ICurrentUserService currentUserService)
        {
            _unitOfWork = uow;
            _userService = userService;
            _currentUserService = currentUserService;
        }
        public async Task<BaseQueryListResponse<GetStudentDto>> Handle(GetStudentListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryListResponse<GetStudentDto>();
            try
            {

                var currentStaffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                if (staff is null)
                {
                    staff = new StaffDto();
                }

                var studentRepository = _unitOfWork.StudentRepository;
                var entities = await studentRepository.GetAllAsync();
                var records = entities.Select(entity => entity.MapToGetStudentDto(staff)).ToList();
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.OK;
                response.Records = records;
                return response;
            }
            catch(Exception ex)
            {
                throw new FailToProcessQueryException(ex);
            }

        }
    }
}
