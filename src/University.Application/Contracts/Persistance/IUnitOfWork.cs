using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace University.Application.Contracts.Persistance
{
    public interface IUnitOfWork:IDisposable
    {
        //repositories can only be set from constructor
        ICourseRepository CourseRepository { get; }
        ICourseEnrollmentRepository CourseEnrollmentRepository { get; }
        IStudentRepository StudentRepository { get; }
        ICreditWorkRepository CreditWorkRepository { get; }
        ICreditWorkEnrollmentRepository CreditWorkEnrollmentRepository { get; }
        ICourseCreditWorkRegistrationRepository CourseCreditWorkRegistrationRepository { get;}

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
