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
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IWebHostEnvironment _environment;

        public AuthController(IAuthService authService, IConfiguration configuration, IJwtTokenService jwtTokenService, IWebHostEnvironment environment)
        {
            _authService = authService;
            _configuration = configuration;
            _jwtTokenService = jwtTokenService;
            _environment = environment;
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
                SameSite = _environment.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenDaysExpiry"]!)),
            });

            var User = new UserResponseDto
            {
                Id = result.Value.User.Id,
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

        [HttpGet("token")]
        public ActionResult<AccessTokenRefreshResponseDto> AccessTokenRefresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken) || !_jwtTokenService.CheckIfTokenValid(refreshToken)) return Unauthorized();

            var newAccessToken = _jwtTokenService.GenerateAccessTokenFromRefreshToken(refreshToken);

            return Ok(new
            {
                AccessToken = newAccessToken
            });
        }

        [HttpPost("logout")]
        public ActionResult Logout()
        {
            if (Request.Cookies["refreshToken"] != null)
            {
                Response.Cookies.Delete("refreshToken", new CookieOptions
                {
                    SameSite = _environment.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Lax,
                    Secure = true,
                    HttpOnly = true
                });
            }

            return NoContent();
        }
    }
}
