using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Application.Exceptions;
using University.Application.Features.CreditWork.Requests.Queries;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWork.Handlers.Queries
{
    public class GetCreditWorkWithDetailsRequestHandler :
        IRequestHandler<GetCreditWorkWithDetailsRequest, BaseQueryResponse<GetCreditWorkWithDetailsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCreditWorkWithDetailsRequestHandler(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<BaseQueryResponse<GetCreditWorkWithDetailsDto>> Handle(GetCreditWorkWithDetailsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseQueryResponse<GetCreditWorkWithDetailsDto> ();
            try
            {
                var repository = _unitOfWork.CreditWorkRepository;
                var entity = await repository.GetByIdDetailAsync(request.CreditWorkId);
                if (entity is null)
                {
                    response.IsSuccessful = false;
                    response.Status = System.Net.HttpStatusCode.NotFound;
                    response.Message = "No credit works found";
                    return response;
                }
                var dto = entity.MapToGetCreditWorkWithDetailsDto();
                response.IsSuccessful = true;
                response.Status = System.Net.HttpStatusCode.OK;
                response.Record = dto;
                return response;
            }
            catch (Exception ex)
            {
                throw new FailToProcessQueryException(ex);
            }
        }
    }
}
