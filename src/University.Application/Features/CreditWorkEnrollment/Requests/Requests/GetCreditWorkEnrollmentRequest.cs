using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CreditWorkEnrollmentDto;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWorkEnrollment.Requests.Requests
{
    public class GetCreditWorkEnrollmentRequest:IRequest<BaseQueryResponse<GetCreditWorkEnrollmentDto>>
    {
        public Guid CreditWorkEnrollmentId {  get; set; }
    }
}
