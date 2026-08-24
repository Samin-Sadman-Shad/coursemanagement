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
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.Course.Handlers.Queries
{
    public class GetCourseListRequestHandler : IRequestHandler<GetCourseListRequest, BaseQueryListResponse<GetCourseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCourseListRequestHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<BaseQueryListResponse<GetCourseDto>> Handle(GetCourseListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryListResponse<GetCourseDto>();
            try
            {
                var repository = _unitOfWork.CourseRepository;
                var entities = await repository.GetAllAsync();
                if (entities is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    response.Message = "No credit works found";
                    return response;
                }
                var dtos = entities.Select(e => e.MapToGetCourseDto()).ToList();
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
