using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.Common
{
    public class BaseCreateDto
    {
        public required Staff CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
