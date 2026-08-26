using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.Models.DTOs.CourseEnrollmentDTOs;
using University.Application.Models.Responses;

namespace University.Application.Features.CourseEnrollment.Requests.Queries
{
    public class GetCourseEnrollmentRequest:IRequest<BaseQueryResponse<GetCourseEnrollmentDto>>
    {
        public Guid CourseEnrollmentId {  get; set; }
    }
}
