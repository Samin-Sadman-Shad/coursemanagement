using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Identity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "9FAA56A1-53A5-4920-AB1D-C877494EC832",
                    Name = "STAFF",
                },
                new IdentityRole
                {
                    Id = "05269E2A-75EE-4DB1-B1C1-4CD0B728EB53",
                    Name = "STUDENT"
                }
             );
        }
    }
}
