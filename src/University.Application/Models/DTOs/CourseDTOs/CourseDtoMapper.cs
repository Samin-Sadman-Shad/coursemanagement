using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.StudentDTOs;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CourseDTOs
{
    public static class CourseDtoMapper
    {
        public static GetCourseWithDetailsDto MapToGetCourseWithDetailsDto(this Course course)
        {
            var courses = course.CreditWorksInCourse
                .Select(cw => cw.CreditWork)
                .Select(creditwork => creditwork.MapToGetCreditWorkDto())
                .ToList();
            var students = course.StudentsInCourse
                .Select(enrollment => enrollment.Student)
                .Select(student => student.MapToGetStudentDto())
                .ToList();
            return new GetCourseWithDetailsDto
            {
                Id = course.Id,
                CourseTitle = course.Title,
                CreditWorks = courses,
                Students = students
            };
        }

        public static GetCourseDto MapToGetCourseDto(this Course course)
        {
            return new GetCourseDto
            {
                Id = course.Id,
                CourseTitle = course.Title,
            };
        }

        public static Course MapToCourse(this CreateCourseDto createCourseDto)
        {
            return new Course
            {
                Title = createCourseDto.CourseTitle,
                CreatedBy = createCourseDto.CreatedBy,
                CreatedAt = createCourseDto.CreatedAt,
                LastModifiedBy = createCourseDto.CreatedBy,
                LastModifiedAt = createCourseDto.CreatedAt
            };
        }

        public static void UpdateCourse(this UpdateCourseTitleDto updateCourseDto, Course entity)
        {
            entity.Title = updateCourseDto.CourseTitle;
            entity.LastModifiedBy = updateCourseDto.LastModifiedBy;
            entity.LastModifiedAt = updateCourseDto.LastModifiedAt;
        }
    }
}
