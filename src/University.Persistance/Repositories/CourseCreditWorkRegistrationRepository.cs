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
    public class CourseCreditWorkRegistrationRepository
        : ICourseCreditWorkRegistrationRepository
    {
        private readonly UniversityDbContext _dbContext;
        public CourseCreditWorkRegistrationRepository(UniversityDbContext dbConext)
        {
            _dbContext = dbConext;
        }

        public async Task<CourseCreditWork> RegisterCourseToCreditWork(Guid courseId, Guid creditWorkId, Guid staffId)
        {
            var course = await _dbContext.FindAsync<Course>(courseId);
            var creditWork = await _dbContext.FindAsync<CreditWork>(creditWorkId);
            if(creditWork is null || course is null)
            {
                throw new ArgumentException("Either courseId or CrediWorkId is not valid");
            }
            var entity = new CourseCreditWork
            {
                CourseId = courseId,
                CreditWorkId = creditWorkId,
                Course = course,
                CreditWork = creditWork,
                EnrolledById = staffId,
                EnrolledAt = DateTimeOffset.UtcNow,
            };
            await _dbContext.AddAsync(entity); //save changes will be dealt from unit of work
            return entity;
        }

        //public async Task<bool> UnregisterCourseFromCreditWork(Guid courseId, Guid creditWorkId)
        //{
        //    var course = await _dbContext.FindAsync<Course>(courseId);
        //    var creditWork = await _dbContext.FindAsync<CreditWork>(creditWorkId);
        //    if (creditWork is null || course is null)
        //    {
        //        throw new ArgumentException("Either courseId or CrediWorkId is not valid");
        //    }
        //    var entity = _dbContext.Set<CourseCreditWork>()
        //        .Where(cc => cc.CourseId == courseId && cc.CreditWorkId == creditWorkId)
        //        .SingleOrDefault();
            
        //    if(entity is null)
        //    {
        //        throw new ArgumentException("Course is not registered to the credit");
        //    }
        //    var entityId = entity.Id;
        //    _dbContext.Set<CourseCreditWork>().Remove(entity);
        //    return await ExistsAsync(entityId);
        //}

        public async Task<bool> ExistsAsync(Guid id)
        {
            var entity = await _dbContext.Set<CourseCreditWork>()
                .AsNoTracking()
                .AnyAsync(ccw => ccw.Id == id);
            return entity ;
        }

        public async Task<bool> ExistsAsync(Guid courseId, Guid creditWorkId)
        {
            return await _dbContext.CreditWorksInCourses
                .AsNoTracking()
                .AnyAsync(ccw => ccw.CourseId == courseId && ccw.CreditWorkId == creditWorkId);
        }

        //public async Task<CourseCreditWork> UnregisterCourseFromCreditWork(Guid registrationId)
        //{
        //    var courseCreditWork = await GetByIdAsync(registrationId);
        //    if(courseCreditWork == null)
        //    {
        //        throw new ArgumentException("Course credit work mapping not found");
        //    }
        //     _dbContext.CreditWorksInCourses.Remove(courseCreditWork);
        //    return courseCreditWork;
        //}

        public CourseCreditWork UnregisterCourseFromCreditWork(CourseCreditWork courseCreditWork)
        {
            _dbContext.CreditWorksInCourses.Remove(courseCreditWork);
            return courseCreditWork;
        }

        public async Task<CourseCreditWork?> GetByIdAsync(Guid id)
        {
            return await _dbContext.CreditWorksInCourses
                .FirstOrDefaultAsync(ccw => ccw.Id == id);
        }

        public async Task<List<CourseCreditWork>> GetAllAsync()
        {
            return await _dbContext.CreditWorksInCourses
                .Include(register => register.Course)
                .Include(register => register.CreditWork)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
