using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Identity.Models;

namespace University.Identity.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var samin = new ApplicationUser
            {
                Id = Guid.Parse("0765A886-A723-42A6-956C-37A43A9AFEB3"),
                UserName = "samin_sadman",
                Email = "samin_sadman.buet@gmail.com",
                EmailConfirmed = true,
            };
            var sample_student_1 = new ApplicationUser
            {
                Id = Guid.Parse("F52D3DE7-CC4C-4B71-97B0-8545A8F80C8A"),
                UserName = "sample_student_1",
                Email = "sample_student1.buet@gmail.com",
                EmailConfirmed = true,
            };
            var sample_student_2 = new ApplicationUser
            {
                Id = Guid.Parse("CD594973-8439-47F8-82FA-8B097A6EE5B7"),
                UserName = "sample_student_2",
                Email = "sample_student2.buet@gmail.com",
                EmailConfirmed = true,
            };
            var hasher = new PasswordHasher<ApplicationUser>();
            samin.PasswordHash = hasher.HashPassword(samin, "P@ssword1");
            sample_student_1.PasswordHash = hasher.HashPassword(sample_student_1, "P@ssword1");
            sample_student_2.PasswordHash = hasher.HashPassword(sample_student_2, "P@ssword1");

            builder.HasData
            (
                  samin,
                  sample_student_1,
                  sample_student_2
            );
        }
    }
}
