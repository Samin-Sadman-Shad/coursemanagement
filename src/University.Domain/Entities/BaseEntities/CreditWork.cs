using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Contract;

namespace University.Domain.Entities.BaseEntities
{
    public class CreditWork:BaseEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Code { get; set; }
        public string CreditWorkName 
        {
            get
            {
                return $"{Title} - {Code}";
            }    
        }
        public string? Description { get; set; }

        public List<Student> StudentsInCreditWork { get; set; }
        public List<Course> CoursesOfCreditWork { get; set; }
    }
}
