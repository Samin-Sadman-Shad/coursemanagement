using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Identity;
using University.Persistance.Context;

namespace University.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        //register for the repositories
        private  ICourseRepository? _courseRepository;
        private  ICourseEnrollmentRepository? _courseEnrollmentRepository;
        private  IStudentRepository? _studentRepository;
        private  ICreditWorkRepository? _creditWorkRepository;
        private  ICreditWorkEnrollmentRepository? _creditWorkEnrollmentRepository;
        private ICourseCreditWorkRegistrationRepository? _courseCreditWorkRegistrationRepository;

        private  UniversityDbContext _dbContext;
        private readonly UniversityIdentityDbContext _identityDbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(UniversityDbContext dbContext, UniversityIdentityDbContext identityDbContext)
        {
            _dbContext = dbContext;
            _identityDbContext= identityDbContext;
        }

        //instantiated first time the caller uses the property
        //same repo instances across multiple calls within one request
        public ICourseRepository CourseRepository => _courseRepository ??= new CourseRepository(_dbContext);

        public ICourseEnrollmentRepository CourseEnrollmentRepository => _courseEnrollmentRepository ??= 
            new CourseEnrollmentRepository(_dbContext);

        public IStudentRepository StudentRepository => _studentRepository ??= new StudentRepository(_dbContext);

        public ICreditWorkRepository CreditWorkRepository => _creditWorkRepository ??= new CreditWorkRepository(_dbContext);

        public ICreditWorkEnrollmentRepository CreditWorkEnrollmentRepository => _creditWorkEnrollmentRepository ??=
            new CreditWorkEnrollmentRepository(_dbContext);

        public ICourseCreditWorkRegistrationRepository CourseCreditWorkRegistrationRepository 
            => _courseCreditWorkRegistrationRepository ??= new CourseCreditWorkRegistrationRepository(_dbContext);

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
            await _identityDbContext.Database.UseTransactionAsync(_transaction.GetDbTransaction());
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
            await _identityDbContext.SaveChangesAsync();
            await _transaction!.CommitAsync();
        }

        public Task RollbackAsync() => _transaction?.RollbackAsync() ?? Task.CompletedTask;

        //public DbConnection Connection => _dbContext.Database.GetDbConnection();
        //public Task<IDbContextTransaction> BeginTransactionAsync()
        //{
        //    return _dbContext.Database.BeginTransaction();
        //} 
    }
}
