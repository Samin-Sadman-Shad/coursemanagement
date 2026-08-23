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

        public async Task<ActionResult<LogoutResponse>> Logout()
        {
            return Ok(await _authService.Logout());
        }

        // GET: api/<AuthController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AuthController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuthController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
