using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace University.Domain.Entities.BaseEntities
{
    public class CreditWork
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
    }
}
