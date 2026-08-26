using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Identity;
using University.Application.Models.DTOs.Staff;
using University.Application.Models.Identity;
using University.Identity.Models;

namespace University.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<(Guid UserId, string ResetToken)> CreateStudentAccountAsync(string email, string name)
        {
            var username = string.Join(
                ".",
                name.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            );
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = email,
                UserName = username.ToLowerInvariant(),
                EmailConfirmed = false
            };
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, RoleEnum.STUDENT.ToString());
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return (user.Id, token);
        }

        public async Task<StaffDto?> GetStaffByIdAsync(Guid staffId)
        {
            var user = await _userManager.FindByIdAsync(staffId.ToString());
            if(user is null) return null;
            return new StaffDto
            {
                UserName = user.UserName ?? user.Email ?? string.Empty, 
                Email = user.Email ?? string.Empty
             };
        }

    }
}
