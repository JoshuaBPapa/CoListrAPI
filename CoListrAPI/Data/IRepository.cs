using CoListrAPI.Domain.Entities;

namespace CoListrAPI.Data
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task<TEntity?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task Add(TEntity entity, CancellationToken cancellationToken);
        Task Update(TEntity entity, CancellationToken cancellationToken);
        Task Delete(TEntity entity, CancellationToken cancellationToken);
    }
}
