using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWork.Requests.Queries
{
    public class GetCreditWorksByCourseIdRequest: IRequest<BaseQueryListResponse<GetCreditWorkDto>>
    {
        public Guid CourseId { get; set; }
    }
}
