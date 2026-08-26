using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Contracts.Persistance;
using University.Application.Contracts.Services;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.DTOs.StudentDTOs.Validators;
using University.Application.Services.Contract;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Services
{
    //    public class StudentService
    //        :GenericService<Student, CreateStudentDto, GetStudentDto, UpdateStudentEmailDto>
    //        , IStudentService
    //    {
    //        private readonly IStudentRepository _studentRepository;
    //        public StudentService(IStudentRepository repository): base(repository)
    //        {
    //            _studentRepository = repository;
    //        }

    //        protected override GetStudentDto ToGetDto(Student entity)
    //        {
    //            return entity.MapToGetStudentDto();
    //        }

    //        protected override Student ToEntity(CreateStudentDto dto)
    //        {
    //            return dto.MapToStudent();
    //        }

    //        protected override Student ApplyUpdate(Student entity, UpdateStudentEmailDto dto)
    //        {
    //            entity.Email = dto.Email;
    //            entity.LastModifiedAt = dto.LastModifiedAt;
    //            entity.LastModifiedBy = dto.LastModifiedBy;
    //            return entity;
    //        }

    //        public async override Task<GetStudentDto> CreateAsync(CreateStudentDto dto)
    //        {
    //            //var validator = new CreateStudentDtoValidator();
    //            //var validationResult = await validator.ValidateAsync(dto);
    //            //if (!validationResult.IsValid)
    //            //{
    //            //    throw new InvalidDataContractException();
    //            //}
    //            return await base.CreateAsync(dto);
    //        }

    //        public async Task<List<GetStudentDto>> GetPeersByStudentIdAsync(Guid studentId)
    //        {
    //            var peers = await _studentRepository.GetPeersByStudentIdAsync(studentId);
    //            return peers.Select(ToGetDto).ToList();
    //        }

    //        public async Task<List<GetStudentDto>> GetStudentsByCourseIdAsync(Guid courseId)
    //        {
    //            var students = await _studentRepository.GetStudentsByCourseIdAsync(courseId);
    //            return students.Select(ToGetDto).ToList();
    //        }

    //        public async Task<List<GetStudentDto>> GetStudentsByCreditWorkIdAsync(Guid creditWorkId)
    //        {
    //            var students = await _studentRepository.GetStudentsByCreditWorkIdAsync(creditWorkId);
    //            return students.Select(ToGetDto).ToList();
    //        }

    //        public async Task<GetStudentDto?> GetStudentByEmailAsync(string email)
    //        {
    //            var student = await _studentRepository.GetStudentByEmailAsync(email);
    //            if(student is not null)
    //            {
    //                return student.MapToGetStudentDto();
    //            }
    //            throw new NullReferenceException($"Student {email} is not found!");

    //        }

    //        public async Task<List<GetStudentDto>> GetStudentsByNameAsync(string name)
    //        {
    //            var students = await _studentRepository.GetStudentsByNameAsync(name);
    //            return students.Select(ToGetDto).ToList();
    //        }

    //        public async Task<GetStudentDto?> GetStudentByRollAsync(int rollNo)
    //        {
    //            var student = await _studentRepository.GetStudentByRollAsync(rollNo);
    //            if (student is not null)
    //            {
    //                return student.MapToGetStudentDto();
    //            }
    //            throw new NullReferenceException($"Student {rollNo} is not found!");
    //        }

    //    }


}
