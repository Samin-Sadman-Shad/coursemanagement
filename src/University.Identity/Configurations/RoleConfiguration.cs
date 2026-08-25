using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.Identity;
using University.Identity.Models;

namespace University.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(
                new ApplicationRole
                {
                    Id = Guid.Parse("9FAA56A1-53A5-4920-AB1D-C877494EC832"),
                    Name = RoleEnum.STAFF.ToString(),
                },
                new ApplicationRole
                {
                    Id = Guid.Parse("05269E2A-75EE-4DB1-B1C1-4CD0B728EB53"),
                    Name = RoleEnum.STUDENT.ToString()
                }
             );
        }
    }
}
