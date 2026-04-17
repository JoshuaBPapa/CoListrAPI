using CoListrAPI.Domain.Auth;

namespace CoListrAPI.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, TokenType tokenType);
        TokenPair GenerateTokenPair(string userId);
    }
}
