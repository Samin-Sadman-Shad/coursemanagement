using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.Course.Requests.Queries;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.Course.Handlers.Queries
{
    public class GetCourseWithDetailsRequestHandler
        : IRequestHandler<GetCourseWithDetailsRequest, BaseQueryResponse<GetCourseWithDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCourseWithDetailsRequestHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<BaseQueryResponse<GetCourseWithDetailsDto>> Handle(GetCourseWithDetailsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryResponse<GetCourseWithDetailsDto>();
            try
            {
                var repository = _unitOfWork.CourseRepository;
                var entity = await repository.GetByIdAsync(request.CourseId);
                if (entity is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    response.Message = "No credit works found";
                    return response;
                }
                var dto = entity.MapToGetCourseWithDetailsDto();
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
