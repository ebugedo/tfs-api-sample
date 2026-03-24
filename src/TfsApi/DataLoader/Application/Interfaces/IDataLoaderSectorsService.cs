using Tfs.Api.Application.Dtos.Sectors;

namespace DataLoader.Application.Interfaces
{
    public interface IDataLoaderSectorsService
	{
        Task Load();
        Task<SectorGetDto> GetSectorByNameAsync(string sectorName);
    }
}
