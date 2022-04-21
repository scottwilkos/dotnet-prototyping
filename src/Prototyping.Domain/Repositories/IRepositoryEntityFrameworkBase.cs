using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Prototyping.Domain.Repositories
{
    public interface IRepositoryEntityFrameworkBase<TEntity> where TEntity : class
    {
        Task<EntityEntry<TEntity>> Add(TEntity tournament, CancellationToken cancellationToken);
        Task<TEntity> FirstOrDefault(CancellationToken cancellationToken);
        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        IQueryable<TEntity> GetAll(CancellationToken cancellationToken);
        Task<int> Save(CancellationToken cancellationToken);
        Task<TEntity> SingleOrDefault(CancellationToken cancellationToken);
        Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<long> Count(CancellationToken cancellationToken);
    }
}