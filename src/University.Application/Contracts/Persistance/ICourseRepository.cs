using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Contracts.Persistance
{
    public interface ICourseRepository:IGenericRepository<Course>
    {
        //student request for own courses
        Task<List<Course>> GetCoursesByStudentIdAsync(Guid studentId);

        //all the courses of a gievn class
        Task<List<Course>> GetCoursesByCreditWorkIdAsync(Guid creditWorkId);

    }
}
