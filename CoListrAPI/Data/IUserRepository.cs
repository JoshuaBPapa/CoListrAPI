using CoListrAPI.Domain.Entities;

namespace CoListrAPI.Data
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
        Task<User?> GetByShareCodeAsync(int shareCode, CancellationToken ct);
    }
}
