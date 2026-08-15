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
    }
}
