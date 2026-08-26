using MediatR;
using Microsoft.AspNetCore.Mvc;
using University.Application.Features.CourseEnrollment.Requests.Queries;
using University.Application.Features.CreditWorkEnrollment.Requests.Commands;
using University.Application.Features.CreditWorkEnrollment.Requests.Requests;
using University.Application.Models.DTOs.CourseEnrollmentDTOs;
using University.Application.Models.DTOs.CreditWorkEnrollmentDto;
using University.Application.Models.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.API.Controllers
{
    [Route("api/enroll-to-class")]
    [ApiController]
    public class CreditWorkEnrollmentController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public CreditWorkEnrollmentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<BaseQueryResponse<GetCreditWorkEnrollmentDto>>> GetEnrollmentById(Guid id)
        {
            var command = new GetCreditWorkEnrollmentRequest
            {
                CreditWorkEnrollmentId = id
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [HttpPost]
        public async Task<ActionResult<CreateCommandResponse<GetCreditWorkEnrollmentDto>>> 
            EnrollStudentInCreditWork([FromBody]CreateCreditWorkEnrollmentDto creditWorkEnrollmentDto)
        {
            var command = new CreateCreditWorkEnrollmentCommand { CreditWorkEnrollmentDto = creditWorkEnrollmentDto };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }

        [HttpDelete("{creditWorkEnrollmentId:guid}")]
        public async Task<ActionResult<BaseCommandResponse>> RemoveCreditWorkEnrollment(Guid creditWorkEnrollmentId)
        {
            var command = new RemoveCreditWorkEnrollmentCommand
            {
                CreditWorkEnrollmentId = creditWorkEnrollmentId
            };
            var response = await _mediator.Send(command);
            return ToActionResult(response);
        }
    }
}
