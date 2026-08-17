using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Models.DTOs.Common;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CreditEnrollmentDto
{
    public class CreateCreditWorkEnrollmentDto:BaseCreateDto
    {
        public required CreditWork CreditWork { get; set; }
        public required Student Student { get; set; }

    }
}
