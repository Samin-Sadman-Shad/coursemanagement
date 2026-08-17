using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Contracts.DTOs
{
    public interface IUpdateDto : IBaseDto
    {
        public Staff LastModifiedBy { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }
    }
}
