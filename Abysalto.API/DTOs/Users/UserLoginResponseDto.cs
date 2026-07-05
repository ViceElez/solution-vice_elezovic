namespace Abysalto.API.DTOs.Users
{
    public class UserLoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
