using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Contract;
using University.Domain.Entities.Interfaces;
using University.Domain.Entities.JunctionEntities;

namespace University.Domain.Entities.BaseEntities
{
    public class Student: BaseEntity, IUser
    {
        [Key]
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public required int Roll { get; set; }
        [EmailAddress]
        public required string Email { get; set; } 

        public List<CourseEnrollment> CoursesEnrolled { get; set; } = new List<CourseEnrollment>();
        public List<CreditWorkEnrollment> CreditWorksEnrolled { get; set; } = new List<CreditWorkEnrollment>();
    }
}
