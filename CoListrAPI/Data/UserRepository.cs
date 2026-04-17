using CoListrAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoListrAPI.Data
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CoListrDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        }

        public async Task<User?> GetByShareCodeAsync(int shareCode, CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.ShareCode == shareCode, cancellationToken);
        }
    }
}