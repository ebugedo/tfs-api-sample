using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using Tfs.Api.Domain.Interfaces;
using Tfs.Api.Infrastructure.DbContexts;

namespace Tfs.Api.Infrastructure.Repositories
{
    public class EFRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : EntityBase<TKey>
	{
		public TfsApiDbContext DbContext { get; private set; }
		protected readonly DbSet<TEntity> _table;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dbContext"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public EFRepository(TfsApiDbContext dbContext)
		{
			this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}
		public virtual async Task<TEntity> AddAsync(TEntity entity)
		{
			var result = this.DbContext.Set<TEntity>().Add(entity);

			if (result.State == EntityState.Added)
			{
				return await Task.FromResult(result.Entity);
			}
			else throw new InvalidOperationException("Entity was not added");
		}
		public virtual async Task<bool> DeleteAsync(TKey key)
		{
			var removed = false;
			// Buscar la entidad por su ID
			var entity = await this.DbContext.Set<TEntity>().FindAsync(key);

			if (entity != null)
			{
				// Si la entidad se encontró, elimínala
				this.DbContext.Set<TEntity>().Remove(entity);
				removed = true;
				//await this.DbContext.SaveChangesAsync();
			}
			return removed;
		}
		public virtual async Task<IList<TEntity>> RetrieveAllAsync()
		{
			var items = this.DbContext.Set<TEntity>().AsNoTracking();
			var result = await items.ToListAsync();
			//DbContext.ChangeTracker.Clear();
			return result;
		}

		public virtual async Task<TEntity> RetrieveByKeyAsync(TKey key, bool asNoTracking = true)
		{
			var entity = await this.DbContext.Set<TEntity>().FindAsync(key);
			if (entity == null)
			{
				return null;
			}
			if (asNoTracking)
			{
				DbContext.ChangeTracker.Clear();
			}
			return entity;
		}
		public virtual async Task<IQueryable<TEntity>> RetrieveAllAsync(List<Expression<Func<TEntity, object>>> includes)
		{
			var query = this.DbContext
				.Set<TEntity>()
				.AsNoTracking()
				.AsQueryable();

			includes.ForEach(include =>
			{
				query = query.Include(include);
			});
			//DbContext.ChangeTracker.Clear();
			return await Task.FromResult(query);
		}
		public virtual async Task<TEntity> RetrieveByKeyAsync(TKey key, List<Expression<Func<TEntity, object>>> includes, bool asNoTracking = true)
		{
			var entity = await DbContext.Set<TEntity>().FindAsync(key);
			if (entity == null)
			{
				return null;
			}
			foreach (var include in includes)
			{
				if (include != null)
				{
					var navigationPropertyName = GetProperty(include);
					if (IsReference(navigationPropertyName))
					{
						await DbContext.Entry(entity).Reference(GetProperty(include)).LoadAsync();
					}
					else
					{
						await DbContext.Entry(entity).Collection(GetProperty(include)).LoadAsync();
					}
				}
			}
			if (asNoTracking)
			{
				DbContext.ChangeTracker.Clear();
			}
			return entity;
		}
		public virtual async Task<TEntity> UpdateAsync(TEntity entity)
		{
			// Marca la entidad como modificada en el contexto para que se actualice en la base de datos
			this.DbContext.Entry(entity).State = EntityState.Modified;

			// Guarda los cambios en la base de datos
			//await this.DbContext.SaveChangesAsync();

			return entity;
		}

		private string GetProperty(Expression<Func<TEntity, object>> expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}
			if (!IsValid(expression))
			{
				throw new ArgumentException(nameof(expression));
			}
			var navigationProperty = expression.ToString().Substring(expression.ToString().LastIndexOf('.') + 1);
			return navigationProperty;
		}

		public async Task<IQueryable<TEntity>> RetrieveByExpressionAsync(Expression<Func<TEntity, bool>> expression, bool asNoTracking = true)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}
			if (!IsValid(expression))
			{
				throw new ArgumentException(nameof(expression));
			}
			var query = this.DbContext.Set<TEntity>().Where(expression);
			if (asNoTracking)
			{
				query = query.AsNoTracking();
			}
			query = query.AsQueryable();

			return await Task.FromResult(query);
		}
		public async Task<IQueryable<TEntity>> RetrieveByExpressionAsync(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, object>>> includes, bool asNoTracking = true)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}
			if (!IsValid(expression))
			{
				throw new ArgumentException(nameof(expression));
			}
			var query = this.DbContext.Set<TEntity>().Where(expression);
			if (asNoTracking)
			{
				query = query.AsNoTracking();
			}
			includes.ForEach(include =>
			{
				query = query.Include(include);
			});
			query = query.AsQueryable();

			return await Task.FromResult(query);
		}

		public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
		{
			if (expression == null || !IsValid(expression))
			{
				throw new ArgumentNullException(nameof(expression));
			}
			var entitiesToDelete = RetrieveByExpressionAsync(expression, false);
			this.DbContext.Remove(entitiesToDelete);
			return true;
		}
		private bool IsValid(Expression<Func<TEntity, bool>> expression)
		{
			//return Enumerable.Count(expression.ToString(), c => c == '.') == 1;
			return true;
		}
		private bool IsValid(Expression<Func<TEntity, object>> expression)
		{
			//return Enumerable.Count(expression.ToString(), c => c == '.') == 1;
			return true;
		}


		//private bool IsReference(string entity)
		//{
		//	var properties = DbContext.GetType().GetProperties();
		//	foreach (var property in properties)
		//	{
		//		if (property.Name == entity)
		//		{
		//			// Collection
		//			if (property.PropertyType.IsGenericType &&
		//		property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
		//			{
		//				return false;
		//			}
		//			// Reference
		//			else if (typeof(EntityEntry).IsAssignableFrom(property.PropertyType))
		//			{
		//				return true;
		//			}
		//		}
		//	}
		//	return false;
		//}
		private bool IsReference(string entityName)
		{
			IModel model = this.DbContext.Model;

			foreach (var entityType in model.GetEntityTypes())
			{
				//var entityTypeName = entityType.Name.ToString().Substring(entityType.Name.ToString().LastIndexOf('.') + 1);
				//if (entityTypeName == nameof(TEntity))
				//{
				foreach (var navigation in entityType.GetNavigations())
				{
					if (navigation.Name == entityName)
					{
						if (navigation.IsCollection)
						{
							return false;
						}
						else
						{
							return true;
						}
					}
				}
			}
			return false;
			//}
		}


	}
}

