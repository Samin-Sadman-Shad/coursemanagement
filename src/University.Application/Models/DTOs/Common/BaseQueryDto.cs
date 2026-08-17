using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.DTOs.Common
{
    public class BaseQueryDto:IBaseDto
    {
        public Guid Id { get; set; }
    }
}
