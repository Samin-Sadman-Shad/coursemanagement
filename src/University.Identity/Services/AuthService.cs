using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using University.Application.Contracts.Identity;
using University.Application.Models.Identity;
using University.Identity.Models;

namespace University.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettingsOptions _jwtSettings;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtSettingsOptions jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings= jwtSettings;
        }
        public async Task<AuthResponse> Login(AuthRequest request)
        {
            var response = new AuthResponse();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                response.IsRegistered = false;
                response.AuthError = "No user with this email has registered";
                return response;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
             if (result.Succeeded)
            {
                var jwt = await GenerateTokenAsync(user);
                response.Id = user.Id;
                response.UserName = user.UserName;
                response.Email = user.Email;
                response.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
                response.IsRegistered = true;
                response.IsAllowedToLogin = true;

                return response;
            }
            else if (result.IsNotAllowed)
            {
                response.IsAllowedToLogin = false;
                response.AuthError = "User is not allowed";
            }
            else if (result.IsLockedOut)
            {
                response.IsAllowedToLogin = false;
                response.AuthError = "User is not allowed";
            }
            return response;
            
        }

        public Task<LogoutResponse> Logout()
        {
            return Task.FromResult(new LogoutResponse
            {
                IsSuccessful = true,
                Message = "Logout successful."
            });
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest request)
        {
            var response = new RegistrationResponse();
            var validator = new RegistrationRequestValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.IsSuccessful = false;
                response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return response;
            }
            //var userByName = await _userManager.FindByNameAsync(request.UserName); //only email is unique
            //if(userByName is not null)
            //{
            //    response.IsSuccessful = false;
            //    response.Errors.Add("User with this username already registered");
            //    return response;
            //}

            var userByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userByEmail is not null)
            {
                response.IsSuccessful = false;
                response.Errors.Add("User with this email already registered");
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName ?? request.FirstName + request.LastName,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                response.IsSuccessful = false;
                response.Errors = result.Errors
                    .Select(e => e.Description)
                    .ToList();

                return response;
            }

            var roleResult = await _userManager.AddToRoleAsync(
                user,
                RoleEnum.STAFF.ToString());

            if (!roleResult.Succeeded)
            {
                response.IsSuccessful = false;
                response.Errors = roleResult.Errors
                    .Select(e => e.Description)
                    .ToList();

                return response;
            }

            response.IsSuccessful = true;
            response.Status = System.Net.HttpStatusCode.Created;
            response.UserId = user.Id;

            return response;
        }

        public async Task<SetPasswordResponse> SetPassword(SetPasswordRequest request)
        {
            var response = new SetPasswordResponse();
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
            {
                response.IsSuccessful = false;
                response.Errors.Add("Invalid user or token.");
                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                response.IsSuccessful = false;
                response.Errors = result.Errors.Select(e => e.Description).ToList();
                return response;
            }

            response.IsSuccessful = true;
            return response;
        }

        private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new InvalidOperationException("User username is required.");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new InvalidOperationException("User email is required.");
            //generate user claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach (var role in roles) //add roles from the request
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,  user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }.Union(userClaims).Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


            //generate the token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials: signingCredentials);

            return token;
        }
    }
}
