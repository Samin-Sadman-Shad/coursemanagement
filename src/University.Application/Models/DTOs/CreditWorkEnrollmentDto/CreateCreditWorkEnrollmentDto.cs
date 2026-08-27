using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Models.DTOs.Common;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.StudentDTOs;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CreditWorkEnrollmentDto
{
    public class CreateCreditWorkEnrollmentDto:BaseCreateDto
    {
        //public required GetCreditWorkDto CreditWork { get; set; }
        //public required GetStudentDto Student { get; set; }
        public required Guid CreditWorkId { get; set; }
        public required Guid StudentId { get; set; }
    }
}
