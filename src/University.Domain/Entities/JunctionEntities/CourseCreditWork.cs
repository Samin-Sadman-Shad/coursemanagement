using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.Contract;

namespace University.Domain.Entities.JunctionEntities
{
    public class CourseCreditWork
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public required Course Course { get; set; }
        public Guid CreditWorkId { get; set; }
        public required CreditWork CreditWork { get; set; }
    }
}
