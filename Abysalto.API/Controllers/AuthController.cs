using Abysalto.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Abysalto.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var loginResponse = await _authService.Login(username, password);
                if (loginResponse == null)
                    return Unauthorized(new { message = "Invalid username or password." });
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            try
            {
                var refreshResponse = await _authService.RefreshToken(refreshToken);
                if (refreshResponse == null)
                    return Unauthorized(new { message = "Refresh token is invalid or expired." });
                return Ok(refreshResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateToken(string accessToken)
        {
            var isValid = await _authService.ValidateToken(accessToken);

            if (!isValid)
                return Unauthorized(new { message = "Token is invalid or expired." });

            return Ok(new { message = "Token is valid." });
        }
    }
}
