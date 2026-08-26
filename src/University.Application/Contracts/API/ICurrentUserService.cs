using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Contracts.API
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}
