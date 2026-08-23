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

        public async override Task<CreditWork?> GetByIdDetailAsync(Guid id)
        {
            return await _dbContext.CreditWorks
                .Where(credit => credit.Id == id)
                .Include(credit => credit.StudentsInCreditWork)
                .Include(credit => credit.CoursesOfCreditWork)
                .SingleOrDefaultAsync();
        }

        public async Task<CreditWork> UpdateCreditWorkCode(CreditWork creditWork, int updatedCode)
        {
            creditWork.Code = updatedCode;
            _dbContext.Attach(creditWork).State = EntityState.Modified;
            return creditWork;
        }

        public async Task<CreditWork> UpdateCreditWorkHeading(CreditWork creditWork, string updatedHeading)
        {
            creditWork.Heading = updatedHeading;
            _dbContext.Attach(creditWork).State = EntityState.Modified;
            return creditWork;
        }

        public async Task<CreditWork> UpdateCreditWorkDescription(CreditWork creditWork, string description)
        {
            creditWork.Description = description;
            _dbContext.Attach(creditWork).State = EntityState.Modified;
            return creditWork;
        }

        public async Task<bool> DoesCreditWorkTitleExistAsync(string heading, int code, Guid? excludeId = null)
        {
            return  await _dbContext.CreditWorks
                .Where(cw => excludeId == null || cw.Id != excludeId)
                .AnyAsync(cw => cw.Heading == heading && cw.Code == code);
        }

        public async Task<string> GetCreditWorkHeading(Guid creditWorkId)
        {
            var cw = await _dbContext.CreditWorks
                .Where(c => c.Id == creditWorkId)
                .SingleAsync();
            return cw.Heading;
        }

        public async Task<string> GetCreditWorkCode(Guid creditWorkId)
        {
            var cw = await _dbContext.CreditWorks
                    .Where(c => c.Id == creditWorkId)
                    .SingleAsync();
            return cw.Code.ToString();
        }
    }
}
