using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using University.Domain.Entities.JunctionEntities;

namespace University.Application.Contracts.Persistance
{
    public interface ICreditWorkEnrollmentRepository:IEnrollment
    {
        Task<CreditWorkEnrollment> CreateCreditWorkEnrollment(CreditWorkEnrollment enrollment, CancellationToken cancellationToken = default);
        //Task<bool> RemoveCreditWorkEnrollment(Guid enrollmentId);
        CreditWorkEnrollment RemoveCreditWorkEnrollment(CreditWorkEnrollment enrollment);
        Task<CreditWorkEnrollment?> GetEnrollment(Guid enrollmentId, CancellationToken cancellationToken = default);
        Task<List<CreditWorkEnrollment>> GetAllEnrollmentAsync(CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid studentId, Guid creditWorkId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid enrollmentId, CancellationToken cancellationToken = default);

    }
}
