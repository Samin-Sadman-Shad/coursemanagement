using Microsoft.AspNetCore.Mvc;
using System.Net;
using University.Application.Models.Responses;

namespace University.API.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase:ControllerBase
    {
        protected ActionResult<BaseCommandResponse> ToActionResult(BaseCommandResponse response)
        {
            return response.Status switch
            {
                HttpStatusCode.Created => StatusCode((int)HttpStatusCode.Created, response),
                HttpStatusCode.NoContent => StatusCode((int)HttpStatusCode.NoContent, response),
                HttpStatusCode.NotFound => NotFound(response),
                HttpStatusCode.BadRequest => BadRequest(response),
                _ => Ok(response)
            };
        }
    }
}
