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
    internal class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        
        public StudentRepository(UniversityDbContext dbContext): base(dbContext) 
        {
           
        }
        public async Task<List<Course>> GetCoursesByStudentIdAsync(Guid studentId)
        {
            var student =  await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == studentId);
            if(student is not null)
            {
                return student.CoursesEnrolled.Select(coursesOfStudent => coursesOfStudent.Course).ToList();
            }
            else
            {
                throw new ArgumentNullException($"Student with id {studentId} not found.");
            }
            
        }

        public async Task<List<CreditWork>> GetCreditWorksByStudentIdAsync(Guid studentId)
        {
            var student =  await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == studentId);
            if (student is not null)
            {
                return student.CreditWorksEnrolled.Select(creditOfStudent => creditOfStudent.CreditWork).ToList();
            }
            else
            {
                throw new ArgumentNullException($"Student with id {studentId} not found.");
            }
        }
        
        //view other students in their classes
        public async Task<List<Student>> GetPeersByStudentIdAsync(Guid studentId)
        {
            var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == studentId);
            if(student is not null)
            {
                var creditWorks = student.CreditWorksEnrolled.Select(creditOfStudent => creditOfStudent.CreditWork).ToList();
                var creditEnrollments  = creditWorks.SelectMany( c=> c.StudentsInCreditWork).ToList();
                return creditEnrollments.Where(c => c.StudentId != studentId).Select(c => c.Student).ToList();
            }
            else
            {
                throw new ArgumentNullException($"Student with id {studentId} not found.");
            }
        }
    }
}
