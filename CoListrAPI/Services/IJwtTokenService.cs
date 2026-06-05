using CoListrAPI.Domain.Auth;

namespace CoListrAPI.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, TokenType tokenType);
        TokenPair GenerateTokenPair(string userId);
        string GetUserIdFromToken(string jwt);
        bool CheckIfTokenValid(string token);
        string GenerateAccessTokenFromRefreshToken(string refreshToken);
    }
}
