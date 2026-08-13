using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Contracts.Persistance
{
    public interface ICreditWorkRepository: IGenericRepository<CreditWork>
    {
        //view student of a given course by staff
        Task<List<Student>> GetStudentsByCreditWorkIdAsync(Guid creditWorkId);

        //all the courses of a gievn class
        Task<List<Course>> GetCoursesByCreditWorkIdAsync(Guid creditWorkId);
    }
}
