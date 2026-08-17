using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.Interfaces;

namespace University.Domain.Entities.BaseEntities
{
    public class Staff : IUser
    {
        [Key]
        public Guid UserId { get; set; }
    }
}
