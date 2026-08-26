using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.DTOs.StudentDTOs;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CourseDTOs
{
    public static class CourseDtoMapper
    {
        public static GetCourseWithDetailsDto MapToGetCourseWithDetailsDto(this Course course, StaffDto staff)
        {
            var courses = course.CreditWorksInCourse
                .Select(cw => cw.CreditWork)
                .Select(creditwork => creditwork.MapToGetCreditWorkDto(staff))
                .ToList();
            var students = course.StudentsInCourse
                .Select(enrollment => enrollment.Student)
                .Select(student => student.MapToGetStudentDto(staff))
                .ToList();
            return new GetCourseWithDetailsDto
            {
                Id = course.Id,
                CourseTitle = course.Title,
                CreditWorks = courses,
                Students = students,
                CreatedBy = staff,
                CreatedAt = course.CreatedAt
            };
        }

        public static GetCourseDto MapToGetCourseDto(this Course course, StaffDto staff)
        {
            return new GetCourseDto
            {
                Id = course.Id,
                CourseTitle = course.Title,
                CreatedBy = staff,
                CreatedAt = course.CreatedAt
            };
        }

        public static Course MapToCourse(this CreateCourseDto createCourseDto, Guid staffId, DateTimeOffset dateTime)
        {
            return new Course
            {
                Title = createCourseDto.CourseTitle,
                CreatedById = staffId,
                CreatedAt = dateTime,
                LastModifiedById = staffId,
                LastModifiedAt = dateTime
            };
        }

        public static void UpdateCourse(this UpdateCourseTitleDto updateCourseDto, 
            Course entity, 
            Guid staffId, 
            DateTimeOffset dateTime)
        {
            entity.Title = updateCourseDto.CourseTitle;
            entity.LastModifiedById = staffId;
            entity.LastModifiedAt = dateTime;
        }
    }
}
