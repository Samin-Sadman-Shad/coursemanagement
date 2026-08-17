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
        public static GetStudentDto MapToGetStudentDto(this Student student, Staff staff)
        {
            return new GetStudentDto
            {
                Id = student.UserId,
                Name = student.Name,
                Roll = student.Roll,
                Email = student.Email,
                EnrolledAt = student.CreatedAt,
                EnrolledBy = student.CreatedBy ??= staff
            };
        }

        public static CreateStudentDto MapToCreateStudentDto(this Student student, Staff staff)
        {
            return new CreateStudentDto
            {
                Name = student.Name,
                Roll = student.Roll,
                Email = student.Email,
                CreatedBy = student.CreatedBy ??= staff,
                CreatedAt = student.CreatedAt
            };
        }

        public static UpdateStudentDto MapToUpdateStudentDto(this Student student, Staff staff)
        {
            return new UpdateStudentDto
            {
                Name = student.Name,
                Roll = student.Roll,
                Email = student.Email,
                LastModifiedBy = student.LastModifiedBy ??= student.CreatedBy ??= staff,
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
                CreatedAt = DateTimeOffset.UtcNow,
                LastModifiedBy = createdBy,
                LastModifiedAt = DateTimeOffset.UtcNow
            };
        }

    }
}
