using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.Responses;

namespace University.Application.Features.CreditWork.Requests.Commands
{
    public class DeleteCreditWorkCommand:IRequest<BaseCommandResponse>
    {
        public Guid CreditWorkId { get; set; }
    }
}
