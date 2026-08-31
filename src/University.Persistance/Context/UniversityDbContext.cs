using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.Contract;
using University.Domain.Entities.JunctionEntities;

namespace University.Persistance.Context
{
    public class UniversityDbContext : DbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
        {

        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite unique constraints for junction tables
            modelBuilder.Entity<CourseCreditWork>()
                .HasIndex(ccw => new { ccw.CourseId, ccw.CreditWorkId })
                .IsUnique()
                .HasDatabaseName("IX_CourseId_CreditWorkId_Unique");

            modelBuilder.Entity<CourseEnrollment>()
                .HasIndex(ce => new { ce.StudentId, ce.CourseId })
                .IsUnique()
                .HasDatabaseName("IX_StudentId_CourseId_Unique");

            modelBuilder.Entity<CreditWorkEnrollment>()
                .HasIndex(cwe => new { cwe.StudentId, cwe.CreditWorkId })
                .IsUnique()
                .HasDatabaseName("IX_StudentId_CreditWorkId_Unique");

            // Configure unique constraints for scalar business keys
            modelBuilder.Entity<Course>()
                .HasIndex(c => c.Title)
                .IsUnique()
                .HasDatabaseName("IX_Course_Title_Unique");

            modelBuilder.Entity<CreditWork>()
                .HasIndex(cw => new { cw.Heading, cw.Code })
                .IsUnique()
                .HasDatabaseName("IX_CreditWork_Heading_Code_Unique");

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique()
                .HasDatabaseName("IX_Student_Email_Unique");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<IBaseEntity>())
            {
                var entity = entry.Entity;
                entity.LastModifiedAt = DateTimeOffset.UtcNow;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTimeOffset.UtcNow;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CreditWork> CreditWorks { get; set; }
        public DbSet<CourseEnrollment> CourseEnrollments { get; set; }
        public DbSet<CreditWorkEnrollment> CreditWorkEnrollments { get; set; }
        public DbSet<CourseCreditWork> CreditWorksInCourses { get; set; }

    }
}
