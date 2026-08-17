using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Contracts.DTOs
{
    public interface IStudentDto:IBaseDto
    {
        public string Name { get; set; }
        public int Roll { get; set; }    
        public string? Email { get; set; }
    }
}
