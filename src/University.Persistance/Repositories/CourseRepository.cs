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

        public async Task<List<Course>> GetCoursesByStudentIdAsync(Guid studentId)
        {
            //var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == studentId);
            //if (student is not null)
            //{
            //    return student.CoursesEnrolled.Select(coursesOfStudent => coursesOfStudent.Course).ToList();
            //}
            //else
            //{
            //    throw new ArgumentNullException($"Student with id {studentId} not found.");
            //}
            return await _dbContext.Courses
                .Where(c=> c.StudentsInCourse.Any(junction => junction.StudentId == studentId)) //if this course is enrolled by student
                .ToListAsync();
        }

        public async Task<List<Course>> GetCoursesByCreditWorkIdAsync(Guid creditWorkId)
        {
            return await _dbContext.Courses
                .Where(c => c.CreditWorksInCourse.Any(junction => junction.CreditWorkId == creditWorkId))
                .ToListAsync();
        }
    }
}
