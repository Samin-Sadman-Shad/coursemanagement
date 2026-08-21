using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CreditWorkDTOs;

namespace University.Application.Features.CreditWork.Requests.Queries
{
    public class GetCreditWorkListRequest:IRequest<List<GetCreditWorkDto>>
    {
    }
}
