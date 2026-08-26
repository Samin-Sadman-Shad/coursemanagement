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
    public class GetStudentWithDetailsRequestHandler : IRequestHandler<GetStudentWithDetailsRequest, BaseQueryResponse<GetStudentWithDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        public GetStudentWithDetailsRequestHandler(IUnitOfWork uow, IUserService userService, ICurrentUserService currentUserService)
        {
            _unitOfWork = uow;
            _userService = userService;
            _currentUserService = currentUserService;
        }
        public async Task<BaseQueryResponse<GetStudentWithDetailsDto>> 
            Handle(GetStudentWithDetailsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryResponse<GetStudentWithDetailsDto>();
            try
            {

                var currentStaffId = _currentUserService.UserId;
                var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                if (staff is null)
                {
                    staff = new StaffDto();
                }

                var studentRepository = _unitOfWork.StudentRepository;
                var student = await studentRepository.GetByIdDetailAsync(request.StudentId);
                if(student is not null)
                {
                    response.Status = HttpStatusCode.Accepted;
                    var record = student.MapToGetStudentWithDetailsDto(staff);
                    response.IsSuccessful = true;
                    response.Record = record;
                    response.Status = HttpStatusCode.OK;
                    return response;
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Status = HttpStatusCode.NotFound;
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new FailToProcessQueryException(ex);
            }
            
        }
    }
}
