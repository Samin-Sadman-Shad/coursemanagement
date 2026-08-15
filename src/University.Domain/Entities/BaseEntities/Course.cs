using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Contract;

namespace University.Domain.Entities.BaseEntities
{
    public class Course: BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }

        public List<CreditWork> CreditWorksInCourse { get; set; }
        public List<Student> StudentsInCourse { get; set; }
    }
}
