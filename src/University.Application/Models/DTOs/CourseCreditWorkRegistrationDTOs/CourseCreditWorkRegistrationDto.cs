using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs;

namespace University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs
{
    public class CourseCreditWorkRegistrationDto
    {
        public required GetCourseDto course {  get; set; }
        public required GetCreditWorkDto creditWork { get; set; }
    }
}
