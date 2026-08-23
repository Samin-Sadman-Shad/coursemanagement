using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.StudentDTOs;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Contracts.Persistance
{
    public interface IStudentRepository: IGenericRepository<Student>
    {
        //student request for other students in their class
        Task<List<Student>> GetPeersByStudentIdAsync(Guid studentId);

        //get all the students of a given course by staff
        Task<List<Student>> GetStudentsByCourseIdAsync(Guid courseId);

        //view student of a given course by staff
        Task<List<Student>> GetStudentsByCreditWorkIdAsync(Guid creditWorkId);

        Task<Student?> GetStudentByEmailAsync(string email);

        Task<List<Student>> GetStudentsByNameAsync(string name);

        Task<Student?> GetStudentByRollAsync(int? rollNo);

        Task<bool> DoesEmailExistAsync(string email, Guid? excludeUserId = null);
    }
}
