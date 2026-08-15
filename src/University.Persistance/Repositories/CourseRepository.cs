using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Domain.Entities.BaseEntities;
using University.Persistance.Context;

namespace University.Persistance.Repositories
{
    internal class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(UniversityDbContext dbContext): base(dbContext) 
        {
            
        }
        public async Task<List<CreditWork>> GetCreditWorksByCourseIdAsync(Guid courseId)
        {
            return await _dbContext.CreditWorks
                .Where(cw => cw.CoursesOfCreditWork.Any(creditWork => creditWork.CourseId == courseId))
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByCourseIdAsync(Guid courseId)
        {
            return await _dbContext.Students
                .Where(s => s.CoursesEnrolled.Any(enrollment => enrollment.CourseId == courseId))
                .ToListAsync();

            _dbContext.Courses.Include(c => c.StudentsInCourse)
                .ThenInclude(se => se.Student)
                .Where(c => c.Id == courseId)
                .SelectMany(c => c.StudentsEnrolled.Select(se => se.Student))
                .ToListAsync();
        }
    }
}
