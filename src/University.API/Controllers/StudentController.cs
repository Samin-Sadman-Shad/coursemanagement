using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using University.Application.Features.Student.Requests.Commands;
using University.Application.Features.Student.Requests.Queries;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<StudentController>
        [HttpGet]
        public async Task<ActionResult< BaseQueryListResponse<GetStudentDto>>> Get()
        {
            var response = await _mediator.Send(new GetStudentListRequest());
            return ToActionResult(response);           
            
        }

        // GET api/<StudentController>/5
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<BaseQueryResponse<GetStudentWithDetailsDto>>> Get(Guid id)
        {
            var request = new GetStudentWithDetailsRequest { StudentId = id };
            var response = await _mediator.Send(request);
            return ToActionResult(response);

        }

        [HttpGet("peers/{id:Guid}")]
        public async Task<ActionResult<BaseQueryListResponse<GetStudentDto>>> GetPeers([FromRoute]Guid id)
        {
            var request = new GetPeersByStudentIdRequest { StudentId = id };
            var response = await _mediator.Send(request);
            return ToActionResult(response);
        }

        [HttpGet("name")]
        public async Task<ActionResult<BaseQueryListResponse<GetStudentDto>>> GetByName([FromQuery]string name)
        {
            var request = new GetStudentsByNameRequest { SerachName = name };
            var response = new BaseQueryListResponse<GetStudentDto>();
            response = await _mediator.Send(request);
            return ToActionResult(response);

        }

        [HttpGet("roll")]
        public async Task<ActionResult<BaseQueryResponse<GetStudentWithDetailsDto>>> GetByRoll([FromQuery]int roll)
        {
            var request = new GetStudentByPersonalInfoRequest { Roll = roll, Email = null };
            var response = new BaseQueryResponse<GetStudentWithDetailsDto>();
            response = await _mediator.Send(request);
            return ToActionResult(response);
        }

        [HttpGet("email")]
        public async Task<ActionResult<BaseQueryResponse<GetStudentWithDetailsDto>>> GetByEmail([FromQuery] string email)
        {
            var request = new GetStudentByPersonalInfoRequest { Roll = null, Email = email };
            var response = new BaseQueryResponse<GetStudentWithDetailsDto>();

            response = await _mediator.Send(request);

            return ToActionResult(response);
        }

        // POST api/<StudentController>
        [HttpPost]
        public async Task<ActionResult<CreateCommandResponse<GetStudentDto>>> Post([FromBody] CreateStudentDto studentDto)
        {
            var command = new CreateStudentCommand { CreateStudentDto = studentDto };
            var response = new CreateCommandResponse<GetStudentDto>();
            response = await _mediator.Send(command);

            return ToActionResult(response);
        }

        // PUT api/<StudentController>/5
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> Put(Guid id, [FromBody] UpdateStudentDto studentDto)
        {
            var command = new UpdateStudentCommand 
            { 
                StudentId = id,
                UpdateStudentDto = studentDto 
            };
            var response = new BaseCommandResponse();
            response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [HttpPatch("email/{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdateEmail([FromRoute]Guid id, [FromBody] UpdateStudentEmailDto studentDto)
        {
            var command = new UpdateStudentEmailCommand
            {
                StudentId = id,
                StudentEmailDto = studentDto
            };
            var response = new BaseCommandResponse();

            response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [HttpPatch("name/{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdateName([FromRoute]Guid id, [FromBody]UpdateStudentNameDto studentDto)
        {

            var command = new UpdateStudentNameCommand
            {
                StudentId = id,
                StudentNameDto = studentDto
            };
            var response = new BaseCommandResponse();
            response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [HttpPatch("roll/{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdateRoll([FromRoute]Guid Id, [FromBody]UpdateStudentRollDto studentDto)
        {
            var command = new UpdateStudentRollCommand
            {
                StudentId = Id,
                StudentRollDto = studentDto
            };
            var response = new BaseCommandResponse();
            response = await _mediator.Send(command);
            return ToActionResult(response);           
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> Delete(Guid id)
        {
            var command = new DeleteStudentCommand { StudentId=id };
            var response = new BaseCommandResponse();

            response = await _mediator.Send(command);
            return ToActionResult(response);
        }
    }
}
