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
        public DbSet<ShoppingList> ShoppingLists { get; set; } = null!;
        public DbSet<ShoppingListUser> ShoppingListUser { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ShoppingListUser>().HasKey(slu => new { slu.ShoppingListId, slu.UserId });
        }

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
                if (entityEntry.Entity is not BaseEntityDateTime) continue;

                ((BaseEntityDateTime)entityEntry.Entity).UpdatedAt = now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntityDateTime)entityEntry.Entity).CreatedAt = now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
