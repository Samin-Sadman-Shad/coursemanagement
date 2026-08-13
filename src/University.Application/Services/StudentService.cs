using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Services.Contract;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Services
{
    public class StudentService:IGenericService<Student>
    {
        //Staff members can operation on CRUD students

        public Task DeleteStudentAsync(Guid studentId) 
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Student> CreateAsync(Student entity)
        {
            throw new NotImplementedException();
        }

        public Task<Student> UpdateAsync(Student entity)
        {
            throw new NotImplementedException();
        }

        public Task<Student> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
