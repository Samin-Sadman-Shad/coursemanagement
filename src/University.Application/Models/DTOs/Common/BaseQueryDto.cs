using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;

namespace University.Application.Models.DTOs.Common
{
    public class BaseQueryDto: IQueryDto
    {
        public Guid Id { get; set; }
    }
}
