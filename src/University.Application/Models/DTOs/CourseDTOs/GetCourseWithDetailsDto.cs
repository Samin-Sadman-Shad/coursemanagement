using University.Application.Contracts.DTOs;
using University.Application.Models.DTOs.Common;
using University.Application.Models.DTOs.CreditWorkDTOs;

namespace University.Application.Models.DTOs.CourseDTOs
{
    public class GetCourseWithDetailsDto : GetCourseDto
    {
        public List<GetCreditWorkDto> CreditWorks { get; set; } = new List<GetCreditWorkDto>();
    }
}
