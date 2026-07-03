using Abysalto.API.DTOs;

namespace Abysalto.API.Repositories
{
    public interface IAuthRepository
    {
        Task<UserLoginResponseDto> Login(string username, string password);

        Task<UserLoginResponseDto> RefreshToken(string refreshToken);

        Task<bool> ValidateToken(string accessToken);

    }
}
