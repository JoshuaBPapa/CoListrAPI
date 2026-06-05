using CoListrAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoListrAPI.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly CoListrDbContext _context;

        public Repository(CoListrDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(id);
            return await _context.Set<TEntity>().FindAsync([guid], cancellationToken);
        }

        public async Task Add(TEntity entity, CancellationToken cancellationToken)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(TEntity entity, CancellationToken cancellationToken)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}