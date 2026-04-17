using CoListrAPI.Data;
using CoListrAPI.Domain.Auth;
using CoListrAPI.Domain.Entities;
using CoListrAPI.Domain.Result;
using CoListrAPI.DTOs.Auth;
using Microsoft.AspNetCore.Identity;

namespace CoListrAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IUserRepository users, IJwtTokenService jwtTokenService, IPasswordHasher<User> passwordHasher)
        {
            _users = users;
            _jwtTokenService = jwtTokenService;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<TokenPair>> SignUpAsync(AuthSignUpDto dto, CancellationToken cancellationToken)
        {
            var existingUser = await _users.GetByUsernameAsync(dto.Username, cancellationToken);
            if (existingUser != null)
            {
                return new Result<TokenPair>
                {
                    IsSuccess = false,
                    Value = null,
                    Error = "Username already taken"
                };
            }

            int shareCode = new Random().Next(10000, 99999);
            var shareCodeExists = await _users.GetByShareCodeAsync(shareCode, cancellationToken);
            while (shareCodeExists != null)
            {
                shareCode = new Random().Next(10000, 99999);
                shareCodeExists = await _users.GetByShareCodeAsync(shareCode, cancellationToken);
            }

            var user = new User
            { 
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username,
                PasswordHash = "",
                ShareCode = shareCode
            };
            
            user.PasswordHash = HashPassword(user, dto.Password);

            await _users.Add(user, cancellationToken);

            var tokenPair = _jwtTokenService.GenerateTokenPair(user.Id.ToString());

            return new Result<TokenPair>
            {
                IsSuccess = true,
                Value = tokenPair,
                Error = null
            };
        }

        public async Task<Result<TokenPair>> LoginAsync(AuthLoginDto dto, CancellationToken cancellationToken)
        {
            var result = new Result<TokenPair>
            {
                IsSuccess = false,
                Value = null,
                Error = "Invalid username or password"
            };

            var existingUser = await _users.GetByUsernameAsync(dto.Username, cancellationToken);
            if (existingUser == null) return result;

            if (!VerifyHashedPassword(existingUser, dto.Password)) return result;

            var tokenPair = _jwtTokenService.GenerateTokenPair(existingUser.Id.ToString());

            result.IsSuccess = true;
            result.Value = tokenPair;
            result.Error = null;

            return result;
        }

        public string HashPassword(User user, string password) 
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyHashedPassword(User user, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, providedPassword);
            return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
