using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.Identity
{
    public class AuthResponse
    {
        public Guid Id { get; set; } 
        public string? UserName { get; set; } 
        public string? Email { get; set; } 
        public string Token { get; set; } = string.Empty;
        public bool IsRegistered { get; set; }
        public bool? IsAllowedToLogin { get; set; }

        public string? AuthError { get; set; }
    } 
}
