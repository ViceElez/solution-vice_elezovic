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
            var loginResponse = await _authService.Login(username, password);
            if (loginResponse == null)
                return NotFound();
            return Ok(loginResponse);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var refreshResponse = await _authService.RefreshToken(refreshToken);
            if (refreshResponse == null)
                return NotFound();
            return Ok(refreshResponse);
        }

        [HttpGet("validate")]
        public async Task<bool> ValidateToken(string accessToken)
        {
            return await _authService.ValidateToken(accessToken);
        }
    }
}
