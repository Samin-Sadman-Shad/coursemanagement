using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Domain.Entities.Common
{
    public class Enroller: BaseEntity
    {
        public DateTimeOffset EnrolledAt { get; set; }
        public required Guid StaffId { get; set; }
        [ForeignKey(nameof(StaffId))]
        public required Staff EnrolledBy { get; set; }
    }
}
