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
                NormalizedUserName = "SAMIN_SADMAN",
                Email = "samin_sadman.buet@gmail.com",
                NormalizedEmail = "SAMIN_SADMAN.BUET@GMAIL.COM",
                EmailConfirmed = true,

                // Password: P@ssword1
                PasswordHash = "AQAAAAEAAYagAAAAELaQ9AunoTuZ7zchIUDM7+O7eH0nEH6Fd+pgjfJWuqJeZS4KNRVzbM2nkWraeBYwDg==",

                SecurityStamp = "11111111-1111-1111-1111-111111111111",
                ConcurrencyStamp = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"
            };

            var sample_student_1 = new ApplicationUser
            {
                Id = Guid.Parse("F52D3DE7-CC4C-4B71-97B0-8545A8F80C8A"),
                UserName = "sample_student_1",
                NormalizedUserName = "SAMPLE_STUDENT_1",
                Email = "sample_student1.buet@gmail.com",
                NormalizedEmail = "SAMPLE_STUDENT1.BUET@GMAIL.COM",
                EmailConfirmed = true,

                // Password: P@ssword1
                PasswordHash = "AQAAAAEAAYagAAAAEEP7InTiB5JTLdFnA93mdbet7CLgYsGY8syE0zwJWwpMZY8UPzL5f5cUjfnFJK0V3w==",

                SecurityStamp = "22222222-2222-2222-2222-222222222222",
                ConcurrencyStamp = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"
            };

            var sample_student_2 = new ApplicationUser
            {
                Id = Guid.Parse("CD594973-8439-47F8-82FA-8B097A6EE5B7"),
                UserName = "sample_student_2",
                NormalizedUserName = "SAMPLE_STUDENT_2",
                Email = "sample_student2.buet@gmail.com",
                NormalizedEmail = "SAMPLE_STUDENT2.BUET@GMAIL.COM",
                EmailConfirmed = true,

                // Password: P@ssword1
                PasswordHash = "AQAAAAEAAYagAAAAENd7rsOWBku8z9g7AyrfW/oS7GZb1xXOdJpEH6HNzm3yaT6rQhtPn4Mh8ebMQzu+3g==",

                SecurityStamp = "33333333-3333-3333-3333-333333333333",
                ConcurrencyStamp = "cccccccc-cccc-cccc-cccc-cccccccccccc"
            };

            builder.HasData(
                samin,
                sample_student_1,
                sample_student_2
            );
        }
    }
}
