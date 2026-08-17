using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CourseDTOs
{
    public class UpdateCourseTitleDto : BaseUpdateDto, ICourseDto
    {
        public required string CourseTitle { get ; set ; }
    }
}
