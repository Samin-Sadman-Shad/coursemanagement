using MediatR;
using Microsoft.AspNetCore.Mvc;
using University.Application.Features.CourseCreditWorkRegistration.Requests.Commands;
using University.Application.Models.DTOs.CourseCreditWorkRegistrationDTOs;
using University.Application.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.API.Controllers
{
    [Route("api/course-to-class-registration")]
    [ApiController]
    public class CourseCreditWorkRegistrationController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public CourseCreditWorkRegistrationController(IMediator mediator)
        {
            _mediator = mediator;
        } 

        [HttpPost("register")]
        public async Task<ActionResult<BaseCommandResponse>> RegisterCreditWorkToCourse(
            [FromBody] CourseCreditWorkRegistrationDto courseCreditWorkDto)
        {
            var command = new RegisterCourseToCreditWorkCommand
            {
                courseCreditWorkDto = courseCreditWorkDto
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [HttpDelete("unregister/{courseCreditWorkId:guid}")]
        public async Task<ActionResult<BaseCommandResponse>> UnregisterCreditWorkFromCourse(Guid courseCreditWorkId)
        {
            var command = new UnregisterCourseToCreditWorkCommand
            {
                CourseCreditWorkId = courseCreditWorkId
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }
    }
}
