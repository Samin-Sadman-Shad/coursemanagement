using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.CreditWorkDTOs;
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

        public static GetStudentWithDetailsDto MapToGetStudentWithDetailsDto(this Student student)
        {
            var courses = student.CoursesEnrolled.Select(enroll => enroll.Course)
                .Select<Course, GetCourseWithDetailsDto>(course => course.MapToGetCourseDto()).ToList();

            var creditWorks = student.CreditWorksEnrolled.Select(enroll => enroll.CreditWork)
                .Select(creditWork => creditWork.MapToGetCreditWorkDto()).ToList();

            return new GetStudentWithDetailsDto
            {
                Id = student.UserId,
                Name = student.Name,
                Roll = student.Roll,
                Email = student.Email,
                EnrolledAt = student.CreatedAt,
                EnrolledBy = student.CreatedBy,
                Courses = courses,
                CreditWorks = creditWorks
            };
        }

        public static Student MapToStudent(this CreateStudentDto createStudentDto)
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

        public static void UpdateStudent(this UpdateStudentDto updateStudentDto, Student entity)
        {
            entity.Name = updateStudentDto.Name ;
            entity.Roll = updateStudentDto.Roll ;
            entity.Email = updateStudentDto.Email ?? entity.Email;
            entity.LastModifiedAt = updateStudentDto.LastModifiedAt;
            entity.LastModifiedBy = updateStudentDto.LastModifiedBy;
        }

    }
}
