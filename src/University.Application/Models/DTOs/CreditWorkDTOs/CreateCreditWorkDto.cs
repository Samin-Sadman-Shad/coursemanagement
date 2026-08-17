using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Models.DTOs.Common;

namespace University.Application.Models.DTOs.CreditWorkDTOs
{
    public class CreateCreditWorkDto : BaseCreateDto, ICreditWorkDto
    {
        public required string Heading { get ; set ; }
        public required int Code { get ; set ; }
        public string? Description { get ; set ; }
    }
}
