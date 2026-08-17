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

        public static CreateStudentDto MapToCreateStudentDto(this Student student)
        {
            return new CreateStudentDto
            {
                Name = student.Name,
                Roll = student.Roll,
                Email = student.Email,
                CreatedBy = student.CreatedBy,
                CreatedAt = student.CreatedAt
            };
        }

        public static UpdateStudentDto MapToUpdateStudentDto(this Student student)
        {
            return new UpdateStudentDto
            {
                Name = student.Name,
                Roll = student.Roll,
                Email = student.Email,
                ModifiedBy = student.LastModifiedBy,
                LastModifiedAt = student.LastModifiedAt
            };
        }

        public static Student MapToStudent(this CreateStudentDto createStudentDto, Staff createdBy)
        {
            return new Student
            {
                Name = createStudentDto.Name,
                Roll = createStudentDto.Roll,
                Email = createStudentDto.Email,
                CreatedBy = createdBy,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        public static Student MapToStudent(this UpdateStudentDto updateStudentDto, Student existingStudent, Staff modifiedBy)
        {
            existingStudent.Name = updateStudentDto.Name;
            existingStudent.Roll = updateStudentDto.Roll;
            existingStudent.Email = updateStudentDto.Email;
            existingStudent.LastModifiedBy = modifiedBy;
            existingStudent.LastModifiedAt = DateTimeOffset.UtcNow;
            return existingStudent;
        }

        public static Student MapToStudent(this GetStudentDto getStudentDto, Staff modifiedBy)
        {
            return new Student
            {
                UserId = getStudentDto.Id,
                Name = getStudentDto.Name,
                Roll = getStudentDto.Roll,
                Email = getStudentDto.Email,
                LastModifiedBy = modifiedBy,
                LastModifiedAt = DateTimeOffset.UtcNow
            };
        }

    }
}
