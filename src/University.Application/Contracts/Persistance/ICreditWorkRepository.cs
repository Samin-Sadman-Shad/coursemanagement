using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Contracts.Persistance
{
    public interface ICreditWorkRepository: IGenericRepository<CreditWork>
    {
        //all the classes of a given course
        Task<List<CreditWork>> GetCreditWorksByCourseIdAsync(Guid courseId);

        //student request for own credit works
        Task<List<CreditWork>> GetCreditWorksByStudentIdAsync(Guid studentId);

        Task<CreditWork> UpdateCreditWorkCode(CreditWork creditWork, int updatedCode);
        Task<CreditWork> UpdateCreditWorkHeading(CreditWork creditWorkHeading, string updatedHeading);
        Task<CreditWork> UpdateCreditWorkDescription(CreditWork creditWork, string description);

        Task<bool> DoesCreditWorkTitleExistAsync(string heading, int code, Guid? excludeId = null);

        Task<string> GetCreditWorkHeading(Guid creditWorkId);
        Task<string> GetCreditWorkCode(Guid creditWorkId);
    }
}
