using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Contract;
using University.Domain.Entities.Interfaces;

namespace University.Domain.Entities.BaseEntities
{
    public class Student: BaseEntity, IUser
    {
        [Key]
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public int Roll { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        public List<Course> Courses { get; set; }
        public List<CreditWork> CreditWorks { get; set; }


    }
}
