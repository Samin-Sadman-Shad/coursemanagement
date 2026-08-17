using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;
using University.Application.Models.DTOs.CreditWorkDTOs;

namespace University.Application.Models.DTOs.CourseDTOs
{
    public class GetCourseDto : BaseQueryDto, ICourseDto
    {
        public required string CourseTitle { get; set; }
        public List<GetCreditWorkDto> CreditWorks { get; set; } = new List<GetCreditWorkDto>();
    }
}
