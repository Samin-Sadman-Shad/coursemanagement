using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Services
{
    public class CourseService
    {
        public async Task<Course> CreateCourseAsync(Course course)
        {
            // Implementation for creating a course
            throw new NotImplementedException();
        }

        public async Task<Course> GetCourseByIdAsync(Guid courseId)
        {
            // Implementation for retrieving a course by ID
            throw new NotImplementedException();
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            // Implementation for updating a course
            throw new NotImplementedException();
        }

        public async Task DeleteCourseAsync(Guid courseId) 
        {
            throw new NotImplementedException();
        }

    }
}
