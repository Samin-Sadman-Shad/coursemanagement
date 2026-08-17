using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.Common;

namespace University.Application.Models.DTOs.StudentDTOs
{
    public interface IStudentDto:IBaseDto
    {
        public string Name { get; set; }
        public int Roll { get; set; }    
        public string? Email { get; set; }
    }
}
