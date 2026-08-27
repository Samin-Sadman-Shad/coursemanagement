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
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.Student.Handlers.Queries
{
    public class GetPeersByStudentIdRequestHandler
        : IRequestHandler<GetPeersByStudentIdRequest, BaseQueryListResponse<GetStudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        private readonly ICurrentUserService _currentUserService;
        public GetPeersByStudentIdRequestHandler(IUnitOfWork uow, IUserService userService, ICurrentUserService currentUser)
        {
            _unitOfWork = uow;
            _userService = userService;
            _currentUserService = currentUser;
        }
        public async Task<BaseQueryListResponse<GetStudentDto>> Handle(GetPeersByStudentIdRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryListResponse<GetStudentDto>();
            try
            {
                var currentStudentId = _currentUserService.UserId;

                var studentRepository = _unitOfWork.StudentRepository;
                var entities = await studentRepository.GetPeersByStudentIdAsync(currentStudentId);

                var dtos = new List<GetStudentDto>();
                foreach (var entity in entities)
                {
                    var staff = await _userService.GetStaffByIdAsync(entity.CreatedById) ?? new StaffDto();
                    var dto = entity.MapToGetStudentDto(staff);
                    dtos.Add(dto);
                }
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.OK;
                response.Records = dtos;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessQueryException(ex);
            }
        }
    }
}
