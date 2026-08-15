using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Interfaces;

namespace University.Domain.Entities.BaseEntities
{
    public class Staff : User
    {
        public Guid UserId { get; set; }
    }
}
