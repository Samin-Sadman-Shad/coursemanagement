using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Domain.Entities.JunctionEntities;

namespace University.Application.Models.DTOs.StudentDTOs
{
    public class GetStudentWithDetailsDto:GetStudentDto
    {
        public List<GetCourseDto> Courses { get; set; } = new List<GetCourseDto>();
        public List<GetCreditWorkDto> CreditWorks { get; set; } = new List<GetCreditWorkDto>();
    }
}
