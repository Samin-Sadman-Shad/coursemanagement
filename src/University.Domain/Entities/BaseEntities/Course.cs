using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Contract;
using University.Domain.Entities.JunctionEntities;

namespace University.Domain.Entities.BaseEntities
{
    public class Course: BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }

        public List<CourseCreditWork> CreditWorksInCourse { get; set; }
        public List<CourseEnrollment> StudentsInCourse { get; set; }
    }
}
