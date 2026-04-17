using CoListrAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoListrAPI.Data
{
    public class CoListrDbContext : DbContext
    {
        public CoListrDbContext(DbContextOptions<CoListrDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
    }
}
