using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Identity.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData
            (
                new IdentityUserRole<string>
                {
                    RoleId = "9FAA56A1-53A5-4920-AB1D-C877494EC832",
                    UserId = "0765A886-A723-42A6-956C-37A43A9AFEB3" //samin_sadman as staff
                },
                new IdentityUserRole<string>
                {
                    RoleId = "05269E2A-75EE-4DB1-B1C1-4CD0B728EB53",
                    UserId = "F52D3DE7-CC4C-4B71-97B0-8545A8F80C8A" //sample1 -> student
                },
                new IdentityUserRole<string>
                {
                    RoleId = "05269E2A-75EE-4DB1-B1C1-4CD0B728EB53",
                    UserId = "CD594973-8439-47F8-82FA-8B097A6EE5B7" //sample2 -> student
                }
            );
        }
    }
}
