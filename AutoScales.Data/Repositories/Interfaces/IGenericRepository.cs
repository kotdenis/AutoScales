using AutoScales.Data.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace AutoScales.Data.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAllAsync(CancellationToken ct);
        Task<TEntity> GetByIdAsync(int id, CancellationToken ct);
        Task CreateAsync(TEntity entity, CancellationToken ct);
        Task<bool> UpdateAsync<T>(T entity, CancellationToken ct) where T : BaseEntity;
        Task<bool> SoftDeleteAsync<T>(T entity, CancellationToken ct) where T : BaseEntity;
        Task<TEntity> FindAsync(Func<TEntity, bool> predicate, CancellationToken ct);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
        Task CommitAsync(IDbContextTransaction transaction, CancellationToken ct);
        Task RollBackAsync(IDbContextTransaction transaction, CancellationToken ct);
        Task<List<TEntity>> FindAllAsync(Func<TEntity, bool> predicate, CancellationToken ct);
    }
}
