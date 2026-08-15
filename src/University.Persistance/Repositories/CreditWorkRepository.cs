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
        public async Task<List<Course>> GetCoursesByCreditWorkIdAsync(Guid creditWorkId)
        {
            return await _dbContext.Courses
                .Where(c => c.CreditWorksInCourse.Any(cw => cw.CreditWorkId == creditWorkId))
                .ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByCreditWorkIdAsync(Guid creditWorkId)
        {
            return await _dbContext.Students
                .Where(s => s.CreditWorksEnrolled.Any(cw => cw.CreditWorkId == creditWorkId))
                .ToListAsync();
        }
    }
}
