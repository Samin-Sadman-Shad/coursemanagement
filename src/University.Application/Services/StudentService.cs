using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Contracts.Persistance;
using University.Application.Contracts.Services;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Services.Contract;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Services
{
    public class StudentService
        :GenericService<Student, CreateStudentDto, GetStudentDto, UpdateStudentEmailDto>
        , IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        public StudentService(IStudentRepository repository): base(repository)
        {
            _studentRepository = repository;
        }

        protected override GetStudentDto ToGetDto(Student entity)
        {
            return entity.MapToGetStudentDto();
        }

        protected override Student ToEntity(CreateStudentDto dto, Staff createdBy)
        {
            return dto.MapToStudentDto(createdBy, createdBy);
        }

        protected override Student ApplyUpdate(Student entity, UpdateStudentEmailDto dto, Staff updatedBy)
        {
            entity.Email = dto.Email;
            entity.LastModifiedAt = dto.LastModifiedAt;
            entity.LastModifiedBy = updatedBy;
            return entity;
        }

        public async Task<List<GetStudentDto>> GetPeersByStudentIdAsync(Guid studentId)
        {
            var peers = await _studentRepository.GetPeersByStudentIdAsync(studentId);
            return peers.Select(ToGetDto).ToList();
        }

        public async Task<List<GetStudentDto>> GetStudentsByCourseIdAsync(Guid courseId)
        {
            var students = await _studentRepository.GetStudentsByCourseIdAsync(courseId);
            return students.Select(ToGetDto).ToList();
        }

        public async Task<List<GetStudentDto>> GetStudentsByCreditWorkIdAsync(Guid creditWorkId)
        {
            var students = await _studentRepository.GetStudentsByCreditWorkIdAsync(creditWorkId);
            return students.Select(ToGetDto).ToList();
        }

        public async Task<GetStudentDto?> GetStudentByEmailAsync(string email)
        {
            var student = await _studentRepository.GetStudentByEmailAsync(email);
            if(student is not null)
            {
                return student.MapToGetStudentDto();
            }
            throw new NullReferenceException($"Student {email} is not found!");
            
        }

        public async Task<List<GetStudentDto>> GetStudentsByNameAsync(string name)
        {
            var students = await _studentRepository.GetStudentsByNameAsync(name);
            return students.Select(ToGetDto).ToList();
        }

        public async Task<GetStudentDto?> GetStudentByRollAsync(int rollNo)
        {
            var student = await _studentRepository.GetStudentByRollAsync(rollNo);
            if (student is not null)
            {
                return student.MapToGetStudentDto();
            }
            throw new NullReferenceException($"Student {rollNo} is not found!");
        }

    }

    
}
