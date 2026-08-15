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
        //value is CLR default,
        //when entity is tracked, EF will generate and assign sequential guid
        public Guid Id { get; private set; } 
        public Guid CreditWorkId { get; set; }
        public CreditWork CreditWork { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

    }
}
