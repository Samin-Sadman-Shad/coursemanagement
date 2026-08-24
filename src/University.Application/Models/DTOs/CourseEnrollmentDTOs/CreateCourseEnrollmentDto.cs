using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Models.DTOs.Common;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.StudentDTOs;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CourseEnrollmentDTOs
{
    public class CreateCourseEnrollmentDto:BaseCreateDto
    {
        //public required GetStudentDto Student { get; set; }
        //public required GetCourseWithDetailsDto Course { get; set; }
        public required Guid StudentId { get; set; }
        public required Guid CourseId { get; set; }
    }
}
