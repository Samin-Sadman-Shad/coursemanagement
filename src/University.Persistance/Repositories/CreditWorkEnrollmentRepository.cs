using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<CreditWorkEnrollment> CreateCreditWorkEnrollment(CreditWorkEnrollment enrollment)
        {
             await _dbContext.AddAsync<CreditWorkEnrollment>(enrollment);
            return enrollment;
        }

        public async Task<CreditWorkEnrollment?> GetEnrollment(Guid enrollmentId)
        {
            return await _dbContext.FindAsync<CreditWorkEnrollment>(enrollmentId);
        }

        public async Task<bool> DoesEnrollmentExist(Guid enrollmentId)
        {
            var entity = await GetEnrollment(enrollmentId);
            return entity is not null;
        }

        public async Task<bool> RemoveCreditWorkEnrollment(Guid enrollmentId)
        {
            var entity = await GetEnrollment(enrollmentId);
            if(entity is not null)
            {
                _dbContext.Remove(entity);
            }
            return await DoesEnrollmentExist(enrollmentId);
        }
    }
}
