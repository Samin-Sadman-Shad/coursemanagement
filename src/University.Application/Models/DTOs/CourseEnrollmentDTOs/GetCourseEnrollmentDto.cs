using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.DTOs.StudentDTOs;

namespace University.Application.Models.DTOs.CourseEnrollmentDTOs
{
    public class GetCourseEnrollmentDto:BaseQueryDto
    {
        public GetStudentDto? StudentDto { get; set; }
        public GetCourseDto? CourseDto { get; set; }

        //public required StaffDto EnrolledBy { get; set; }
    }
}
