using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.JunctionEntities;

namespace University.Application.Contracts.Persistance
{
    public interface ICourseCreditWorkRegistrationRepository
    {
        Task<CourseCreditWork> RegisterCourseToCreditWork(Guid courseId, Guid creditWorkId);

        //Task<bool> UnregisterCourseFromCreditWork(Guid courseId, Guid creditWorkId);
        //Task<CourseCreditWork> UnregisterCourseFromCreditWork(Guid registrationId);
        CourseCreditWork UnregisterCourseFromCreditWork(CourseCreditWork courseCreditWork);

        Task<bool> ExistsAsync(Guid id);

        Task<bool> ExistsAsync(Guid courseId, Guid creditWorkId);

        public Task<CourseCreditWork?> GetByIdAsync(Guid id);
    }
}
