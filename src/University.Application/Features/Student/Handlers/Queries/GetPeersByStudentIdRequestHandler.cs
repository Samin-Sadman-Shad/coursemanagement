using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Student.Requests.Queries;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.Student.Handlers.Queries
{
    public class GetPeersByStudentIdRequestHandler
        : IRequestHandler<GetPeersByStudentIdRequest, BaseQueryListResponse<GetStudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPeersByStudentIdRequestHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<BaseQueryListResponse<GetStudentDto>> Handle(GetPeersByStudentIdRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryListResponse<GetStudentDto>();
            try
            {
                var studentRepository = _unitOfWork.StudentRepository;
                var entities = await studentRepository.GetPeersByStudentIdAsync(request.StudentId);
                var records = entities.Select(entity => entity.MapToGetStudentDto()).ToList();
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
