using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.StudentDTOs
{
    internal static class StudentDtoMapper
    {
        public static GetStudentDto MapToGetStudentDto(this Student student)
        {
            return new GetStudentDto
            {
                Id = student.UserId,
                Name = student.Name,
                Roll = student.Roll,
                Email = student.Email,
                EnrolledAt = student.CreatedAt,
                EnrolledBy = student.CreatedBy 
            };
        }

        public static Student MapToStudent(this CreateStudentDto createStudentDto, Staff createdBy)
        {
            return new Student
            {
                Name = createStudentDto.Name,
                Roll = createStudentDto.Roll,
                Email = createStudentDto.Email,
                CreatedBy = createStudentDto.CreatedBy,
                CreatedAt = createStudentDto.CreatedAt,
                LastModifiedBy = createStudentDto.CreatedBy,
                LastModifiedAt = createStudentDto.CreatedAt
            };
        }

        public static Student MapToStudent(this UpdateStudentDto updateStudentDto)
        {
            return new Student
            {
                Name = updateStudentDto.Name,
                Roll = updateStudentDto.Roll,
                Email = updateStudentDto.Email,
                LastModifiedBy = updateStudentDto.LastModifiedBy,
                LastModifiedAt = updateStudentDto.LastModifiedAt 
            };
        }

    }
}
