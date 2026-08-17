using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Models.DTOs.Common;

namespace University.Application.Models.DTOs.CourseDTOs
{
    public class CreateCourseDto : BaseCreateDto, ICourseDto
    {
        public required string CourseTitle { get ; set ; }
    }
}
