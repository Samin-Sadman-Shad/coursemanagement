using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Persistance;
using University.Persistance.Context;
using University.Persistance.Repositories;

namespace University.Persistance.DI
{
    public static class PersistanceServiceRegistration
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("UniversityDbConnectionString");

            services.AddScoped<NpgsqlConnection>(_ => new NpgsqlConnection(connectionString));

            services.AddDbContext<UniversityDbContext>( (sp, options) =>
            {
                options.UseNpgsql(sp.GetRequiredService<NpgsqlConnection>());
            }); 
            //postgres provider added later
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ICreditWorkRepository, CreditWorkRepository>();
            services.AddScoped<ICourseEnrollmentRepository, CourseEnrollmentRepository>();
            services.AddScoped<ICreditWorkEnrollmentRepository, CreditWorkEnrollmentRepository>();
            services.AddScoped<ICourseCreditWorkRegistrationRepository, CourseCreditWorkRegistrationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
