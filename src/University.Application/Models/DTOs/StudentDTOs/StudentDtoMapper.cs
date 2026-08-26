using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.StudentDTOs
{
    internal static class StudentDtoMapper
    {
        public static GetStudentDto MapToGetStudentDto(this Student student, StaffDto staff)
        {
            return new GetStudentDto
            {
                Id = student.UserId,
                Name = student.Name,
                Roll = student.Roll,
                Email = student.Email,
                CreatedAt = student.CreatedAt,
                CreatedBy = staff 
            };
        }

        public static GetStudentWithDetailsDto MapToGetStudentWithDetailsDto(this Student student, StaffDto staff)
        {
            var courses = student.CoursesEnrolled.Select(enroll => enroll.Course)
                .Select<Course, GetCourseDto>(course => course.MapToGetCourseDto(staff)).ToList();

            var creditWorks = student.CreditWorksEnrolled.Select(enroll => enroll.CreditWork)
                .Select(creditWork => creditWork.MapToGetCreditWorkDto(staff)).ToList();

            return new GetStudentWithDetailsDto
            {
                Id = student.UserId,
                Name = student.Name,
                Roll = student.Roll,
                Email = student.Email,
                CreatedAt = student.CreatedAt,
                CreatedBy = staff,
                Courses = courses,
                CreditWorks = creditWorks
            };
        }

        public static Student MapToStudent(this CreateStudentDto createStudentDto, Guid staffId, DateTimeOffset dateTime)
        {
            return new Student
            {
                Name = createStudentDto.Name,
                Roll = createStudentDto.Roll,
                Email = createStudentDto.Email,
                CreatedAt = dateTime,
                CreatedById = staffId,
                LastModifiedById = staffId,
                LastModifiedAt = dateTime
            };
        }

        public static void UpdateStudent(this UpdateStudentDto updateStudentDto, Student entity, Guid staffId, DateTimeOffset dateTime)
        {
            entity.Name = updateStudentDto.Name ;
            entity.Roll = updateStudentDto.Roll ;
            entity.Email = updateStudentDto.Email ?? entity.Email;
            entity.LastModifiedAt = dateTime;
            entity.LastModifiedById = staffId;
        }

    }
}
