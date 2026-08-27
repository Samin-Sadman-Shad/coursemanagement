using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Staff;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.JunctionEntities;

namespace University.Tests.Common
{
    
    
    public static class EntityTestFactory
    {
        public static Student CreateStudent(Guid? userId = null) => new()
        {
            UserId = userId ?? Guid.NewGuid(),
            Name = "Test Student",
            Roll = 1
        };

        public static CreditWork CreateCreditWork(Guid? id = null) => new()
        {
            Id = id ?? Guid.NewGuid(),
            Heading = "Test Credit Work",
            Code = 101
        };

        public static Course CreateCourse(Guid? id = null) => new()
        {
            Id = id ?? Guid.NewGuid(),
            Title = "Test Course"
        };

        public static CreditWorkEnrollment CreateCreditWorkEnrollment(
            Guid? id = null,
            Guid? enrolledById = null,
            Student? student = null,
            CreditWork? creditWork = null)
        {
            var resolvedStudent = student ?? CreateStudent();
            var resolvedCreditWork = creditWork ?? CreateCreditWork();

            return new CreditWorkEnrollment
            {
                Id = id ?? Guid.NewGuid(),
                EnrolledById = enrolledById ?? Guid.NewGuid(),
                StudentId = resolvedStudent.UserId,
                Student = resolvedStudent,
                CreditWorkId = resolvedCreditWork.Id,
                CreditWork = resolvedCreditWork
            };
        }

        public static CourseEnrollment CreateCourseEnrollment(
            Guid? id = null,
            Guid? enrolledById = null,
            Student? student = null,
            Course? course = null)
        {
            var resolvedStudent = student ?? CreateStudent();
            var resolvedCourse = course ?? CreateCourse();

            return new CourseEnrollment
            {
                Id = id ?? Guid.NewGuid(),
                EnrolledById = enrolledById ?? Guid.NewGuid(),
                StudentId = resolvedStudent.UserId,
                Student = resolvedStudent,
                CourseId = resolvedCourse.Id,
                Course = resolvedCourse
            };
        }

        public static CourseCreditWork CreateCourseCreditWork(
            Guid? id = null,
            Course? course = null,
            CreditWork? creditWork = null)
        {
            var resolvedCourse = course ?? CreateCourse();
            var resolvedCreditWork = creditWork ?? CreateCreditWork();

            return new CourseCreditWork
            {
                Id = id ?? Guid.NewGuid(),
                CourseId = resolvedCourse.Id,
                Course = resolvedCourse,
                CreditWorkId = resolvedCreditWork.Id,
                CreditWork = resolvedCreditWork
            };
        }
    }
    
}
