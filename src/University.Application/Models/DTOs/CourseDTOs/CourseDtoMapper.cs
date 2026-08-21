using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CourseDTOs
{
    public static class CourseDtoMapper
    {
        public static GetCourseWithDetailsDto MapToGetCourseDto(this Course course)
        {
            return new GetCourseWithDetailsDto
            {
                Id = course.Id,
                CourseTitle = course.Title,
                //CreditWorks = course.CreditWorks.Select(cw => new CreditWorkDto
                //{
                //    Id = cw.Id,
                //    Title = cw.Title,
                //    Description = cw.Description,
                //    MaxScore = cw.MaxScore
                //}).ToList()
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
    }
}
