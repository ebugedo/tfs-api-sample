using Domain.AggregateModels.Projects;
using Tfs.Api.Domain.Interfaces;
namespace Domain.Interfaces
{
	public interface IProjectRepository : IRepository<Project, int>
	{
		Task<IList<Project>> RetrieveAllAsync();
		Task<Project> RetrieveByKeyAsync(int id);
		Task<int> SaveChangesAsync();
	}
}
