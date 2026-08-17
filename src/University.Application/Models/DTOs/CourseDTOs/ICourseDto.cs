using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;

namespace University.Application.Models.DTOs.CourseDTOs
{
    public interface ICourseDto:IBaseDto
    {
        public string CourseTitle { get; set; }
    }
}
