using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Domain.Entities.JunctionEntities;

namespace University.Persistance.Repositories
{
    internal class CourseEnrollmentRepository : ICourseEnrollmentRepository
    {
        private readonly DbContext _dbContext;
        public async Task<CourseEnrollment> CreateCourseEnrollment(CourseEnrollment enrollment)
        {
            return await _dbContext.Add(enrollment);
        }
    }
}
