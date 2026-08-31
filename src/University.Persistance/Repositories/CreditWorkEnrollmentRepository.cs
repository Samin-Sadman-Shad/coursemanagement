using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Domain.Entities.JunctionEntities;
using University.Persistance.Context;

namespace University.Persistance.Repositories
{
    internal class CreditWorkEnrollmentRepository : ICreditWorkEnrollmentRepository
    {
        private readonly UniversityDbContext _dbContext;
        public CreditWorkEnrollmentRepository(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<CreditWorkEnrollment> CreateCreditWorkEnrollment(CreditWorkEnrollment enrollment, CancellationToken cancellationToken = default)
        {
             await _dbContext.AddAsync<CreditWorkEnrollment>(enrollment, cancellationToken);
            return enrollment;
        }

        public async Task<CreditWorkEnrollment?> GetEnrollment(Guid enrollmentId, CancellationToken cancellationToken = default)
        {
            //return await _dbContext.FindAsync<CreditWorkEnrollment>(enrollmentId);
            return await _dbContext.CreditWorkEnrollments
                .Include(enroll => enroll.CreditWork)
                .Include(enroll => enroll.Student)
                .FirstOrDefaultAsync(enroll => enroll.Id == enrollmentId, cancellationToken);
        }

        public async Task<List<CreditWorkEnrollment>> GetAllEnrollmentAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.CreditWorkEnrollments
                .Include(enrollment => enrollment.Student)
                .Include(enrollment => enrollment.CreditWork)
                .AsNoTracking()
                .ToListAsync(cancellationToken); 
        }

        public async Task<bool> DoesEnrollmentExist(Guid enrollmentId)
        {
            var entity = await GetEnrollment(enrollmentId);
            return entity is not null;
        }

        //public async Task<bool> RemoveCreditWorkEnrollment(Guid enrollmentId)
        //{
        //    var entity = await GetEnrollment(enrollmentId);
        //    if(entity is not null)
        //    {
        //        _dbContext.Remove(entity);
        //    }
        //    return await DoesEnrollmentExist(enrollmentId);
        //}

        public CreditWorkEnrollment RemoveCreditWorkEnrollment(CreditWorkEnrollment enrollment) 
        {
            _dbContext.Remove(enrollment);
            return enrollment;
        } 

        public async Task<bool> ExistsAsync(Guid enrollmentId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<CreditWorkEnrollment>()
                .AsNoTracking().AnyAsync(e => e.Id == enrollmentId, cancellationToken);
        }


        public async Task<bool> ExistsAsync(Guid studentId, Guid creditWorkId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<CreditWorkEnrollment>()
                .AsNoTracking()
                .AnyAsync(e => e.StudentId == studentId && e.CreditWorkId == creditWorkId, cancellationToken);
        }

    }
}
