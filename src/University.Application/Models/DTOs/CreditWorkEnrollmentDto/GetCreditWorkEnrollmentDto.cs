using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.StudentDTOs;

namespace University.Application.Models.DTOs.CreditWorkEnrollmentDto
{
    public class GetCreditWorkEnrollmentDto:BaseQueryDto
    {
        public GetStudentDto? StudentDto { get; set; }
        public GetCreditWorkDto? CreditWorkDto { get; set; }
    }
}
