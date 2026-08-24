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
        private List<string> _errors = new List<string>();
        public List<string> Errors
        {
            get
            {
                if (IsSuccessful)
                {
                    throw new InvalidOperationException("No Registration error for successful registration");
                }
                else
                {
                    return _errors;
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
                    _errors = value;
                }
            }
        }

    }
}
