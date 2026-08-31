using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using University.Domain.Entities.JunctionEntities;

namespace University.Application.Contracts.Persistance
{
    public interface ICourseCreditWorkRegistrationRepository
    {
        Task<CourseCreditWork> RegisterCourseToCreditWork(Guid courseId, Guid creditWorkId, Guid staffId, CancellationToken cancellationToken = default);

        //Task<bool> UnregisterCourseFromCreditWork(Guid courseId, Guid creditWorkId);
        //Task<CourseCreditWork> UnregisterCourseFromCreditWork(Guid registrationId);
        CourseCreditWork UnregisterCourseFromCreditWork(CourseCreditWork courseCreditWork);

        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(Guid courseId, Guid creditWorkId, CancellationToken cancellationToken = default);

        public Task<CourseCreditWork?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<CourseCreditWork>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
