using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using University.Application.Features.CreditWork.Requests.Commands;
using University.Application.Features.CreditWork.Requests.Queries;
using University.Application.Features.Student.Requests.Commands;
using University.Application.Models.DTOs.CreditWorkDTOs;
using University.Application.Models.DTOs.StudentDTOs;
using University.Application.Models.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditWorkController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CreditWorkController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/<CreditWorkController>
        [HttpGet]
        public async Task<ActionResult<BaseQueryListResponse<GetCreditWorkDto>>> Get()
        {
            var response = new BaseQueryListResponse<GetCreditWorkDto>();
            try
            {
                response = await _mediator.Send(new GetCreditWorkListRequest());
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(50, response);
            }
        }

        // GET api/<CreditWorkController>/5
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<BaseQueryResponse<GetCreditWorkWithDetailsDto>>> Get(Guid id)
        {
            var request = new GetCreditWorkWithDetailsRequest { CreditWorkId = id };
            var response = new BaseQueryResponse<GetCreditWorkWithDetailsDto>();
            try
            {
                response = await _mediator.Send(request);
                if (response is null)
                {
                    return StatusCode(500);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(50, response);
            }
        }

        [HttpGet]
        public async Task<ActionResult<BaseQueryListResponse<GetCreditWorkDto>>> GetByStudentId([FromQuery]Guid studentId)
        {
            var response = new BaseQueryListResponse<GetCreditWorkDto>();
            try
            {
                response = await _mediator.Send(new GetCreditWorksByStudentIdRequest { StudentId= studentId});
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(50, response);
            }
        }

        [HttpGet]
        public async Task<ActionResult<BaseQueryListResponse<GetCreditWorkDto>>> GetByCourseId([FromQuery] Guid courseId)
        {
            var response = new BaseQueryListResponse<GetCreditWorkDto>();
            try
            {
                response = await _mediator.Send(new GetCreditWorksByCourseIdRequest { CourseId = courseId });
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(50, response);
            }
        }

        // POST api/<CreditWorkController>
        [HttpPost]
        public async Task<ActionResult<CreateCommandResponse<GetCreditWorkDto>>> Post([FromBody] CreateCreditWorkDto creditWorkDto)
        {
            var request = new CreateCreditWorkCommand { CreateCreditWorkDto = creditWorkDto };
            var response = new CreateCommandResponse<GetCreditWorkDto>();
            try
            {
                response = await _mediator.Send(request);
                if (response is null || response.Status == HttpStatusCode.BadRequest)
                {
                    return BadRequest(response);
                }
                return CreatedAtAction(nameof(Post), new { id = response.RecordId }, response.Record);
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
        }

        // PUT api/<CreditWorkController>/5
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> Put(Guid id, [FromBody] UpdateCreditWorkDto creditWorkDto)
        {
            var command = new UpdateCreditWorkCommand { CreditWorkId = id , CreditWorkDto=creditWorkDto};
            var response = new BaseCommandResponse();
            try
            {
                response = await _mediator.Send(command);
                if (response is null || response.Status == HttpStatusCode.BadRequest)
                {
                    return BadRequest(response);
                }
                else if (response.Status == HttpStatusCode.NotFound)
                {
                    return NotFound(response);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPatch("code/{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdateCode([FromRoute]Guid id, [FromBody] UpdateCreditWorkCodeDto creditWorkDto)
        {
            var command = new UpdateCreditWorkCodeCommand { CreditWorkId = id, CreditWorkDto = creditWorkDto };
            var response = new BaseCommandResponse();
            try
            {
                response = await _mediator.Send(command);
                if (response is null || response.Status == HttpStatusCode.BadRequest)
                {
                    return BadRequest(response);
                }
                else if (response.Status == HttpStatusCode.NotFound)
                {
                    return NotFound(response);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPatch("heading/{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdateHeading([FromRoute] Guid id, UpdateCreditWorkHeadingDto creditWorkDto)
        {
            var command = new UpdateCreditWorkHeadingCommand { CreditWorkId = id, CreditWorkDto = creditWorkDto };
            var response = new BaseCommandResponse();
            try
            {
                response = await _mediator.Send(command);
                if (response is null || response.Status == HttpStatusCode.BadRequest)
                {
                    return BadRequest(response);
                }
                else if (response.Status == HttpStatusCode.NotFound)
                {
                    return NotFound(response);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
        }

        [HttpPatch("description/{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UpdateDescription([FromRoute] Guid id, UpdateCreditWorkDescriptionDto creditWorkDto)
        {
            var command = new UpdateCreditWorkDescriptionCommand { CreditWorkId = id, CreditWorkDto = creditWorkDto };
            var response = new BaseCommandResponse();
            try
            {
                response = await _mediator.Send(command);
                if (response is null || response.Status == HttpStatusCode.BadRequest)
                {
                    return BadRequest(response);
                }
                else if (response.Status == HttpStatusCode.NotFound)
                {
                    return NotFound(response);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
        }

        // DELETE api/<CreditWorkController>/5
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<BaseCommandResponse>> Delete(Guid id)
        {
            var command = new DeleteCreditWorkCommand { CreditWorkId = id };
            var response = new BaseCommandResponse();
            try
            {
                response = await _mediator.Send(command);
                if (response is null)
                {
                    return BadRequest();
                }
                if (response.Status == HttpStatusCode.NotFound)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
}
