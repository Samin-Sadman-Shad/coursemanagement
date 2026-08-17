using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Models.DTOs.Common;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CourseEnrollmentDTOs
{
    public class CreateCourseEnrollmentDto:BaseCreateDto
    {
        public required Student Student { get; set; }
        public required Course Course { get; set; }
    }
}
