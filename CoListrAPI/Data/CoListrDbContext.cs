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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));
            
            var now = DateTime.UtcNow;

            foreach (var entityEntry in entries)
            {

                ((BaseEntity)entityEntry.Entity).UpdatedAt = now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
