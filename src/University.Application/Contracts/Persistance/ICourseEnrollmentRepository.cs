using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using University.Domain.Entities.JunctionEntities;

namespace University.Application.Contracts.Persistance
{
    public interface ICourseEnrollmentRepository:IEnrollment
    {
        Task<CourseEnrollment> CreateCourseEnrollment(CourseEnrollment enrollment, CancellationToken cancellationToken = default);
        CourseEnrollment RemoveCourseEnrollment(CourseEnrollment enrollment);
        Task<bool> ExistsAsync(Guid enrollmentId, CancellationToken cancellationToken = default);

        Task<CourseEnrollment?> GetEnrollment(Guid enrollmentId, CancellationToken cancellationToken = default);
        Task<List<CourseEnrollment>> GetAllEnrollmentAsync(CancellationToken cancellationToken = default);
    }
}
