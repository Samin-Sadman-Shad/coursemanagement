using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using University.Application.Features.Student.Requests.Commands;
using University.Application.Features.Student.Requests.Queries;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Identity;
using University.Application.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<StudentController>
        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpGet]
        public async Task<ActionResult< BaseQueryListResponse<GetStudentDto>>> Get()
        {
            var response = await _mediator.Send(new GetStudentListRequest());
            return ToActionResult(response);           
            
        }

        // GET api/<StudentController>/5
        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<BaseQueryResponse<GetStudentWithDetailsDto>>> Get(Guid id)
        {
            var request = new GetStudentWithDetailsRequest { StudentId = id };
            var response = await _mediator.Send(request);
            return ToActionResult(response);

        }

        [Authorize(Roles = nameof(RoleEnum.STUDENT))]
        [HttpGet("/me/peers")]
        public async Task<ActionResult<BaseQueryListResponse<GetStudentDto>>> GetPeers()
        {
            var request = new GetPeersByStudentIdRequest();
            var response = await _mediator.Send(request);
            return ToActionResult(response);
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpGet("name")]
        public async Task<ActionResult<BaseQueryListResponse<GetStudentDto>>> GetByName([FromQuery]string name)
        {
            var request = new GetStudentsByNameRequest { SerachName = name };
            var response = await _mediator.Send(request);
            return ToActionResult(response);

        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpGet("roll")]
        public async Task<ActionResult<BaseQueryResponse<GetStudentWithDetailsDto>>> GetByRoll([FromQuery]int roll)
        {
            var request = new GetStudentByPersonalInfoRequest { Roll = roll, Email = null };
            var response = await _mediator.Send(request);
            return ToActionResult(response);
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpGet("email")]
        public async Task<ActionResult<BaseQueryResponse<GetStudentWithDetailsDto>>> GetByEmail([FromQuery] string email)
        {
            var request = new GetStudentByPersonalInfoRequest { Roll = null, Email = email };

            var response = await _mediator.Send(request);

            return ToActionResult(response);
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpGet("class")]
        public async Task<ActionResult<BaseQueryListResponse<GetStudentDto>>> GetByCreditWorkId([FromQuery] Guid creditWorkId)
        {
            var request = new GetStudentsByCreditWorkIdRequest { CreditWorkId = creditWorkId };
            var response = await _mediator.Send(request);

            return ToActionResult(response);
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpGet("course")]
        public async Task<ActionResult<BaseQueryListResponse<GetStudentDto>>> GetByCourseId([FromQuery] Guid courseId)
        {
            var request = new GetStudentsByCourseIdRequest { CourseId = courseId };
            var response = new BaseQueryListResponse<GetStudentDto>();
            response = await _mediator.Send(request);

            return ToActionResult(response);
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        // POST api/<StudentController>
        [HttpPost]
        public async Task<ActionResult<CreateCommandResponse<GetStudentDto>>> Post([FromBody] CreateStudentDto studentDto)
        {
            var command = new CreateStudentCommand { CreateStudentDto = studentDto };
            var response = new CreateCommandResponse<GetStudentDto>();
            response = await _mediator.Send(command);

            return ToActionResult(response);
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        // PUT api/<StudentController>/5
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> Put(Guid id, [FromBody] UpdateStudentDto studentDto)
        {
            var command = new UpdateStudentCommand 
            { 
                StudentId = id,
                UpdateStudentDto = studentDto 
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpPatch("email/{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdateEmail([FromRoute]Guid id, [FromBody] UpdateStudentEmailDto studentDto)
        {
            var command = new UpdateStudentEmailCommand
            {
                StudentId = id,
                StudentEmailDto = studentDto
            };

            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpPatch("name/{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdateName([FromRoute]Guid id, [FromBody]UpdateStudentNameDto studentDto)
        {

            var command = new UpdateStudentNameCommand
            {
                StudentId = id,
                StudentNameDto = studentDto
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        [HttpPatch("roll/{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdateRoll([FromRoute]Guid Id, [FromBody]UpdateStudentRollDto studentDto)
        {
            var command = new UpdateStudentRollCommand
            {
                StudentId = Id,
                StudentRollDto = studentDto
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);           
        }

        [Authorize(Roles = nameof(RoleEnum.STAFF))]
        // DELETE api/<StudentController>/5
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> Delete(Guid id)
        {
            var command = new DeleteStudentCommand { StudentId=id };
            

            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }
    }
}
