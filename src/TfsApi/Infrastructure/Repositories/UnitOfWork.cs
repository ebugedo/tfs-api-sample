using Microsoft.EntityFrameworkCore;
using Tfs.Api.Domain.Interfaces;
using Tfs.Api.Infrastructure.DbContexts;
namespace Tfs.Api.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private TfsApiDbContext _dbContext;
		public UnitOfWork(TfsApiDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}

		public async Task<int> SaveChangesAsync()
		{
			int saved = 0;
			try
			{
				saved = await _dbContext.SaveChangesAsync();
				_dbContext.ChangeTracker.Entries().ToList().ForEach(entry => entry.State = EntityState.Detached);
			}
			catch (Exception exception)
			{
				//throw GetDomainException(exception);
				throw;
			}
			return saved;
		}
	}
}

