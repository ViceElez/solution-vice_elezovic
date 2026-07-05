using Abysalto.API.DTOs.Users;

namespace Abysalto.API.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<UserLoginResponseDto> Login(string username, string password);

        Task<UserLoginResponseDto> RefreshToken(string refreshToken);

        Task<bool> ValidateToken(string accessToken);

    }
}
