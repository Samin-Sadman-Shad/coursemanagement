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
using Entities =  University.Domain.Entities.BaseEntities ;

namespace University.Application.Features.Student.Handlers.Queries
{
    public class GetStudentByPersonalInfoRequestHandler
        : IRequestHandler<GetStudentByPersonalInfoRequest, BaseQueryResponse<GetStudentWithDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public GetStudentByPersonalInfoRequestHandler(IUnitOfWork uow, IUserService userService)
        {
            _unitOfWork = uow;
            _userService = userService;
        }
        public async Task<BaseQueryResponse<GetStudentWithDetailsDto>> Handle(GetStudentByPersonalInfoRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryResponse<GetStudentWithDetailsDto>();
            try
            {
                var studentRepository = _unitOfWork.StudentRepository;
                Entities.Student? student = null;
                if(request.Email != null)
                {
                    student = await studentRepository.GetStudentByEmailAsync(request.Email);
                }
                else if(request.Roll != null)
                {
                    student = await studentRepository.GetStudentByRollAsync(request.Roll);
                }

                //var currentStaffId = _currentUserService.UserId;
                //var staff = await _userService.GetStaffByIdAsync(currentStaffId);
                //if (staff is null)
                //{
                //    staff = new StaffDto();
                //}
                var staff = await _userService.GetStaffByIdAsync(student!.CreatedById) ?? new StaffDto();

                if (student is not null)
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
