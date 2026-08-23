using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Identity;
using University.Application.Models.Identity;
using University.Identity.Models;
using University.Identity.Services;

namespace University.Identity.DI
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSettingsOptions jwtSettingOptions = new JwtSettingsOptions();
            var section = configuration.GetSection(JwtSettingsOptions.jwtSettings);
            section.Bind(jwtSettingOptions);
            services.AddSingleton(jwtSettingOptions);

            var identityConnectionString = configuration.GetConnectionString("IdentityDbConnectionString");

            services.AddDbContext<UniversityIdentityDbContext>(options =>
            {
                //options.UseSqlServer(configuration.GetConnectionString("IdentityDbConnectionString"),
                //    sqlOptionBuilder =>
                //    {
                //        sqlOptionBuilder.MigrationsAssembly(typeof(LeaveManagementIdentityDbContext).Assembly.FullName);
                //    });
                options.UseNpgsql(identityConnectionString, options =>
                options.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null
                    )
                );
                
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<UniversityIdentityDbContext>()
                .AddDefaultTokenProviders();

            //everytime user tries to login, new service will be created
            services.AddTransient<IAuthService, AuthService>();

            var jwtKey = configuration["JwtSettings:Key"]
                ?? throw new InvalidOperationException(
                            "JWT signing key is not configured.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });
            return services;
        }
    }
}
