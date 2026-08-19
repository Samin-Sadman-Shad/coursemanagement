using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.JunctionEntities;

namespace University.Application.Contracts.Persistance
{
    public interface ICreditWorkEnrollmentRepository:IEnrollment
    {
        Task<CreditWorkEnrollment> CreateCreditWorkEnrollment(CreditWorkEnrollment enrollment);
        Task<bool> RemoveCreditWorkEnrollment(Guid enrollmentId);
        Task<CreditWorkEnrollment?> GetEnrollment(Guid enrollmentId);

    }
}
