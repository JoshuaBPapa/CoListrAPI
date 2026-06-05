using CoListrAPI.Data;
using CoListrAPI.Domain.Entities;
using CoListrAPI.Domain.Result;

namespace CoListrAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _users;

        public UserService(IUserRepository users)
        {
            _users = users;
        }

        public async Task<Result<User>> GetUserByIdAsync(string userId, CancellationToken ct)
        {
            var user = await _users.GetByIdAsync(userId, ct);

            if (user == null)
            {
                return new Result<User>
                {
                    IsSuccess = false,
                    Value = null,
                    Error = "User not found"
                };
            }

            return new Result<User>
            {
                IsSuccess = true,
                Value = user,
                Error = null
            };
        }
    }
}
