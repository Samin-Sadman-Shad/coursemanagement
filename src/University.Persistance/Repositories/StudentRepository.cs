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

        public async Task<List<Student>> GetStudentsByCreditWorkIdAsync(Guid creditWorkId)
        {
            return await _dbContext.Students
                .Where(s => s.CreditWorksEnrolled.Any(junction => junction.CreditWorkId == creditWorkId))
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByCourseIdAsync(Guid courseId)
        {
            //filter students by checking the student-course mapping list of each student
            //and checking if the courseId is present in the mapping list 
            return await _dbContext.Students
                .Where(s => s.CoursesEnrolled.Any(junction => junction.CourseId == courseId))
                .ToListAsync();

        }

        public async Task<Student?> GetStudentByEmailAsync(string email)
        {
            var student = await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Email == email);
            return student;
        }

        public async Task<List<Student>> GetStudentsByNameAsync(string name)
        {
            return await _dbContext.Students
                .Where(s=> s.Name == name)
                .ToListAsync();   
        }

        public async Task<Student?> GetStudentByRollAsync(int rollNo)
        {
            var student = await _dbContext.Students
                .FirstOrDefaultAsync(s => s.Roll == rollNo);
            return student;
        }
    }
}
