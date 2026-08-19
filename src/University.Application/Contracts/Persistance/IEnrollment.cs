using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Contracts.Persistance
{
    public interface IEnrollment
    {
        Task<bool> DoesEnrollmentExist(Guid enrollmentId);
    }
}
