using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.StudentDTOs
{
    public class UpdateStudentDto : IStudentDto
    {
        public string Name { get ; set ; }
        public int Roll { get  ; set  ; }
        public string? Email { get ; set ; }
        public Staff ModifiedBy { get; set; }
        public DateTimeOffset LastModifiedAt { get; set; }
    }
}
