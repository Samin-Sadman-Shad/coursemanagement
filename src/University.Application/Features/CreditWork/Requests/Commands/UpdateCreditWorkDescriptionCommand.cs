using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWork.Requests.Commands
{
    public class UpdateCreditWorkDescriptionCommand : IRequest<BaseCommandResponse>
    {
        public Guid CreditWorkId { get; set; }
        public required UpdateCreditWorkDescriptionDto CreditWorkDto { get; set; }
    
    }
}
