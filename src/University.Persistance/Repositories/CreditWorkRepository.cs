using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.JunctionEntities;
using University.Persistance.Context;

namespace University.Persistance.Repositories
{
    internal class CreditWorkRepository : GenericRepository<CreditWork>, ICreditWorkRepository
    {
        public CreditWorkRepository(UniversityDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<List<CreditWork>> GetCreditWorksByStudentIdAsync(Guid studentId)
        {
            //var student = await _dbContext.Students.FirstOrDefaultAsync(s => s.UserId == studentId);
            //if (student is not null)
            //{
            //    return student.CreditWorksEnrolled.Select(creditOfStudent => creditOfStudent.CreditWork).ToList();
            //}
            //else
            //{
            //    throw new ArgumentNullException($"Student with id {studentId} not found.");
            //}
            return await _dbContext.CreditWorks
                .Where(cw => cw.StudentsInCreditWork.Any(junction => junction.StudentId == studentId))
                .ToListAsync();
        }

        public async Task<List<CreditWork>> GetCreditWorksByCourseIdAsync(Guid courseId)
        {
            return await _dbContext.CreditWorks
                .Where(cw => cw.CoursesOfCreditWork.Any(junction => junction.CourseId == courseId))
                .ToListAsync();
        }
    }
}
