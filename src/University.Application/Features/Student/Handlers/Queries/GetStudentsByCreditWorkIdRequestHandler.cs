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
    public class GetStudentsByCreditWorkIdRequestHandler : IRequestHandler<GetStudentsByCreditWorkIdRequest, BaseQueryListResponse<GetStudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public GetStudentsByCreditWorkIdRequestHandler(IUnitOfWork uow, IUserService userService)
        {
            _unitOfWork = uow;
            _userService = userService;
        }
        public async Task<BaseQueryListResponse<GetStudentDto>> Handle(GetStudentsByCreditWorkIdRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryListResponse<GetStudentDto>();
            try
            {
                var studentRepository = _unitOfWork.StudentRepository;
                var entities = await studentRepository.GetStudentsByCreditWorkIdAsync(request.CreditWorkId);

                var records = new List<GetStudentDto>();
                foreach (var entity in entities)
                {
                    var staff = await _userService.GetStaffByIdAsync(entity.CreatedById) ?? new StaffDto();
                    var dto = entity.MapToGetStudentDto(staff);
                    records.Add(dto);
                }
                response.IsSuccessful = true;
                response.Status = HttpStatusCode.OK;
                response.Records = records;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessQueryException(ex);
            }
        }
    }
}
