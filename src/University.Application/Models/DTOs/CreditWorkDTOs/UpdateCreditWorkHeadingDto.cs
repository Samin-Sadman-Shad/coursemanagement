using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;

namespace University.Application.Models.DTOs.CreditWorkDTOs
{
    public class UpdateCreditWorkHeadingDto : BaseUpdateDto
    {
        public required string Heading { get ; set ; }
    }
}
