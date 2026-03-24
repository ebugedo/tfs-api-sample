using Tfs.Api.Application.Dtos.Clients;

namespace DataLoader.Application.Interfaces
{
    public interface IDataLoaderClientsService
	{
        Task Load();
        Task<ClientGetDto> GetClientByNameAsync(string clientName);
    }
}
