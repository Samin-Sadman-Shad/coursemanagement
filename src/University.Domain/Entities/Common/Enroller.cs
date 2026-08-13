using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Domain.Entities.Common
{
    public class Enroller
    {
        public DateTimeOffset EnrolledAt { get; set; }
        public Guid StaffId { get; set; }
        public Staff EnrolledBy { get; set; }
    }
}
