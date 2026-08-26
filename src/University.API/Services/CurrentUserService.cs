using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using University.Application.Contracts.API;

namespace University.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid UserId
        {
            get
            {
                //var value= _httpContextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var value = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(value, out Guid userId))
                {
                    throw new UnauthorizedAccessException(
                    "Authenticated user ID is not available.");
                }
                return userId;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

        public bool IsInRole(string role)
        {
           return _httpContextAccessor.HttpContext?.User.IsInRole(role) == true;
        }
    }
}
