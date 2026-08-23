using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CreditWorkEnrollmentDto;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWorkEnrollment.Requests.Commands
{
    public class CreateCreditWorkEnrollmentCommand:IRequest<CreateCommandResponse<GetCreditWorkEnrollmentDto>>
    {
        public required CreateCreditWorkEnrollmentDto CreditWorkEnrollmentDto { get; set; }
    }
}
