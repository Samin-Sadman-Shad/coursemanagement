using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.Student.Requests.Queries
{
    public class GetStudentByPersonalInfoRequest:IRequest<BaseQueryResponse<GetStudentWithDetailsDto>>
    {
        public int? Roll { get; set; }
        public string? Email { get; set; }
    }
}
