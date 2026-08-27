using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.Common;
using University.Domain.Entities.Contract;

namespace University.Domain.Entities.JunctionEntities
{
    public class CreditWorkEnrollment : Enroller
    {
        public Guid Id { get; set; } 
        public Guid CreditWorkId { get; set; }
        public required CreditWork CreditWork { get; set; }
        public Guid StudentId { get; set; }
        public required Student Student { get; set; }

    }
}
