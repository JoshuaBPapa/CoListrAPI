using CoListrAPI.DTOs.Auth;
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

            Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDaysExpiry"]!)),
            });

            return Ok(new
            {
                accessToken = result.Value.AccessToken
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(AuthLoginDto dto, CancellationToken ct)
        {
            var result = await _authService.LoginAsync(dto, ct);

            if (result.Error == "Invalid username or password") return Unauthorized();

            Response.Cookies.Append("refreshToken", result.Value!.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDaysExpiry"]!)),
            });

            return Ok(new
            {
                accessToken = result.Value.AccessToken
            });
        }
    }
}
