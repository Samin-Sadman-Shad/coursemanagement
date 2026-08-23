using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.Identity
{
    public class RegistrationResponse
    {
        public Guid UserId { get; set; }
        public bool IsSuccessful { get; set; }
        private string? _error;
        public string? Error
        {
            get
            {
                if (IsSuccessful)
                {
                    throw new InvalidOperationException("No Registration error for successful registration");
                }
                else
                {
                    return _error;
                }
            }
            set
            {
                if (IsSuccessful)
                {
                    throw new InvalidOperationException("No Registration error for successful registration");
                }
                else
                {
                    _error = value;
                }
            }
        }

    }
}
