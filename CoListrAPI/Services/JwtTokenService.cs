using CoListrAPI.Domain.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoListrAPI.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public string GenerateToken(string userId, TokenType tokenType)
        {
            DateTime expires; 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };

            if (tokenType == TokenType.RefreshToken)
            {
                expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDaysExpiry"]!));
            }
            else
            {
                expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:AccessTokenMinutesExpiry"]!));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenPair GenerateTokenPair(string userId)
        {
            var accessToken = GenerateToken(userId, TokenType.AccessToken);
            var refreshToken = GenerateToken(userId, TokenType.RefreshToken);

            return new TokenPair(accessToken, refreshToken);
        }

        public string GetUserIdFromToken(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            var userId = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            return userId;
        }

        public bool CheckIfTokenValid(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = key,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GenerateAccessTokenFromRefreshToken(string refreshToken)
        {
            return GenerateToken(GetUserIdFromToken(refreshToken), TokenType.AccessToken);
        }
    }
}
