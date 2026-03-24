using Tfs.Api.Application.Dtos.Sectors;
namespace Tfs.Api.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISectorService
    {
		Task<SectorGetDto> CreateAsync(SectorPostDto sector);
		Task<IList<SectorGetDto>> GetAllAsync();
		Task<SectorGetDto> GetAsync(int id);
		Task<SectorGetDto> GetByNameAsync(string name);
	}
}
