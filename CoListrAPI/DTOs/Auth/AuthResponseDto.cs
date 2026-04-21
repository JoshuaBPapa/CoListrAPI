using CoListrAPI.DTOs.User;

namespace CoListrAPI.DTOs.Auth
{
    public class AuthResponseDto
    {
        public required string AccessToken { get; set; }
        public required UserResponseDto User { get; set; }
    }
}
