using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs
{
    public class CourseCreditWorkUnregistrationDto:CourseCreditWorkRegistrationDto
    {
        public Guid RegistrationId { get; set; }
    }
}
