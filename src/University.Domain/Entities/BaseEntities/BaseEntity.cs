using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Contract;

namespace University.Domain.Entities.BaseEntities
{
    public class BaseEntity: IBaseEntity
    {
        public Guid CreatedById { get; set; }
        //[ForeignKey(nameof(CreadtedById))]
        //public required Staff CreatedBy { get; set; }
        public Guid LastModifiedById { get; set; }
        //public  Staff? LastModifiedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }
    }
}
