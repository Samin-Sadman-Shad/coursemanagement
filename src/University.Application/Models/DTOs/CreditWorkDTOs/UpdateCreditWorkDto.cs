using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;

namespace University.Application.Models.DTOs.CreditWorkDTOs
{
    public class UpdateCreditWorkDto:BaseUpdateDto
    {
        public string? Heading { get; set; }
        public int? Code { get; set; }
        public string? Description { get; set; }
    }
}
