using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.DTOs.StudentDTOs;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.CreditWorkDTOs
{
    public static class CreditWorkDtoMapper
    {
        public static GetCreditWorkDto MapToGetCreditWorkDto(this CreditWork creditWork, StaffDto staff)
        {
            return new GetCreditWorkDto
            {
                Id = creditWork.Id,
                Heading = creditWork.Heading,
                Code = creditWork.Code,
                Description = creditWork.Description,
                CreatedAt = creditWork.CreatedAt,
                CreatedBy = staff
            };
        }

        public static GetCreditWorkWithDetailsDto MapToGetCreditWorkWithDetailsDto(this CreditWork creditWork, StaffDto staff)
        {
            var studentDtos = creditWork.StudentsInCreditWork.Select(enroll => enroll.Student)
                .Select(student => student.MapToGetStudentDto(staff)).ToList();
            var courseDtos = creditWork.CoursesOfCreditWork.Select(enroll => enroll.Course)
                .Select(course => course.MapToGetCourseDto(staff)).ToList();
            return new GetCreditWorkWithDetailsDto
            {
                Id = creditWork.Id,
                Heading = creditWork.Heading,
                Code = creditWork.Code,
                Description = creditWork.Description,
                Courses = courseDtos,
                Students = studentDtos,
                CreatedBy = staff
            };

        }

        public static CreditWork MaptoCreditWork(this CreateCreditWorkDto createCreditWorkDto, Guid staffId, DateTimeOffset dateTime)
        {
            return new CreditWork
            {
                Heading = createCreditWorkDto.Heading,
                Code = createCreditWorkDto.Code,
                Description = createCreditWorkDto.Description,
                CreatedById = staffId,
                CreatedAt = dateTime
            };
        }

        public static void UpdateCreditWork(this UpdateCreditWorkDto updateCreditWorkDto, CreditWork entity
            , Guid staffId, DateTimeOffset dateTime)
        {
            entity.Code = updateCreditWorkDto.Code;
            entity.Heading = updateCreditWorkDto.Heading;
            entity.Description = updateCreditWorkDto.Description;
            entity.LastModifiedById = staffId;
            entity.LastModifiedAt = dateTime;
        }
    }
}
