using CoListrAPI.DTOs.User;
using CoListrAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoListrAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetMe(CancellationToken ct)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            var result = await _userService.GetUserByIdAsync(userIdClaim, ct);

            if (result.Value == null) return NotFound();

            var user = new UserResponseDto
            {
                Id = result.Value.Id,
                FirstName = result.Value.FirstName,
                LastName = result.Value.LastName,
                Username = result.Value.Username,
                ShareCode = result.Value.ShareCode
            };

            return Ok(user);
        }
    }
}
