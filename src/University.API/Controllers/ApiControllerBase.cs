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

        protected ActionResult<TResponse> ToActionResult<TResponse>(TResponse response)
        where TResponse : IBaseResponse
        {
            return response.Status switch
            {
                HttpStatusCode.OK => Ok(response),
                HttpStatusCode.Created => StatusCode((int)HttpStatusCode.Created, response),
                HttpStatusCode.NoContent => StatusCode((int)HttpStatusCode.NoContent, response),
                HttpStatusCode.BadRequest => BadRequest(response),
                HttpStatusCode.Unauthorized => Unauthorized(response),
                HttpStatusCode.Forbidden => StatusCode((int)HttpStatusCode.Forbidden, response),
                HttpStatusCode.NotFound => NotFound(response),
                HttpStatusCode.InternalServerError => StatusCode((int)HttpStatusCode.InternalServerError, response),
                _ => StatusCode((int)HttpStatusCode.BadGateway, response) // Default to 502 Bad Gateway
            };
        }
    }
}
