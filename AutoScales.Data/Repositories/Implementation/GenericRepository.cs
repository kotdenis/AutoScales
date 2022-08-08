using AutoScales.Data.Entities;
using AutoScales.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace AutoScales.Data.Repositories.Implementation
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        private readonly ScalesDbContext _scalesDbContext;

        public GenericRepository(ScalesDbContext scalesDbContext)
        {
            _scalesDbContext = scalesDbContext;
        }

        public Task<IQueryable<TEntity>> GetAllAsync(CancellationToken ct)
        {
            var entities = _scalesDbContext.Set<TEntity>().AsQueryable();
            if (entities.Any())
                return Task.FromResult(entities);
            else
                return Task.FromResult(new List<TEntity>().AsQueryable());
        }

        public async Task<TEntity> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _scalesDbContext.Set<TEntity>().FindAsync(id, ct);
            if (entity == null)
                throw new NullReferenceException("There is no such entity with this id.");
            else
                return entity;
        }

        public async Task CreateAsync(TEntity entity, CancellationToken ct)
        {
            if (entity == null)
                throw new NullReferenceException("There is no such entity with this id.");
            _scalesDbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await _scalesDbContext.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync<T>(T entity, CancellationToken ct) where T : BaseEntity
        {
            if (entity == null)
                return false;
            _scalesDbContext.ChangeTracker.Clear();
            _scalesDbContext.Set<T>().Update(entity);
            await _scalesDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync<T>(T entity, CancellationToken ct) where T : BaseEntity
        {
            if (entity == null)
                return false;
            var ent = _scalesDbContext.Set<T>().Where(t => t.Id == entity.Id).FirstOrDefault();
            if (ent != null)
                ent.IsDeleted = true;
            await _scalesDbContext.SaveChangesAsync();
            return true;
        }

        public Task<TEntity> FindAsync(Func<TEntity, bool> predicate, CancellationToken ct)
        {
            var ent = _scalesDbContext.Set<TEntity>().Where(predicate).FirstOrDefault();
            if (ent != null)
                return Task.FromResult(ent);
            else
                return Task.FromResult(new TEntity());
        }

        public Task<List<TEntity>> FindAllAsync(Func<TEntity, bool> predicate, CancellationToken ct)
        {
            var ents = _scalesDbContext.Set<TEntity>().Where(predicate).ToList();
            if (ents != null)
                return Task.FromResult(ents);
            return Task.FromResult(new List<TEntity>());
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct)
        {
            return _scalesDbContext.Database.BeginTransactionAsync(ct);
        }

        public Task CommitAsync(IDbContextTransaction transaction, CancellationToken ct)
        {
            return transaction.CommitAsync(ct);
        }

        public Task RollBackAsync(IDbContextTransaction transaction, CancellationToken ct)
        {
            return transaction.RollbackAsync(ct);
        }
    }
}
