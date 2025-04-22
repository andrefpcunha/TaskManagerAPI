using Microsoft.EntityFrameworkCore;
using TaskManager.Infra.Interfaces.Repositories;
using TaskManager.Persistence.Contexts;

namespace TaskManager.Persistence.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
    where TEntity : class
    {
        protected readonly AppDbContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        public RepositoryBase(AppDbContext context)
        {
            DbContext = context;
            DbSet = DbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbSet.FindAsync([id, cancellationToken], cancellationToken);
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbSet.ToListAsync(cancellationToken);
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }

        public virtual void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            DbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public virtual async Task AddRangeAsync(IList<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }
    }
}