using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Domain.Entities.JunctionEntities;
using University.Persistance.Context;

namespace University.Persistance.Repositories
{
    internal class CourseEnrollmentRepository : ICourseEnrollmentRepository
    {
        private readonly UniversityDbContext _dbContext;
        public CourseEnrollmentRepository(UniversityDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<CourseEnrollment> CreateCourseEnrollment(CourseEnrollment enrollment)
        {
            try
            {
                await _dbContext.AddAsync<CourseEnrollment>(enrollment);
                //fetch all the creditworks of this course and create credit enrollments for the student
                var creditWorks = await _dbContext.CreditWorksInCourses
                    .Where(courseToCredit => courseToCredit.CourseId == enrollment.CourseId)
                    .Select(courseToCredit => new { courseToCredit.CreditWorkId, courseToCredit.CreditWork })
                    .ToListAsync();
                foreach (var creditWorkWrapper in creditWorks)
                {
                    var creditWorkId = creditWorkWrapper.CreditWorkId;
                    var creditWorkEntity = creditWorkWrapper.CreditWork;
                    CreditWorkEnrollment creditEnrollment = new CreditWorkEnrollment
                    {
                        CreditWorkId = creditWorkId,
                        CreditWork = creditWorkEntity,
                        StudentId = enrollment.StudentId,
                        Student = enrollment.Student,
                        EnrolledAt = enrollment.EnrolledAt,
                        StaffId = enrollment.StaffId,
                        EnrolledBy = enrollment.EnrolledBy,
                        CreatedBy = enrollment.CreatedBy,
                        CreatedAt = enrollment.CreatedAt,
                    };
                    await _dbContext.AddAsync<CreditWorkEnrollment>(creditEnrollment);
                }
                return enrollment;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating course enrollment: {ex.Message}", ex);
            }

        }
    }
}
