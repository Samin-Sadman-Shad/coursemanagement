using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using University.Application.Contracts.Identity;
using University.Application.Models.Identity;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
        {
            var response = await _authService.Login(request);
            if ( !response.IsRegistered)
            {
                return BadRequest(response);
            }
            if (response.IsAllowedToLogin == false)
            {
                return BadRequest(response);
            }
            return Ok(await _authService.Login(request));
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest request)
        {
            var response = await _authService.Register(request);
            if (!response.IsSuccessful)
            {
                return BadRequest(response);
            }
            return Ok(await _authService.Register(request));
        }

        [HttpPost("logout")]
        public async Task<ActionResult<LogoutResponse>> Logout()
        {
            return Ok(await _authService.Logout());
        }

        [HttpPost("set-password")]
        public async Task<ActionResult<SetPasswordResponse>> SetPassword([FromBody] SetPasswordRequest request)
        {
            var response = await _authService.SetPassword(request);
            return response.IsSuccessful ? Ok(response) : BadRequest(response);
        }
    }
}
