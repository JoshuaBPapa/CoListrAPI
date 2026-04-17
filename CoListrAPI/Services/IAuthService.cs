using CoListrAPI.Domain.Auth;
using CoListrAPI.Domain.Entities;
using CoListrAPI.Domain.Result;
using CoListrAPI.DTOs.Auth;

namespace CoListrAPI.Services
{
    public interface IAuthService
    {
        Task<Result<TokenPair>> SignUpAsync(AuthSignUpDto dto, CancellationToken cancellationToken);
        Task<Result<TokenPair>> LoginAsync(AuthLoginDto dto, CancellationToken cancellationToken);
        string HashPassword(User user, string password);
        bool VerifyHashedPassword(User user, string providedPassword);
    }
}
