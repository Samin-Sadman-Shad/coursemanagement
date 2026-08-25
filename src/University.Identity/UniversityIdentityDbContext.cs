using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Identity.Configurations;
using University.Identity.Models;

namespace University.Identity
{
    public class UniversityIdentityDbContext:IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public UniversityIdentityDbContext(DbContextOptions<UniversityIdentityDbContext> options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
