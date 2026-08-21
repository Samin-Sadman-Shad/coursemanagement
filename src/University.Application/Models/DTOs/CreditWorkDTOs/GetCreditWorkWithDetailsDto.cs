using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CourseDTOs;

namespace University.Application.Models.DTOs.CreditWorkDTOs
{
    public class GetCreditWorkWithDetailsDto:GetCreditWorkDto
    {
        public List<GetCourseDto> Courses { get; set; } = new List<GetCourseDto>();
    }
}
