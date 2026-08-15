using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Contracts.Persistance
{
    public interface IStudentRepository: IGenericRepository<Student>
    {
        //student request for own courses
        Task<List<Course>> GetCoursesByStudentIdAsync(Guid studentId);

        //student request for own credit works
        Task<List<CreditWork>> GetCreditWorksByStudentIdAsync(Guid studentId);

        //student request for other students in their class
        Task<List<Student>> GetPeersByStudentIdAsync(Guid studentId);
    }
}
