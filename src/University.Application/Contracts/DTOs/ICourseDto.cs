using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Contracts.DTOs
{
    public interface ICourseDto:IBaseDto
    {
        public string CourseTitle { get; set; }
    }
}
