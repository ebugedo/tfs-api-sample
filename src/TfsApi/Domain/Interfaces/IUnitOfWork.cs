namespace Tfs.Api.Domain.Interfaces
{
	public interface IUnitOfWork: IDisposable
	{
		Task<int> SaveChangesAsync();
	}
}
