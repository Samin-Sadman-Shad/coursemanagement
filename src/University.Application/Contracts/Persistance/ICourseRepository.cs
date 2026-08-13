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
        //get all the students of a given course by staff
        Task<List<Student>> GetStudentsByCourseIdAsync(Guid courseId);

        //all the classes of a given course
        Task<List<CreditWork>> GetCreditWorksByCourseIdAsync(Guid courseId);
    }
}
