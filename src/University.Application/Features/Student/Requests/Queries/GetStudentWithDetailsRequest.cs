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
    public class GetStudentWithDetailsRequest:IRequest<BaseQueryResponse<GetStudentWithDetailsDto>>
    {
        public Guid studentId { get; set; }

    }
}
