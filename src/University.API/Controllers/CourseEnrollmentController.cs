using MediatR;
using Microsoft.AspNetCore.Mvc;
using University.Application.Features.CourseEnrollment.Requests.Commands;
using University.Application.Features.CourseEnrollment.Requests.Queries;
using University.Application.Models.DTOs.CourseEnrollmentDTOs;
using University.Application.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.API.Controllers
{
    [Route("api/enroll-to-course")]
    [ApiController]
    public class CourseEnrollmentController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public CourseEnrollmentController(IMediator mediator)
        {
            _mediator=mediator;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<BaseQueryResponse<GetCourseEnrollmentDto>>> GetEnrollmentById(Guid id)
        {
            var command = new GetCourseEnrollmentRequest
            {
                CourseEnrollmentId = id
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [HttpPost("enroll")]
        public async Task<ActionResult<CreateCommandResponse<GetCourseEnrollmentDto>>> EnrollStudentInCourse(
                [FromBody] CreateCourseEnrollmentDto courseEnrollmentDto)
        {
            var command = new CreateCourseEnrollmentCommand
            {
                CourseEnrollmentDto = courseEnrollmentDto
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [HttpDelete("{courseEnrollmentId:guid}")]
        public async Task<ActionResult<BaseCommandResponse>> RemoveCourseEnrollment(Guid courseEnrollmentId)
        {
            var command = new RemoveCourseEnrollmentCommand
            {
                CourseEnrollmentId = courseEnrollmentId
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }
    }
}
