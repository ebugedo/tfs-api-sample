using Tfs.Api.Application.Dtos.Projects;
namespace Tfs.Api.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProjectService
    {
		Task<ProjectGetDto> CreateAsync(ProjectPostDto project);
		Task<IList<ProjectGetDto>> GetAllAsync();
		Task<ProjectGetDto> GetAsync(int id);
	}
}
