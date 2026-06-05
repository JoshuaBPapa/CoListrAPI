using CoListrAPI.Domain.Entities;
using CoListrAPI.Domain.Result;

namespace CoListrAPI.Services
{
    public interface IUserService
    {
        Task<Result<User>> GetUserByIdAsync(string userId, CancellationToken cancellationToken);
    }
}
