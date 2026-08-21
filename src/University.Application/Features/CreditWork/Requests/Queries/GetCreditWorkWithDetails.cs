using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CreditWorkDTOs;

namespace University.Application.Features.CreditWork.Requests.Queries
{
    public class GetCreditWorkWithDetails:IRequest<GetCreditWorkWithDetailsDto>
    {
        public Guid CreditWorkId { get; set; }
    }
}
