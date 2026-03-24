using System.Linq.Expressions;
using Domain.Common;

namespace Tfs.Api.Domain.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
	{
		Task<TEntity> AddAsync(TEntity entity);
		Task<bool> DeleteAsync(TKey key);
		Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression);
		Task<TEntity> RetrieveByKeyAsync(TKey key, bool asNoTracking=true);
		Task<TEntity> RetrieveByKeyAsync(TKey key, List<Expression<Func<TEntity, object>>> includes, bool asNoTracking = true);
		Task<IList<TEntity>> RetrieveAllAsync();
		Task<IQueryable<TEntity>> RetrieveAllAsync(List<Expression<Func<TEntity, object>>> includes);
		Task<IQueryable<TEntity>> RetrieveByExpressionAsync(Expression<Func<TEntity, bool>> expression, bool asNoTracking = true);
		Task<IQueryable<TEntity>> RetrieveByExpressionAsync(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, object>>> includes, bool asNoTracking = true);
		Task<TEntity> UpdateAsync(TEntity entity);
	}
}
