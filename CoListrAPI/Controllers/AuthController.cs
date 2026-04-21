using CoListrAPI.Domain.Auth;
using CoListrAPI.Domain.Result;
using CoListrAPI.DTOs.Auth;
using CoListrAPI.DTOs.User;
using CoListrAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoListrAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService AuthService, IConfiguration configuration)
        {
            _authService = AuthService;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AuthResponseDto>> SignUp(AuthSignUpDto dto, CancellationToken ct)
        {
            var result = await _authService.SignUpAsync(dto, ct);

            if (result.Error == "Username already taken") return Conflict(new ProblemDetails
            {
                Title = "Conflict",
                Detail = "A user with this username already exists.",
                Status = StatusCodes.Status409Conflict,
                Extensions =
                {
                    ["errors"] = new Dictionary<string, string[]>
                    {
                        { "username", new[] { "Username already exists." } }
                    }
                }
            }
            );

            return SuccessfulAuthResponse(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(AuthLoginDto dto, CancellationToken ct)
        {
            var result = await _authService.LoginAsync(dto, ct);

            if (result.Error == "Invalid username or password") return Unauthorized();

            return SuccessfulAuthResponse(result);
        }

        public ActionResult<AuthResponseDto> SuccessfulAuthResponse(Result<AuthResult> result)
        {
            Response.Cookies.Append("refreshToken", result.Value!.TokenPair.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDaysExpiry"]!)),
            });

            var User = new UserResponseDto
            {
                FirstName = result.Value.User.FirstName,
                LastName = result.Value.User.LastName,
                Username = result.Value.User.Username,
                ShareCode = result.Value.User.ShareCode
            };

            return Ok(new
            {
                accessToken = result.Value.TokenPair.AccessToken,
                user = User
            });
        }
    }
}
