using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Contracts.DTOs
{
    public interface ICreateDto:IBaseDto
    {
        public  Staff CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
