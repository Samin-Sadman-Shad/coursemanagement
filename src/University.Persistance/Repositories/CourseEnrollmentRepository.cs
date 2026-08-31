using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public async Task<CourseEnrollment> CreateCourseEnrollment(CourseEnrollment enrollment, CancellationToken cancellationToken = default)
        {
            try
            {
                await _dbContext.AddAsync<CourseEnrollment>(enrollment, cancellationToken);
                //fetch all the creditworks of this course and create credit enrollments for the student
                var creditWorks = await _dbContext.CreditWorksInCourses
                    .Where(courseToCredit => courseToCredit.CourseId == enrollment.CourseId)
                    .Select(courseToCredit => new { courseToCredit.CreditWorkId, courseToCredit.CreditWork })
                    .ToListAsync(cancellationToken);
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
                        EnrolledById = enrollment.EnrolledById,
                        CreatedById = enrollment.CreatedById,
                        CreatedAt = enrollment.CreatedAt,
                    };
                    await _dbContext.AddAsync<CreditWorkEnrollment>(creditEnrollment, cancellationToken);
                }
                return enrollment;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating course enrollment: {ex.Message}", ex);
            }

        }

        public async Task<CourseEnrollment?> GetEnrollment(Guid enrollmentId, CancellationToken cancellationToken = default)
        {
            //return await _dbContext.FindAsync<CourseEnrollment>(enrollmentId)
            return await _dbContext.CourseEnrollments
                .Include(enroll => enroll.Student)
                .Include(enroll => enroll.Course)
                .FirstOrDefaultAsync(enroll => enroll.Id == enrollmentId, cancellationToken);
        }

        public async Task<List<CourseEnrollment>> GetAllEnrollmentAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.CourseEnrollments
                .Include(enrollment => enrollment.Student)
                .Include(enrollment => enrollment.Course)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> DoesEnrollmentExist(Guid enrollmentId)
        {
            var entity = await GetEnrollment(enrollmentId);
            return entity is not null;
        }

        public CourseEnrollment RemoveCourseEnrollment(CourseEnrollment enrollment)
        {
            _dbContext.Remove(enrollment);
            return enrollment;
            //return await DoesEnrollmentExist(enrollmentId);
        }

        public async Task<bool> ExistsAsync(Guid enrollmentId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<CourseEnrollment>()
                .AsNoTracking()
                .AnyAsync(e => e.Id == enrollmentId, cancellationToken);
        }

    }
}
