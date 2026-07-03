using Abysalto.API.DTOs;

namespace Abysalto.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly HttpClient _httpClient;
        public AuthRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserLoginResponseDto> Login(string username, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", new
            {
                username,
                password
            });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();

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
            response.EnsureSuccessStatusCode();
            var content=await response.Content.ReadFromJsonAsync<UserLoginResponseDto>();
            return new UserLoginResponseDto
            {
                AccessToken = content?.AccessToken,
                RefreshToken = content?.RefreshToken
            };
        }
    }
}
