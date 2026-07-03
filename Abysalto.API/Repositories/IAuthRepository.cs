using Abysalto.API.DTOs;
using Abysalto.API.Models;

namespace Abysalto.API.Repositories
{
    public interface IAuthRepository
    {
        Task<UserLoginResponseDto> Login(string username, string password);

        Task<UserLoginResponseDto> RefreshToken(string refreshToken);

    }
}
