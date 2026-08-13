using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Domain.Entities.JunctionEntities
{
    public class CourseCreditWork
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public Guid CreditWorkId { get; set; }
        public CreditWork CreditWork { get; set; }
    }
}
