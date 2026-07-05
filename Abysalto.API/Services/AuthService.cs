using Abysalto.API.DTOs.Users;
using Abysalto.API.Repositories.Auth;

namespace Abysalto.API.Services
{
    public class AuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<UserLoginResponseDto> Login(string username, string password)
        {
            return await _authRepository.Login(username, password);
        }

        public async Task<UserLoginResponseDto> RefreshToken(string refreshToken)
        {
            return await _authRepository.RefreshToken(refreshToken);
        }

        public async Task<bool> ValidateToken(string accessToken)
        {
            return await _authRepository.ValidateToken(accessToken);
        }
    }
}
