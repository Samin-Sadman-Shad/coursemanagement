using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Staff;

namespace University.Application.Contracts.Identity
{
    public interface IUserService
    {
        Task<(Guid UserId, string ResetToken)> CreateStudentAccountAsync(string email, string name);
        Task<StaffDto?> GetStaffByIdAsync(Guid staffId);
    }
}
