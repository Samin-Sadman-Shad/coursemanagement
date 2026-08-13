using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;
using University.Domain.Entities.Common;

namespace University.Domain.Entities.JunctionEntities
{
    //many to many between student and course
    public class CourseEnrollment : Enroller
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

    }
}
