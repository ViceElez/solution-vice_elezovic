using Abysalto.API.DTOs.Users;
using System.Net;
using System.Net.Http.Headers;

namespace Abysalto.API.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthRepository> _logger;
        public AuthRepository(HttpClient httpClient, ILogger<AuthRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<UserLoginResponseDto> Login(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", new
            {
                username,
                password
            });
            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode==HttpStatusCode.BadRequest)
            {
                _logger.LogWarning("User not found");
                return null;
            }
            Console.WriteLine(response.StatusCode);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            _logger.LogInformation("User logged in successfully");
            return  new UserLoginResponseDto
            {
                AccessToken = content?.AccessToken,
                RefreshToken = content?.RefreshToken
            };
        }

        public async Task<UserLoginResponseDto> RefreshToken(string refreshToken)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/refresh", new
            {
                refreshToken
            });
            if(response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _logger.LogWarning("Refresh token is invalid or expired");
                return null;
            }
            response.EnsureSuccessStatusCode();
            var content=await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            _logger.LogInformation("Token refreshed successfully");
            return new UserLoginResponseDto
            {
                AccessToken = content?.AccessToken,
                RefreshToken = content?.RefreshToken
            };  
        }

        public async Task<bool> ValidateToken(string accessToken)
        { 
            var request= new HttpRequestMessage(HttpMethod.Get, "auth/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Token is invalid or expired");
                return false;
            }
            _logger.LogInformation("Token validated successfully");
            return true;
        }
    }
}
