using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.Features.Course.Requests.Commands;
using University.Application.Features.Course.Requests.Queries;
using University.Application.Models.DTOs.CourseDTOs;
using University.Application.Models.Identity;
using University.Application.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =nameof( RoleEnum.STAFF))]
    public class CourseController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<CourseController>
        [HttpGet]
        public async Task<ActionResult<BaseQueryListResponse<GetCourseDto>>>  Get()
        {
            var request = new GetCourseListRequest();
            var response = await _mediator.Send(request);
            return ToActionResult(response);
        }

        // GET api/<CourseController>/5
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<BaseQueryResponse<GetCourseWithDetailsDto>>> Get(Guid id)
        {
            var request = new GetCourseWithDetailsRequest { CourseId = id };
            var response = await _mediator.Send(request);
            return ToActionResult(response);
        }

        [HttpGet("class")]
        public async Task<ActionResult<BaseQueryListResponse<GetCourseDto>>> GetByCreditWorkId([FromQuery]Guid creditWorkId)
        {
            var request = new GetCourseListByCreditWorkIdRequest { CreditWorkId = creditWorkId };
            var response = await _mediator.Send(request);
            return ToActionResult(response);
        }

        [HttpGet("student")]
        public async Task<ActionResult<BaseQueryListResponse<GetCourseDto>>> GetByStudentId([FromQuery]Guid studentId)
        {
            var request = new GetCourseListByStudentIdRequest { StudentId = studentId };
            var response = await _mediator.Send(request);
            return ToActionResult(response);
        }

        // POST api/<CourseController>
        [HttpPost]
        public async Task<ActionResult<CreateCommandResponse<GetCourseDto>>> Post([FromBody] CreateCourseDto courseDto)
        {
            var command = new CreateCourseCommand { CreateCourseDto = courseDto };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        // PUT api/<CourseController>/5
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> Put(Guid id, [FromBody] UpdateCourseTitleDto courseDto)
        {
            var command = new UpdateCourseTitleCommand { CourseId = id, UpdateCourseTitleDto = courseDto };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        // DELETE api/<CourseController>/5
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> Delete(Guid id)
        {
            var command = new DeleteCourseCommand { CourseId = id };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }
    }
}
