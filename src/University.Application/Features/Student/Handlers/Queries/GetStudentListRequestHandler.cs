using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Student.Requests.Queries;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.Student.Handlers.Queries
{
    public class GetStudentListRequestHandler : IRequestHandler<GetStudentListRequest, BaseQueryListResponse<GetStudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetStudentListRequestHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<BaseQueryListResponse<GetStudentDto>> Handle(GetStudentListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryListResponse<GetStudentDto>();
            try
            {
                var studentRepository = _unitOfWork.StudentRepository;
                var entities = await studentRepository.GetAllAsync();
                var records = entities.Select(entity => entity.MapToGetStudentDto()).ToList();
                response.IsSuccessful = true;
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
