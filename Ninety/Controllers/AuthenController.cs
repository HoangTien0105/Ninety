using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ninety.Business.Services;
using Ninety.Business.Services.Interfaces;
using Ninety.Models.DTOs.Request;

namespace Ninety.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/auth")]
    [ApiVersionNeutral]
    public class AuthenController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            var user = await _userService.CheckLogin(loginRequest.Username, loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password!");
            }
            else
            {
                string accessToken = JWTGenerator.GenerateToken(user);
                return Ok(new
                {
                    AccountId = user.Id,
                    FullName = user.Name,
                    Role = user.Role,
                    AccountDTO = user,
                    accessToken = accessToken
                });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return Task.FromResult<IActionResult>(BadRequest(new { message = "Invalid authorization header" }));
            }

            string token = authorizationHeader.Substring("Bearer ".Length).Trim();

            JWTGenerator.InvalidateToken(token);

            return Task.FromResult<IActionResult>(Ok(new { message = "Logout successfully" }));
        }
    }
}
