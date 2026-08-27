using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.DTOs;
using University.Application.Models.DTOs.Common;

namespace University.Application.Models.DTOs.StudentDTOs
{
    public class UpdateStudentEmailDto:BaseUpdateDto
    {
        //public Guid StudentId { get; set; }
        public required string Email { get; set; }
    }
}
