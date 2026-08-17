using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Contract;
using University.Domain.Entities.JunctionEntities;

namespace University.Domain.Entities.BaseEntities
{
    public class CreditWork:BaseEntity
    {
        public Guid Id { get; set; }
        public required string Heading { get; set; }
        public required int Code { get; set; }
        public string CreditWorkTitle 
        {
            get
            {
                return $"{Heading} - {Code}";
            }    
        }
        public string? Description { get; set; }

        public List<CreditWorkEnrollment> StudentsInCreditWork { get; set; } = new List<CreditWorkEnrollment>();
        public List<CourseCreditWork> CoursesOfCreditWork { get; set; } = new List<CourseCreditWork>();
    }
}
