using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Prototyping.Domain.Repositories
{
    public abstract class RepositoryEntityFrameworkBase<TEntity> : IRepositoryEntityFrameworkBase<TEntity> where TEntity : class
    {
        private readonly DbContext context;

        private readonly DbSet<TEntity> entities;

        public RepositoryEntityFrameworkBase(DbContext context)
        {
            this.context = context;
            entities = context.Set<TEntity>();
        }

        public async Task<EntityEntry<TEntity>> Add(TEntity tournament, CancellationToken cancellationToken) => await entities.AddAsync(tournament, cancellationToken);

        public async Task<long> Count(CancellationToken cancellationToken) => await entities.CountAsync(cancellationToken);

        public async Task<TEntity> FirstOrDefault(CancellationToken cancellationToken) => await entities.FirstOrDefaultAsync(cancellationToken);
        
        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) => await entities.FirstOrDefaultAsync(predicate, cancellationToken);

        public IQueryable<TEntity> GetAll(CancellationToken cancellationToken) => entities.AsQueryable();

        public async Task<int> Save(CancellationToken cancellationToken) => await this.context.SaveChangesAsync(cancellationToken);

        public async Task<TEntity> SingleOrDefault(CancellationToken cancellationToken) => await entities.SingleOrDefaultAsync(cancellationToken);
        
        public async Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) => await entities.SingleOrDefaultAsync(predicate, cancellationToken);
    }
}