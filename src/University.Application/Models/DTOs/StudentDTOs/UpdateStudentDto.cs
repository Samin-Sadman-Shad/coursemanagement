using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Models.DTOs.Common;
using University.Domain.Entities.BaseEntities;

namespace University.Application.Models.DTOs.StudentDTOs
{
    public class UpdateStudentDto : BaseUpdateDto, IStudentDto
    {
        public required string Name { get; set; } 
        public required int Roll { get  ; set  ; }
        public string? Email { get ; set ; }
    }
}
