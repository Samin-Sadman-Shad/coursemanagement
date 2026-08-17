using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Services.Contract;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Contracts.Services
{
    public interface IStudentService
    {
        //student request for other students in their class
        Task<List<GetStudentDto>> GetPeersByStudentIdAsync(Guid studentId);

        //get all the students of a given course by staff
        Task<List<GetStudentDto>> GetStudentsByCourseIdAsync(Guid courseId);

        //view student of a given course by staff
        Task<List<GetStudentDto>> GetStudentsByCreditWorkIdAsync(Guid creditWorkId);

        Task<GetStudentDto?> GetStudentByEmailAsync(string email);

        Task<List<GetStudentDto>> GetStudentsByNameAsync(string name);
         
        Task<GetStudentDto?> GetStudentByRollAsync(int rollNo);
    }
}
