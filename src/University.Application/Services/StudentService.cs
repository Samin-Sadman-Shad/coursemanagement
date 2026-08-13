using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using University.Domain.Entities.BaseEntities;

namespace University.Application.Services
{
    public class StudentService
    {
        //Staff members can operation on CRUD students
        public async Task<Student> CreateStudentAsync(Student student)
        {
            // Implementation for creating a student
            throw new NotImplementedException();
        }

        public async Task<Student> GetStudentByIdAsync(Guid studentId)
        {
            // Implementation for retrieving a student by ID
            throw new NotImplementedException();
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            // Implementation for updating a student
            throw new NotImplementedException();
        }

        public Task DeleteStudentAsync(Guid studentId) 
        {
            throw new NotImplementedException();
        }

    }
}
