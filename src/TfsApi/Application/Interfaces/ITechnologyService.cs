using Tfs.Api.Application.Dtos.Technologies;
namespace Tfs.Api.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITechnologyService
    {
		Task<TechnologyGetDto> CreateAsync(TechnologyPostDto technology);
		Task<IList<TechnologyGetDto>> GetAllAsync();
		Task<TechnologyGetDto> GetAsync(int id);
		Task<TechnologyGetDto> GetByNameAsync(string name);
	}
}
