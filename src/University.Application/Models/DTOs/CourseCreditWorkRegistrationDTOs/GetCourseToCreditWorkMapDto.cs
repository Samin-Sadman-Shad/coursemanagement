using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs;

namespace University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs
{
    public class GetCourseToCreditWorkMapDto:BaseQueryDto
    {
        public Guid Registrationid {  get; set; }
        public GetCreditWorkDto? CreditWorkDto { get; set; }
        public GetCourseDto? GetCourseDto { get; set; }
    }
}
