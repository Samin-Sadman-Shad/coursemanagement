using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        Task SaveChangesAsync();
    }
}
