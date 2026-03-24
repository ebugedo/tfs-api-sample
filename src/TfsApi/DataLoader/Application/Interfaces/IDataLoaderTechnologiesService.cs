using Tfs.Api.Application.Dtos.Technologies;

namespace DataLoader.Application.Interfaces
{
    public interface IDataLoaderTechnologiesService
    {
        Task Load();
        Task<TechnologyGetDto> GetTechnologyByNameAsync(string technologyName);
    }
}
